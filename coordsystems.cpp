#include "pch.h"
#include "coordsystems.h"

#define COPY(matr) \
for (unsigned int i = 0; i < size; i++)\
	for (unsigned int j = 0; j < size; j++)\
		this->matrix[i][j] = matr[i][j];

namespace crd
{
	RotationMatrix operator+(const RotationMatrix& matrix1, const RotationMatrix& matrix2)
	{
		RotationMatrix result = RotationMatrix();
		for (unsigned int i = 0; i < result.size; i++)
			for (unsigned int j = 0; j < result.size; j++)
				result(i, j) = matrix1(i, j) + matrix2(i, j);

		return result;
	}

	RotationMatrix operator-(const RotationMatrix& matrix1, const RotationMatrix& matrix2)
	{
		RotationMatrix result = RotationMatrix();
		for (unsigned int i = 0; i < result.size; i++)
			for (unsigned int j = 0; j < result.size; j++)
				result(i, j) = matrix1(i, j) - matrix2(i, j);

		return result;
	}

	RotationMatrix operator*(const RotationMatrix& matrix, double m)
	{
		RotationMatrix result = RotationMatrix();
		for (unsigned int i = 0; i < result.size; i++)
			for (unsigned int j = 0; j < result.size; j++)
				result(i, j) = matrix(i, j) * m;

		return result;
	}

	RotationMatrix operator*(double m, const RotationMatrix& matrix)
	{
		return matrix * m;
	}

	RotationMatrix operator/(const RotationMatrix& matrix, double m)
	{
		RotationMatrix result = RotationMatrix();
		for (unsigned int i = 0; i < result.size; i++)
			for (unsigned int j = 0; j < result.size; j++)
				result(i, j) = matrix(i, j) / m;

		return result;
	}

	RotationMatrix operator*(const RotationMatrix& matrix1, const RotationMatrix& matrix2)
	{
		RotationMatrix result = RotationMatrix();
		double sum;
		for (unsigned int i = 0; i < result.size; i++)
		{
			for (unsigned int j = 0; j < result.size; j++)
			{
				sum = 0;
				for (unsigned int r = 0; r < matrix1.size; r++)
					sum += matrix1(i, r) * matrix2(r, j);
				result(i, j) = sum;
			}
		}

		return result;
	}

	std::ostream& operator<<(std::ostream& out, const RotationMatrix& matrix)
	{
		for (unsigned int i = 0; i < matrix.size; i++)
		{
			for (unsigned int j = 0; j < matrix.size; j++)
				out << matrix(i, j) << ",\t";
			out << std::endl;
		}
		return out;
	}

	RotationMatrix getTransposed(const RotationMatrix& matrix)
	{
		RotationMatrix result = RotationMatrix(matrix);
		result.transpose();
		return result;
	}

	RotationMatrix getInversed(const RotationMatrix& matrix)
	{
		RotationMatrix result = RotationMatrix(matrix);
		result.inverse();
		return result;
	}

	RotationMatrix::RotationMatrix()
	{
		matrix = new double* [size];
		for (unsigned int i = 0; i < size; i++)
		{
			matrix[i] = new double[size];
			for (unsigned int j = 0; j < size; j++)
				matrix[i][j] = 0;
		}
	}

	RotationMatrix::RotationMatrix(const RotationMatrix& matrix) : RotationMatrix()
	{
		set(matrix);
	}

	RotationMatrix::RotationMatrix(const double** matrix) : RotationMatrix()
	{
		set(matrix);
	}

	RotationMatrix::RotationMatrix(const double(&matrix)[size][size]) : RotationMatrix()
	{
		set(matrix);
	}

	RotationMatrix::~RotationMatrix()
	{
		for (unsigned int i = 0; i < size; i++)
			delete[] matrix[i];
		delete[] matrix;
	}

	void RotationMatrix::set(const RotationMatrix& matrix)
	{
		COPY(matrix.matrix);
	}

	void RotationMatrix::set(const double** matrix)
	{
		COPY(matrix);
	}

	void RotationMatrix::set(const double(&matrix)[size][size])
	{
		COPY(matrix);
	}

	RotationMatrix RotationMatrix::operator+=(const RotationMatrix& matrix)
	{
		*this = *this + matrix;
		return *this;
	}

	RotationMatrix RotationMatrix::operator-=(const RotationMatrix& matrix)
	{
		*this = *this - matrix;
		return *this;
	}

	RotationMatrix RotationMatrix::operator*=(double value)
	{
		*this = *this * value;
		return *this;
	}

	RotationMatrix RotationMatrix::operator*=(const RotationMatrix& matrix)
	{
		*this = *this * matrix;
		return *this;
	}

	RotationMatrix RotationMatrix::operator/=(double value)
	{
		*this = *this / value;
		return *this;
	}

	double& RotationMatrix::operator()(unsigned int i, unsigned int j) const
	{
		if (i >= size || j >= size)
			throw "cell does not exist";
		return matrix[i][j];
	}


	RotationMatrix& RotationMatrix::operator=(const RotationMatrix& matrix)
	{
		set(matrix);
		return *this;
	}

	RotationMatrix& RotationMatrix::operator=(const double(&matrix)[size][size])
	{
		set(matrix);
		return *this;
	}

	RotationMatrix& RotationMatrix::operator=(const double** matrix)
	{
		set(matrix);
		return *this;
	}

	RotationMatrix RotationMatrix::operator-() const
	{
		RotationMatrix result = RotationMatrix();
		for (unsigned int i = 0; i < result.size; i++)
			for (unsigned int j = 0; j < result.size; j++)
				result(i, j) = -matrix[i][j];

		return result;
	}

	void RotationMatrix::transpose()
	{
		RotationMatrix result = RotationMatrix();
		for (unsigned int i = 0; i < result.size; i++)
			for (unsigned int j = 0; j < result.size; j++)
				result.matrix[i][j] = this->matrix[j][i];

		*this = result;
	}

	void RotationMatrix::inverse()
	{
		RotationMatrix result = RotationMatrix();
		for (unsigned int i = 0; i < size; i++)
			result(i, i) = 1;

		double bigMatrix[size][size * 2];
		for (unsigned int i = 0; i < size; i++)
		{
			for (unsigned int j = 0; j < size; j++)
			{
				bigMatrix[i][j] = matrix[i][j];
				bigMatrix[i][j + size] = result(i, j);
			}
		}

		double K;

		for (unsigned int k = 0; k < size; k++) //k-номер строки
		{
			for (unsigned int i = 0; i < 2 * size; i++) //i-номер столбца
				bigMatrix[k][i] = bigMatrix[k][i] / matrix[k][k]; //Деление k-строки на первый член !=0 для преобразования его в единицу

			for (unsigned int i = k + 1; i < size; i++) //i-номер следующей строки после k
			{
				K = bigMatrix[i][k] / bigMatrix[k][k]; //Коэффициент
				for (unsigned int j = 0; j < 2 * size; j++) //j-номер столбца следующей строки после k
					bigMatrix[i][j] = bigMatrix[i][j] - bigMatrix[k][j] * K; //Зануление элементов матрицы ниже первого члена, преобразованного в единицу
			}

			for (unsigned int i = 0; i < size; i++) //Обновление, внесение изменений в начальную матрицу
				for (unsigned int j = 0; j < size; j++)
					matrix[i][j] = bigMatrix[i][j];
		}

		//Обратный ход (Зануление верхнего правого угла)

		for (int k = size - 1; k > -1; k--) //k-номер строки
		{
			for (int i = 2 * size - 1; i > -1; i--) //i-номер столбца
				bigMatrix[k][i] = bigMatrix[k][i] / matrix[k][k];

			for (int i = k - 1; i > -1; i--) //i-номер следующей строки после k
			{
				K = bigMatrix[i][k] / bigMatrix[k][k];
				for (int j = 2 * size - 1; j > -1; j--) //j-номер столбца следующей строки после k
					bigMatrix[i][j] = bigMatrix[i][j] - bigMatrix[k][j] * K;
			}
		}

		//Отделяем от общей матрицы
		for (unsigned int i = 0; i < size; i++)
			for (unsigned int j = 0; j < size; j++)
				result(i, j) = bigMatrix[i][j + size];

		*this = result;
	}

	void RotationMatrix::swapLines(unsigned int first, unsigned int second)
	{
		if (first >= size)
			throw "first must be < size";
		if (second >= size)
			throw "second must be < size";

		double* temp = new double[size];
		for (unsigned int i = 0; i < size; i++)
			temp[i] = matrix[second][i];
		for (unsigned int i = 0; i < size; i++)
			matrix[second][i] = matrix[first][i];
		for (unsigned int i = 0; i < size; i++)
			matrix[first][i] = temp[i];
		delete[] temp;
	}

	void RotationMatrix::swapColumns(unsigned int first, unsigned int second)
	{
		if (first >= size)
			throw "first must be < size";
		if (second >= size)
			throw "second must be < size";

		double* temp = new double[size];
		for (unsigned int i = 0; i < size; i++)
			temp[i] = matrix[i][second];
		for (unsigned int i = 0; i < size; i++)
			matrix[i][second] = matrix[i][first];
		for (unsigned int i = 0; i < size; i++)
			matrix[i][first] = temp[i];
		delete[] temp;
	}

	double RotationMatrix::getDeterminant() const
	{
		int l;
		double result;
		double sum11 = 1, sum12 = 0, sum21 = 1, sum22 = 0;

		for (int i = 0; i < size; i++)
		{
			sum11 = 1; l = 2 * size - 1 - i; sum21 = 1;
			for (int j = 0; j < size; j++)
			{
				sum21 *= matrix[j][l % size];
				l--;
				sum11 *= matrix[j][(j + i) % (size)];
			}
			sum22 += sum21;
			sum12 += sum11;
		}
		result = sum12 - sum22;
		return result;
	}




	std::ostream& operator<< (std::ostream& out, const Calendar& calendar)
	{
		out << calendar.year << "." << calendar.month << "." << calendar.day << " " <<
			calendar.hour << ":" << calendar.minute << ":" << std::fixed << calendar.second << std::endl;
		return out;
	}

	double convertGCtoJD(const Calendar& calendar)
	{
		return convertGCtoJDN(calendar) +
			(calendar.hour - 12.0) / 24.0 + calendar.minute / 1440.0 + calendar.second / 86400.0;
	}

	unsigned int convertGCtoJDN(const Calendar& calendar)
	{
		int a = (14 - calendar.month) / 12;
		int y = calendar.year + 4800 - a;
		int m = calendar.month + 12 * a - 3;
		return calendar.day + (int)((153 * m + 2) / 5) + 365 * y +
			(int)(y / 4) - (int)(y / 100) + (int)(y / 400) - 32045;
	}

	Calendar convertJDtoGC(double julianDate)
	{
		int a = julianDate + 32044;
		int b = (4 * a + 3) / 146097;
		int c = a - 146097 * b / 4;
		int d = (4 * c + 3) / 1461;
		int e = c - 1461 * d / 4;
		int m = (5 * e + 2) / 153;
		Calendar calendar;
		calendar.day = e - (153 * m + 2) / 5 + 1;
		calendar.month = m + 3 - 12 * (m / 10);
		calendar.year = 100 * b + d - 4800 + m / 10;
		double time = julianDate - (int)julianDate;
		time *= 24;
		calendar.hour = (int)(time);
		time = (time - calendar.hour) * 60;
		calendar.minute = (int)(time);
		calendar.second = (time - calendar.minute) * 60;
		return calendar;
	}




	RectangleVector RectangleVector::operator*=(double value)
	{
		return *this = *this * value;
	}

	RectangleVector RectangleVector::operator*=(const RectangleVector& vector)
	{
		return *this = *this * vector;
	}

	RectangleVector RectangleVector::operator/=(double value)
	{
		return *this = *this / value;
	}

	RectangleVector RectangleVector::operator+=(const RectangleVector& vector)
	{
		return *this = *this + vector;
	}

	RectangleVector RectangleVector::operator-=(const RectangleVector& vector)
	{
		return *this = *this - vector;
	}

	RectangleVector RectangleVector::operator-()
	{
		this->x = -this->x;
		this->y = -this->y;
		this->z = -this->z;
		return *this;
	}

	PolarVector PolarVector::operator*=(double value)
	{
		this->length *= value;
		return *this;
	}

	PolarVector PolarVector::operator/=(double value)
	{
		this->length /= value;
		return *this;
	}

	PolarVector PolarVector::operator+=(const PolarVector& vector)
	{
		return *this = *this + vector;
	}

	PolarVector PolarVector::operator-=(const PolarVector& vector)
	{
		return *this = *this - vector;
	}

	PolarVector PolarVector::operator*=(const PolarVector& vector)
	{
		return *this = *this * vector;
	}

	PolarVector PolarVector::operator-()
	{
		this->length = -this->length;
		return *this;
	}

	RectangleVector operator*(const RectangleVector& vector1, const RectangleVector& vector2)
	{
		RectangleVector result;
		result.x = vector1.y * vector2.z - vector1.z * vector2.y;
		result.y = vector1.z * vector2.x - vector1.x * vector2.z;
		result.z = vector1.x * vector2.y - vector1.y * vector2.x;
		return result;
	}

	RectangleVector operator+(const RectangleVector& vector1, const RectangleVector& vector2)
	{
		RectangleVector result;
		result.x = vector1.x + vector2.x;
		result.y = vector1.y + vector2.y;
		result.z = vector1.z + vector2.z;
		return result;
	}

	RectangleVector operator-(const RectangleVector& vector1, const RectangleVector& vector2)
	{
		RectangleVector result;
		result.x = vector1.x - vector2.x;
		result.y = vector1.y - vector2.y;
		result.z = vector1.z - vector2.z;
		return result;
	}


	RectangleVector operator*(const RectangleVector& vector, double value)
	{
		RectangleVector result;
		result.x = vector.x * value;
		result.y = vector.y * value;
		result.z = vector.z * value;
		return result;
	}

	RectangleVector operator*(const RotationMatrix& matrix, const RectangleVector& vector)
	{
		RectangleVector result;
		result.x = matrix(0, 0) * vector.x + matrix(0, 1) * vector.y + matrix(0, 2) * vector.z;
		result.y = matrix(1, 0) * vector.x + matrix(1, 1) * vector.y + matrix(1, 2) * vector.z;
		result.z = matrix(2, 0) * vector.x + matrix(2, 1) * vector.y + matrix(2, 2) * vector.z;
		return result;
	}

	RectangleVector operator/(const RectangleVector& vector, double value)
	{
		RectangleVector result;
		result.x = vector.x / value;
		result.y = vector.y / value;
		result.z = vector.z / value;
		return result;
	}

	PolarVector operator*(const PolarVector& vector1, const PolarVector& vector2)
	{
		return convertRectangleToPolar(
			convertPolarToRectangle(vector1) *
			convertPolarToRectangle(vector2));
	}

	PolarVector operator+(const PolarVector& vector1, const PolarVector& vector2)
	{
		return convertRectangleToPolar(
			convertPolarToRectangle(vector1) +
			convertPolarToRectangle(vector2));
	}

	PolarVector operator-(const PolarVector& vector1, const PolarVector& vector2)
	{
		return convertRectangleToPolar(
			convertPolarToRectangle(vector1) -
			convertPolarToRectangle(vector2));
	}

	PolarVector operator*(const PolarVector& vector, double value)
	{
		PolarVector result = vector;
		result.length *= value;
		return result;
	}

	PolarVector operator/(const PolarVector& vector, double value)
	{
		PolarVector result = vector;
		result.length /= value;
		return result;
	}

	double length(const RectangleVector& vector)
	{
		return sqrt(vector.x * vector.x + vector.y * vector.y + vector.z * vector.z);
	}

	double length(const PolarVector& vector)
	{
		return vector.length;
	}

	std::ostream& operator<< (std::ostream& out, const RectangleVector& vector)
	{
		out << "(";
		out << vector.x << "; ";
		out << vector.y << "; ";
		out << vector.z << ")\n";
		return out;
	}

	std::ostream& operator<< (std::ostream& out, const PolarVector& vector)
	{
		out << "(";
		out << vector.length << "; ";
		out << vector.theta << "; ";
		out << vector.phi << ")\n";
		return out;
	}

	RectangleVector convertPolarToRectangle(const PolarVector& vector)
	{
		RectangleVector result;
		result.x = vector.length * sin(vector.theta) * cos(vector.phi);
		result.y = vector.length * sin(vector.theta) * sin(vector.phi);
		result.z = vector.length * cos(vector.theta);
		return result;
	}

	PolarVector convertRectangleToPolar(const RectangleVector& vector)
	{
		PolarVector result;
		result.length = length(vector);
		result.theta = acos(vector.z / result.length);
		result.phi = atan(vector.y / vector.x);
		return result;
	}




	RectangleVector convertTo(const RectangleVector& vector, const RotationMatrix& matrix, const RectangleVector& vector12)
	{
		return vector12 + matrix * vector;
	}

	RectangleVector convertGeodesicCoordsToVector(double latitude, double longitude)
	{
		latitude *= DD2R;
		longitude *= DD2R;
		RectangleVector result;
		result.x = EARTH_MEAN_RADIUS * cos(latitude) * cos(longitude);
		result.y = EARTH_MEAN_RADIUS * cos(latitude) * sin(longitude);
		result.z = EARTH_MEAN_RADIUS * sin(latitude);
		return result;
	}

	RotationMatrix getPrecessionMatrix(double julianDate)
	{
		double matr[3][3];
		iauPmat06(DJ00, julianDate - DJ00, matr);
		RotationMatrix matrix(matr);
		return matrix;
	}

	RotationMatrix getNutationMatrix(double julianDate)
	{
		double epsa, dpsi, deps;
		double matr[3][3];
		epsa = iauObl06(DJ00, julianDate - DJ00);
		iauNut06a(DJ00, julianDate - DJ00, &dpsi, &deps);
		iauNumat(epsa, dpsi, deps, matr);
		RotationMatrix matrix(matr);
		return matrix;
	}

	RotationMatrix getPolarMotionMatrix(double julianDate)
	{
		double xp = 0; // here should be function xp(julianDate)
		double yp = 0; // here should be function yp(julianDate)
		RotationMatrix matrix = RotationMatrix();
		matrix(0, 0) = cos(xp);
		matrix(0, 1) = 0;
		matrix(0, 2) = sin(xp);
		matrix(1, 0) = sin(xp) * sin(yp);
		matrix(1, 1) = cos(yp);
		matrix(1, 2) = -cos(xp) * sin(yp);
		matrix(2, 0) = -sin(xp) * cos(yp);
		matrix(2, 1) = sin(yp);
		matrix(2, 2) = cos(xp) * cos(yp);
		return matrix;
	}

	RotationMatrix getEarthRotationMatrix(double julianDate)
	{
		double sa = iauGst06a(DJ00, julianDate - DJ00, DJ00, julianDate - DJ00);
		double matr[3][3];
		matr[0][0] = cos(sa);
		matr[0][1] = sin(sa);
		matr[0][2] = 0;
		matr[1][0] = -sin(sa);
		matr[1][1] = cos(sa);
		matr[1][2] = 0;
		matr[2][0] = 0;
		matr[2][1] = 0;
		matr[2][2] = 1;
		RotationMatrix matrix(matr);
		return matrix;
	}

	RotationMatrix ICStoGCSmatrix(double julianDate)
	{
		return getPolarMotionMatrix(julianDate) * getEarthRotationMatrix(julianDate) *
			getNutationMatrix(julianDate) * getPrecessionMatrix(julianDate);
	}

	RotationMatrix GCStoICSmatrix(double julianDate)
	{
		return getInversed(ICStoGCSmatrix(julianDate));
	}

	RotationMatrix GCStoTCSmatrix(double latitude, double longitude)
	{
		return GCStoTCSmatrix(convertGeodesicCoordsToVector(latitude, longitude));
	}

	RectangleVector convertICStoGCS(const RectangleVector& vector, double julianDate)
	{
		RectangleVector vector12;
		return convertTo(vector, ICStoGCSmatrix(julianDate), vector12);
	}

	RectangleVector convertGCStoICS(const RectangleVector& vector, double julianDate)
	{
		RectangleVector vector12;
		return convertTo(vector, GCStoICSmatrix(julianDate), vector12);
	}

	RotationMatrix GCStoTCSmatrix(const RectangleVector& vector)
	{
		double xst = vector.x;
		double yst = vector.y;
		double zst = vector.z;
		double c = 1 / (1 - POLAR_COMPRESSION);
		double Rst = sqrt(xst * xst + yst * yst + c * c * c * c * zst * zst);
		double f = sqrt(xst * xst + yst * yst);

		RectangleVector ez;
		ez.x = xst;
		ez.y = yst;
		ez.z = c * c * zst;
		ez /= Rst;

		RectangleVector ee;
		ee.x = -yst;
		ee.y = xst;
		ee.z = 0.0;
		ee /= f;

		RectangleVector en = ez * ee;

		RotationMatrix result = RotationMatrix();
		result(0, 0) = en.x;
		result(0, 1) = en.y;
		result(0, 2) = en.z;
		result(1, 0) = ee.x;
		result(1, 1) = ee.y;
		result(1, 2) = ee.z;
		result(2, 0) = ez.x;
		result(2, 1) = ez.y;
		result(2, 2) = ez.z;

		return result;
	}

	RotationMatrix TCStoGCSmatrix(double latitude, double longitude)
	{
		return getInversed(GCStoTCSmatrix(latitude, longitude));
	}

	RotationMatrix TCStoGCSmatrix(const RectangleVector& vector)
	{
		return getInversed(GCStoTCSmatrix(vector));
	}

	RectangleVector HICStoICSvector(double julianDate)
	{
		double pvh[2][3];
		double pvb[2][3];
		iauEpv00(DJ00, julianDate - DJ00, pvh, pvb);
		RectangleVector result = { pvh[0][0], pvh[0][1], pvh[0][2] };
		result *= DAU;
		return result;
	}

	RectangleVector ICStoHICSvector(double julianDate)
	{
		return -HICStoICSvector(julianDate);
	}

	RotationMatrix HICStoICSmatrix(double julianDate)
	{
		return getInversed(ICStoHICSmatrix(julianDate));
	}

	RotationMatrix ICStoHICSmatrix(double julianDate)
	{
		double eps0, deps;
		iauNut06a(DJ00, julianDate - DJ00, &eps0, &deps);
		eps0 = 84381.406 * DAS2R + deps; // from iauP06e
		return getRx(eps0);
	}


	RectangleVector convertGCStoTCS(double latitude, double longitude, const RectangleVector& vector)
	{
		return convertTo(vector, GCStoTCSmatrix(latitude, longitude), convertGeodesicCoordsToVector(latitude, longitude));
	}

	RectangleVector convertTCStoGCS(double latitude, double longitude, const RectangleVector& vector)
	{
		return convertTo(vector, TCStoGCSmatrix(latitude, longitude), -convertGeodesicCoordsToVector(latitude, longitude));
	}

	RectangleVector convertGCStoTCS(const RectangleVector& vector, const RectangleVector& vector12)
	{
		return convertTo(vector, GCStoTCSmatrix(vector12), vector12);
	}

	RectangleVector convertTCStoGCS(const RectangleVector& vector, const RectangleVector& vector12)
	{
		return convertTo(vector, TCStoGCSmatrix(vector12), vector12);
	}

	RectangleVector convertICStoHICS(const RectangleVector& vector, double julianDate)
	{
		return convertTo(vector, ICStoHICSmatrix(julianDate), ICStoHICSvector(julianDate));
	}

	RectangleVector convertHICStoICS(const RectangleVector& vector, double julianDate)
	{
		return convertTo(vector, HICStoICSmatrix(julianDate), HICStoICSvector(julianDate));
	}


	RotationMatrix getRx(double angle)
	{
		RotationMatrix result = RotationMatrix();

		result(0, 0) = 1;
		result(0, 1) = 0;
		result(0, 2) = 0;
		result(1, 0) = 0;
		result(1, 1) = cos(angle);
		result(1, 2) = sin(angle);
		result(2, 0) = 0;
		result(2, 1) = -sin(angle);
		result(2, 2) = cos(angle);

		return result;
	}

	RotationMatrix getRy(double angle)
	{
		RotationMatrix result = RotationMatrix();

		result(0, 0) = cos(angle);
		result(0, 1) = 0;
		result(0, 2) = -sin(angle);
		result(1, 0) = 0;
		result(1, 1) = 1;
		result(1, 2) = 0;
		result(2, 0) = sin(angle);
		result(2, 1) = 0;
		result(2, 2) = cos(angle);

		return result;
	}

	RotationMatrix getRz(double angle)
	{
		RotationMatrix result = RotationMatrix();

		result(0, 0) = cos(angle);
		result(0, 1) = sin(angle);
		result(0, 2) = 0;
		result(1, 0) = -sin(angle);
		result(1, 1) = cos(angle);
		result(1, 2) = 0;
		result(2, 0) = 0;
		result(2, 1) = 0;
		result(2, 2) = 1;

		return result;
	}


	RotationMatrix getEulerRotationMatrix(double precession, double nutation, double rotation)
	{
		return getRz(precession) * getRx(nutation) * getRz(rotation);
	}
}