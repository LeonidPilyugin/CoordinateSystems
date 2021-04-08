#pragma once

#ifdef COORDSYSTEMS_EXPORTS
#define COORDSYSTEMS_API __declspec(dllexport)
#else
#define COORDSYSTEMS_API __declspec(dllimport)
#endif

#include <iostream>
#include <cmath>
#include "sofa.h"

namespace crd
{
	static const double EARTH_POLAR_COMPRESSION = 1/298.3;
	static const double EARTH_MEAN_RADIUS = 6371000.0;
	static const double EARTH_MAX_RADIUS = 6378245.0;
	static const double EARTH_MIN_RADIUS = EARTH_MAX_RADIUS * (1 - EARTH_POLAR_COMPRESSION);

	class COORDSYSTEMS_API RotationMatrix {
		friend COORDSYSTEMS_API RotationMatrix operator+(const RotationMatrix&, const RotationMatrix&);
		friend COORDSYSTEMS_API RotationMatrix operator-(const RotationMatrix&, const RotationMatrix&);
		friend COORDSYSTEMS_API RotationMatrix operator*(const RotationMatrix&, double);
		friend COORDSYSTEMS_API RotationMatrix operator*(double, const RotationMatrix&);
		friend COORDSYSTEMS_API RotationMatrix operator*(const RotationMatrix&, const RotationMatrix&);
		friend COORDSYSTEMS_API RotationMatrix operator/(const RotationMatrix&, double);
		friend COORDSYSTEMS_API RotationMatrix getTransposed(const RotationMatrix&);
		friend COORDSYSTEMS_API RotationMatrix getInversed(const RotationMatrix&);
		friend COORDSYSTEMS_API std::ostream& operator<<(std::ostream& out, const RotationMatrix&);

	protected:
		double** matrix;

	public:
		static const unsigned int size = 3;
		RotationMatrix();
		RotationMatrix(const RotationMatrix&);
		RotationMatrix(const double**);
		RotationMatrix(const double(&matrix)[size][size]);
		~RotationMatrix();

		void set(const double**);
		void set(const double(&matrix)[size][size]);
		void set(const RotationMatrix&);

		RotationMatrix operator+=(const RotationMatrix&);
		RotationMatrix operator-=(const RotationMatrix&);
		RotationMatrix operator*=(double);
		RotationMatrix operator*=(const RotationMatrix&);
		RotationMatrix operator/=(double);

		double& operator()(unsigned int, unsigned int) const;
		RotationMatrix& operator=(const RotationMatrix&);
		RotationMatrix& operator=(const double(&matrix)[size][size]);
		RotationMatrix& operator=(const double**);
		RotationMatrix operator-() const;

		void transpose();

		void swapLines(unsigned int, unsigned int);
		void swapColumns(unsigned int, unsigned int);

		double getDeterminant() const;
		void inverse();
	};

	struct COORDSYSTEMS_API RectangleVector
	{
		double x = 0.0;
		double y = 0.0;
		double z = 0.0;

		RectangleVector operator*=(double value);
		RectangleVector operator*=(const RectangleVector& vector);
		RectangleVector operator/=(double value);
		RectangleVector operator+=(const RectangleVector& vector);
		RectangleVector operator-=(const RectangleVector& vector);
		RectangleVector operator-();
	};

	struct COORDSYSTEMS_API PolarVector
	{
		double length = 0.0;
		double theta = 0.0;
		double phi = 0.0;

		PolarVector operator*=(double value);
		PolarVector operator*=(const PolarVector& vector);
		PolarVector operator/=(double value);
		PolarVector operator+=(const PolarVector& vector);
		PolarVector operator-=(const PolarVector& vector);
		PolarVector operator-();
	};

	struct COORDSYSTEMS_API Calendar
	{
		int year;
		int month;
		int day;
		int hour;
		int minute;
		double second;
	};

	extern COORDSYSTEMS_API std::ostream & operator<< (std::ostream & out, const PolarVector & vector);
	extern COORDSYSTEMS_API std::ostream & operator<< (std::ostream & out, const RectangleVector & vector);

	extern COORDSYSTEMS_API RectangleVector operator*(const RectangleVector&, const RectangleVector&);
	extern COORDSYSTEMS_API RectangleVector operator*(const RotationMatrix&, const RectangleVector&);
	extern COORDSYSTEMS_API RectangleVector operator+(const RectangleVector&, const RectangleVector&);
	extern COORDSYSTEMS_API RectangleVector operator-(const RectangleVector&, const RectangleVector&);
	extern COORDSYSTEMS_API RectangleVector operator*(const RectangleVector&, double value);
	extern COORDSYSTEMS_API RectangleVector operator/(const RectangleVector&, double value);
	extern COORDSYSTEMS_API double multiplyScalar(const RectangleVector&, const RectangleVector&);


	extern COORDSYSTEMS_API PolarVector operator*(const PolarVector&, const PolarVector&);
	extern COORDSYSTEMS_API PolarVector operator+(const PolarVector&, const PolarVector&);
	extern COORDSYSTEMS_API PolarVector operator-(const PolarVector&, const PolarVector&);
	extern COORDSYSTEMS_API PolarVector operator*(const PolarVector&, double value);
	extern COORDSYSTEMS_API PolarVector operator/(const PolarVector&, double value);

	extern COORDSYSTEMS_API double length(const RectangleVector&);
	extern COORDSYSTEMS_API double length(const PolarVector&);

	extern COORDSYSTEMS_API RectangleVector convertPolarToRectangle(const PolarVector&);
	extern COORDSYSTEMS_API PolarVector convertRectangleToPolar(const RectangleVector&);

	// Converts Grigorian calendar to Julian date
	// Argument — Grigorian calendar
	// Example: double a = coords::convertGCtoJD(calendar);
	// NOTE: Julian date must be > 0.0
	extern COORDSYSTEMS_API double convertGCtoJD(const Calendar & grigorianCalendar);

	// Converts Grigorian calendar to Julian day number
	// Argument — Grigorian calendar
	// Example: unsigned int a = coords::convertGCtoJDN(calendar);
	// NOTE: Julian date must be > 0.0
	extern COORDSYSTEMS_API unsigned int convertGCtoJDN(const Calendar & grigorianCalendar);

	// Converts Julian date to Grigorian calendar
	// Argument — Julian date
	// Example: coords::Calendar a = coords::convertGCtoJDN(julianDate);
	// NOTE: Julian date must be > 0.0
	extern COORDSYSTEMS_API Calendar convertJDtoGC(double julianDate);

	extern COORDSYSTEMS_API std::ostream & operator<< (std::ostream & out, const Calendar & calendar);

	// Returns converted vector from first coordinate system (CS1) to second (CS2)
	// First argument — vector in CS1 (vector)
	// Second argument — rotation matrix from CS1 to CS2
	// Third argument — vector in CS2 to CS1
	// Example: coords::RectangleVector v = coords::convertTo(vector, rotationMatrix, vector12);
	extern COORDSYSTEMS_API RectangleVector convertTo(const RectangleVector& vector, const RotationMatrix& matrix,
		const RectangleVector& vector12);

	// Returns vector in GCS from latitude and longitude (in degrees)
	// First argument — latitude
	// Second argument — longitude
	// Example: coords::RectangleVector v = coords::convertGeodesicCoordsToVector(latitude, longitude);
	extern COORDSYSTEMS_API RectangleVector convertGeodesicCoordsToVector(double latitude, double longitude);



	// Returns Earth precession rotation matrix by julian date
	// Argument — julian date
	// Example: coords::RotationMatrix m = coords::getPrecessionMatrix(julianDate);
	extern COORDSYSTEMS_API RotationMatrix getPrecessionMatrix(double julianDate);

	// Returns Earth nutation rotation matrix by julian date
	// Argument — julian date
	// Example: coords::RotationMatrix m = coords::getNutationMatrix(julianDate);
	extern COORDSYSTEMS_API RotationMatrix getNutationMatrix(double julianDate);

	// Returns Earth polar motion rotation matrix by julian date
	// Argument — julian date
	// Example: coords::RotationMatrix m = coords::getNutationMatrix(julianDate);
	// NOTE: in this function coordinates of north pole are constant
	extern COORDSYSTEMS_API RotationMatrix getPolarMotionMatrix(double julianDate);

	// Returns Earth rotation rotation matrix by julian date
	// Argument — julian date
	// Example: coords::RotationMatrix m = coords::getEarthRotationMatrix(julianDate);
	extern COORDSYSTEMS_API RotationMatrix getEarthRotationMatrix(double julianDate);

	// Returns rotation matrix to convert vector from ICS to GCS
	// Argument — julian date
	// Example: coords::RotationMatrix m = coords::ICStoGCSmatrix(julianDate);
	// NOTE: in this function coordinates of north pole are constant
	extern COORDSYSTEMS_API RotationMatrix ICStoGCSmatrix(double julianDate);

	// Returns rotation matrix to convert vector from GCS to ICS
	// Argument — julian date
	// Example: coords::RotationMatrix m = coords::GCStoICSmatrix(julianDate);
	// NOTE: in this function coordinates of north pole are constant
	extern COORDSYSTEMS_API RotationMatrix GCStoICSmatrix(double julianDate);

	// Returns vector converted from ICS to GCS
	// First argument — vector in ICS
	// Second argument — julian date
	// Example: coords::RectangleVector v = coords::convertICStoGCS(vector, julianDate);
	// NOTE: in this function coordinates of north pole are constant
	extern COORDSYSTEMS_API RectangleVector convertICStoGCS(const RectangleVector& vector, double julianDate);

	// Returns vector converted from GCS to ICS
	// First argument — vector in GCS
	// Second argument — julian date
	// Example: coords::RectangleVector v = coords::convertGCStoICS(vector, julianDate);
	// NOTE: in this function coordinates of north pole are constant
	extern COORDSYSTEMS_API RectangleVector convertGCStoICS(const RectangleVector& vector, double julianDate);



	// Returns rotation matrix to convert vector from GCS to TCS
	// First argument — latitude (in degrees)
	// Second argument — longitude (in degrees)
	// Example: coords::RotationMatrix m = coords::GCStoTCSmatrix(latitude, longitude);
	extern COORDSYSTEMS_API RotationMatrix GCStoTCSmatrix(double latitude, double longitude);

	// Returns rotation matrix to convert vector from GCS to TCS
	// Argument — vector in GCS to TCS
	// Example: coords::RotationMatrix m = coords::GCStoTCSmatrix(vector);
	extern COORDSYSTEMS_API RotationMatrix GCStoTCSmatrix(const RectangleVector& vector);

	// Returns rotation matrix to convert vector from TCS to GCS
	// First argument — latitude (in degrees)
	// Second argument — longitude (in degrees)
	// Example: coords::RotationMatrix m = coords::TCStoGCSmatrix(latitude, longitude);
	extern COORDSYSTEMS_API RotationMatrix TCStoGCSmatrix(double latitude, double longitude);

	// Returns rotation matrix to convert vector from TCS to GCS
	// Argument — vector in TCS to GCS
	// Example: coords::RotationMatrix m = coords::TCStoGCSmatrix(vector);
	extern COORDSYSTEMS_API RotationMatrix TCStoGCSmatrix(const RectangleVector& vector);

	// Returns rotation matrix to convert vector from HICS to ICS (heliocentric ICS)
	// Argument — Julian date
	// Example: coords::RotationMatrix m = coords::HICStoICSmatrix(julianDate);
	extern COORDSYSTEMS_API RotationMatrix HICStoICSmatrix(double julianDate);

	// Returns rotation matrix to convert vector from ICS to HICS (heliocentric ICS)
	// Argument — Julian date
	// Example: coords::RotationMatrix m = coords::ICStoHICSmatrix(julianDate);
	extern COORDSYSTEMS_API RotationMatrix ICStoHICSmatrix(double julianDate);

	// Returns vector from Earth to Sun in HICS (heliocentric ICS)
	// Argument — Julian date
	// Example: coords::RectangleVector = ICStoHICSvector(julianDate);
	extern COORDSYSTEMS_API RectangleVector ICStoHICSvector(double julianDate);

	// Returns vector from Sun to Earth in HICS (heliocentric ICS)
	// Argument — Julian date
	// Example: coords::RectangleVector = HICStoICSvector(julianDate);
	extern COORDSYSTEMS_API RectangleVector HICStoICSvector(double julianDate);

	// Returns vector converted from GCS to TCS
	// First argument — latitude (in degrees)
	// Second argument — longitude (in degrees)
	// Third — vector in GCS
	// Example: coords::RectangleVector v = coords::convertGCStoICS(latitude, longitude, vector);
	extern COORDSYSTEMS_API RectangleVector convertGCStoTCS(double latitude, double longitude, const RectangleVector& vector);

	// Returns vector converted from TCS to GCS
	// First argument — latitude (in degrees)
	// Second argument — longitude (in degrees)
	// Third — vector in TCS
	// Example: coords::RectangleVector v = coords::convertTCStoGCS(latitude, longitude, vector);
	extern COORDSYSTEMS_API RectangleVector convertTCStoGCS(double latitude, double longitude, const RectangleVector& vector);

	// Returns vector converted from GCS to TCS
	// First argument — vector in GCS
	// Second argument — vector in GCS to TCS
	// Example: coords::RectangleVector v = coords::convertGCStoTCS(vector, vector12);
	extern COORDSYSTEMS_API RectangleVector convertGCStoTCS(const RectangleVector& vector, const RectangleVector& vector12);

	// Returns vector converted from TCS to GCS
	// First argument — vector in TCS
	// Second argument — vector in TCS to GCS
	// Example: coords::RectangleVector v = coords::convertTCStoGCS(vector, vector12);
	extern COORDSYSTEMS_API RectangleVector convertTCStoGCS(const RectangleVector& vector, const RectangleVector& vector12);

	// Returns vector converted from HICS (heliocentric ICS) to ICS
	// First argument — vector in HICS
	// Second argument — Julian date
	// Example: coords::RectangleVector b = coords::convertHICStoICS(vector, julianDate);
	extern COORDSYSTEMS_API RectangleVector convertHICStoICS(const RectangleVector& vector, double julianDate);

	// Returns vector converted from ICS to HICS (heliocentric ICS)
	// First argument — vector in ICS
	// Second argument — Julian date
	// Example: coords::RectangleVector b = coords::convertICStoHICS(vector, julianDate);
	extern COORDSYSTEMS_API RectangleVector convertICStoHICS(const RectangleVector& vector, double julianDate);


	// Returns rotation matrix to turn vector around X axis
	// Argument — rotation angle
	// Example: coords::RotationMatrix = coords::getRx(angle);
	extern COORDSYSTEMS_API RotationMatrix getRx(double angle);

	// Returns rotation matrix to turn vector around Y axis
	// Argument — rotation angle
	// Example: coords::RotationMatrix = coords::getRy(angle);
	extern COORDSYSTEMS_API RotationMatrix getRy(double angle);

	// Returns rotation matrix to turn vector around Z axis
	// Argument — rotation angle
	// Example: coords::RotationMatrix = coords::getRz(angle);
	extern COORDSYSTEMS_API RotationMatrix getRz(double angle);

	// Returns rotation matrix to turn vector by Euler angles
	// First argument — precession angle
	// Second argument — nutation angle
	// Third argument — rotation angle
	// Example: coords::RotationMatrix = coords::getEulerRotationMatrix(precession, nutation, rotation);
	extern COORDSYSTEMS_API RotationMatrix getEulerRotationMatrix(double precession, double nutation, double rotation);

	extern COORDSYSTEMS_API RotationMatrix getAxisRottionMatrix(RectangleVector axis, double angle);
	extern COORDSYSTEMS_API RectangleVector turnAroundAxis(RectangleVector vector, RectangleVector axis, double angle);
	extern COORDSYSTEMS_API RectangleVector getDirectionVector(const RectangleVector& point1, const RectangleVector& point2);
	extern COORDSYSTEMS_API RectangleVector getProjection(const RectangleVector& vector1, const RectangleVector& vector2,
		                                                  const RectangleVector& point);
	// Vectors in ICS or GCS
	extern COORDSYSTEMS_API bool isInsideEarth(const RectangleVector& vector);
	extern COORDSYSTEMS_API bool isCrossingEarth(const RectangleVector& vector1, const RectangleVector& vector2);
}