#pragma once

#include "../common/types.h"

class ArcGIS
{
	int m_iNumRows;
	int m_iNumColumns;
	float m_fLeft;
	float m_fTop;
	float m_fCellsize;
	s16 *mp_Data;
public:
	void Reset();
	float GetHeight(float fLong, float fLat) const;
	bool LoadFile(LPCWSTR pFileName);
	ArcGIS();
	~ArcGIS();
};

