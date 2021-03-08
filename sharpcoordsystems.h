#pragma once
#include "coordsystems.h"

extern "C" COORDSYSTEMS_API void ICStoGCS(double julianDate, double* x, double* y, double* z);
extern "C" COORDSYSTEMS_API void GCStoICS(double julianDate, double* x, double* y, double* z);
extern "C" COORDSYSTEMS_API void TCStoGCS(double latitude, double longitude, double* x, double* y, double* z);
extern "C" COORDSYSTEMS_API void GCStoTCS(double latitude, double longitude, double* x, double* y, double* z);
extern "C" COORDSYSTEMS_API void HICStoICS(double julianDate, double* x, double* y, double* z);
extern "C" COORDSYSTEMS_API void ICStoHICS(double julianDate, double* x, double* y, double* z);

extern "C" COORDSYSTEMS_API void turnX(double* x, double* y, double* z, double angle);
extern "C" COORDSYSTEMS_API void turnY(double* x, double* y, double* z, double angle);
extern "C" COORDSYSTEMS_API void turnZ(double* x, double* y, double* z, double angle);
extern "C" COORDSYSTEMS_API void turnAxis(double* x, double* y, double* z,
	                                      double axisX, double axisY, double axisZ, double angle);


// This is int to marsahll correctly
extern "C" COORDSYSTEMS_API int isCrossingEarth(double x1, double y1, double z1,
	                                             double x2, double y2, double z2);