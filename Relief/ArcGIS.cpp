#include "stdafx.h"
#include "ArcGIS.h"

extern void Debugf(const char *pFmt, ...);
extern void wDebugf(const WCHAR *pFmt, ...);

ArcGIS::ArcGIS()
: m_iNumRows(0)
, m_iNumColumns(0)
, m_fLeft(0.0f)
, m_fTop(0.0f)
, m_fCellsize(0.0f)
, mp_Data(0)
{
}


ArcGIS::~ArcGIS()
{
	delete[]mp_Data;
}

void ArcGIS::Reset()
{
	m_iNumRows = m_iNumColumns = 0;
	m_fLeft = m_fTop = m_fCellsize = 0.0f;
	delete[]mp_Data;
	mp_Data = 0;
}

bool ArcGIS::LoadFile(LPCWSTR pFileName)
{
	Reset();
	if (!pFileName || !pFileName[0])
	{
		Debugf("ArcGIS::LoadFile called with invalid filename");
		return false;
	}

	HANDLE hFile = CreateFile(pFileName, GENERIC_READ, FILE_SHARE_READ, 0, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, 0);
	if (hFile == INVALID_HANDLE_VALUE)
	{
		wDebugf(L"ArcGIS::LoadFile - could not open file '%s'", pFileName);
		return false;
	}

	//Example header
	/*	ncols        2275
		nrows        1924
		xllcorner - 118.842083333333
		yllcorner    32.370416666667
		cellsize     0.000833333333
	*/

	DWORD dwFileSizeHigh = 50 * 1024 * 1024;
	DWORD dwFileSize = GetFileSize(hFile, &dwFileSizeHigh);
	if (dwFileSize == 0)
	{
		wDebugf(L"ArcGIS::LoadFile - filesize invalid for '%s'", pFileName);
		CloseHandle(hFile);
		return false;
	}

	wDebugf(L"Loading '%s'...", pFileName);

	//Read the data
	char *pFileData = new char[dwFileSize];
	DWORD dwDataRead = 0;
	ReadFile(hFile, pFileData, dwFileSize, &dwDataRead, 0);
	CloseHandle(hFile);

	char *pReader = pFileData;
	char *pContext = 0;

	char *pWord=0;
	pWord = strtok_s(pReader, " \n\r\t", &pContext);
	pWord = strtok_s(0, " \n\r\t", &pContext);
	m_iNumColumns = atoi(pWord);
	pWord = strtok_s(0, " \n\r\t", &pContext);
	pWord = strtok_s(0, " \n\r\t", &pContext);
	m_iNumRows = atoi(pWord);
	pWord = strtok_s(0, " \n\r\t", &pContext);
	pWord = strtok_s(0, " \n\r\t", &pContext);
	m_fLeft = (float)atof(pWord);
	pWord = strtok_s(0, " \n\r\t", &pContext);
	pWord = strtok_s(0, " \n\r\t", &pContext);
	m_fTop = (float)atof(pWord);
	pWord = strtok_s(0, " \n\r\t", &pContext);
	pWord = strtok_s(0, " \n\r\t", &pContext);
	m_fCellsize = (float)atof(pWord);

	if (!m_iNumColumns || !m_iNumRows)
	{
		goto FailHere;
	}
	//Now read the data
	int iToRead = m_iNumColumns * m_iNumRows;
	mp_Data = new s16[m_iNumColumns * m_iNumRows];

	for (int i = 0; i<iToRead; i++)
	{
		pWord = strtok_s(0, " \n\r\t", &pContext);
		mp_Data[i] = atoi(pWord);
	}

	delete []pFileData;

	wDebugf(L"...Loaded '%s'", pFileName);
	return true;

FailHere:;
	wDebugf(L"...FAILED Loading '%s'", pFileName);
	delete[]pFileData;
	return false;
}

float ArcGIS::GetHeight(float fLong, float fLat) const
{
	//Convert fLong / fLat to grid space
	fLong = (fLong - m_fLeft) / m_fCellsize;
	fLat = (fLat - m_fTop) / m_fCellsize;

	//Try flipping....
	fLat = (float)(m_iNumRows - 1) - fLat;

	//Clamp to edge of grid
	bool bBlendLong = true;
	bool bBlendLat = true;
	if (fLong < 0.0f)
	{
		fLong = 0.0f; 
		bBlendLong = false;
	}
	if (fLat < 0.0f)
	{
		fLat = 0.0f;
		bBlendLat = false;
	}
	if (fLong >(float)(m_iNumColumns - 1))
	{
		fLong = (float)(m_iNumColumns - 1);
		bBlendLong = false;
	}
	if (fLat > (float)(m_iNumRows - 1))
	{
		fLat = (float)(m_iNumRows - 1); 
		bBlendLat = false;
	}

	//Now calculate t values and offsets
	int iLat0 = int(fLat);
	float fLatT = fLat - (float)iLat0;
	int iLat1 = bBlendLat ? iLat0 + 1 : iLat0;
	int iLong0 = int(fLong);
	float fLongT = fLong - (float)iLong0;
	int iLong1 = bBlendLong ? iLong0 + 1 : iLong0;

	//Grab our four values
	float fVal00 = (float)mp_Data[iLat0 * m_iNumColumns + iLong0];
	float fVal01 = (float)mp_Data[iLat0 * m_iNumColumns + iLong1];
	float fVal10 = (float)mp_Data[iLat1 * m_iNumColumns + iLong0];
	float fVal11 = (float)mp_Data[iLat1 * m_iNumColumns + iLong1];

	//Blend long
	float fLat0 = fVal00 + (fVal01 - fVal00) * fLongT;
	float fLat1 = fVal10 + (fVal11 - fVal10) * fLongT;

	//Blend lat
	return fLat0 + (fLat1 - fLat0) * fLatT;
}
