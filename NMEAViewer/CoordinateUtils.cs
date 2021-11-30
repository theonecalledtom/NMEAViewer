using NMEAViewer;
using System;

class CoordinateUtils
{
	public const double R = 6371.0 * 1000.0;    // m
	public const double NMtoM = 1852.0;         //Nautical miles to meters
	public const double HoursToSeconds = 3600.0;
	public static double DistanceBetween(double fFromLong, double fFromLat, double fToLong, double fToLat)
	{
		double dLat = (fFromLat - fToLat) * AngleUtil.DegToRad;
		double dLon = (fFromLong - fToLong) * AngleUtil.DegToRad;
		double lat1 = fToLat * AngleUtil.DegToRad;
		double lat2 = fFromLat * AngleUtil.DegToRad;
		double fDLatOver2 = dLat * 0.5f;
		double fDLongOver2 = dLon * 0.5f;
		double a = Math.Sin(fDLatOver2) * Math.Sin(fDLatOver2) +
			Math.Sin(fDLongOver2) * Math.Sin(fDLongOver2) * Math.Cos(lat1) * Math.Cos(lat2);
		double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

		double d = R * c;
		return d;
	}

	//From https://stackoverflow.com/questions/3932502/calculate-angle-between-two-latitude-longitude-points
	public static double HeadingTo(double fFromLong, double fFromLat, double fToLong, double fToLat)
    {
		double dLon = (fToLong - fFromLong);

		double y = Math.Sin(dLon) * Math.Cos(fToLat);
		double x = Math.Cos(fFromLat) * Math.Sin(fToLat) - Math.Sin(fFromLat)
				* Math.Cos(fToLat) * Math.Cos(dLon);

		double brng = Math.Atan2(y, x);
		double brngDegrees = brng * AngleUtil.RadToDeg;
		brngDegrees = AngleUtil.ContainAngle0To360(brngDegrees);
		return brngDegrees;
	}
};