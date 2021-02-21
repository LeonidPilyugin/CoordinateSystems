#include "pch.h"
#include "sharpcoordsystems.h"

#define PUT \
*x = vector.x;\
*y = vector.y;\
*z = vector.z;

void ICStoGCS(double julianDate, double* x, double* y, double* z)
{
	crd::RectangleVector vector = { *x, *y, *z };
	vector = crd::convertICStoGCS(vector, julianDate);
	PUT;
}

void GCStoICS(double julianDate, double* x, double* y, double* z)
{
	crd::RectangleVector vector = { *x, *y, *z };
	vector = crd::convertGCStoICS(vector, julianDate);
	PUT;
}

void TCStoGCS(double latitude, double longitude, double* x, double* y, double* z)
{
	crd::RectangleVector vector = { *x, *y, *z };
	vector = crd::convertTCStoGCS(latitude, longitude, vector);
	PUT;
}

void GCStoTCS(double latitude, double longitude, double* x, double* y, double* z)
{
	crd::RectangleVector vector = { *x, *y, *z };
	vector = crd::convertGCStoTCS(latitude, longitude, vector);
	PUT;
}

void HICStoICS(double julianDate, double* x, double* y, double* z)
{
	crd::RectangleVector vector = { *x, *y, *z };
	vector = crd::convertHICStoICS(vector, julianDate);
	PUT;
}

void ICStoHICS(double julianDate, double* x, double* y, double* z)
{
	crd::RectangleVector vector = { *x, *y, *z };
	vector = crd::convertICStoHICS(vector, julianDate);
	PUT;
}

void turnX(double* x, double* y, double* z, double angle)
{
	crd::RectangleVector vector = { *x, *y, *z };
	vector = crd::getRx(angle) * vector;
	PUT;
}

void turnY(double* x, double* y, double* z, double angle)
{
	crd::RectangleVector vector = { *x, *y, *z };
	vector = crd::getRy(angle) * vector;
	PUT;
}

void turnZ(double* x, double* y, double* z, double angle)
{
	crd::RectangleVector vector = { *x, *y, *z };
	vector = crd::getRz(angle) * vector;
	PUT;
}