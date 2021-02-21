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