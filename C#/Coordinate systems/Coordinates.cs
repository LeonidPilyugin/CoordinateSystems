using System;
using static System.Math;
using static IAUSOFA.IAUSOFA;

// Фвйл содержит статический класс Coordinates.

namespace CoordinateSystems
{
    using Date;

    /// <summary>
    /// Класс содержит методы и константы для перехода между системами координат.
    /// </summary>
    /// 
    /// <remarks>
    /// Сокращения систем координат:<br/>
    /// ICS — иннерциальная система координат (вторая экваториальная)<br/>
    /// GCS — неиннерциальная система координат (первая экваториальная)<br/>
    /// TCS — топоцентрическая система координат (горизонтальная)<br/>
    /// HICS — гелиоцентрическая иннерциальная (вторая экваториальная с центром в Солнце)<br/>
    /// 
    /// Константы:<br/>
    /// 1) <see cref="EARTH_POLAR_COMPRESSION"/><br/>
    /// 2) <see cref="EARTH_MEAN_RADIUS"/><br/>
    /// 3) <see cref="EARTH_MAX_RADIUS"/><br/>
    /// 4) <see cref="EARTH_MIN_RADIUS"/><br/>
    /// <br/>
    /// Методы:<br/>
    /// 1) <see cref="GetPrecessionMatrix(double)"/><br/>
    /// 2) <see cref="GetNutationMatrix(double)"/><br/>
    /// 3) <see cref="GetPolarMotionMatrix(double)"/><br/>
    /// 4) <see cref="GetEarthRotationMatrix(double)"/><br/>
    /// 5) <see cref="GetICStoGCSmatrix(double)"/><br/>
    /// 6) <see cref="GetGCStoICSmatrix(double)"/><br/>
    /// 7) <see cref="GetGCStoTCSmatrix(Vector)"/><br/>
    /// 8) <see cref="GetTCStoGCSmatrix(Vector)"/><br/>
    /// 9) <see cref="GetICStoHICSmatrix(double)"/><br/>
    /// 10) <see cref="GetHICStoICSmatrix(double)"/><br/>
    /// 11) <see cref="GetHICStoICSvector(double)"/><br/>
    /// 12) <see cref="GetICStoHICSvector(double)"/><br/>
    /// 13) <see cref="GetGCStoTCSvector(double, double)"/><br/>
    /// 14) <see cref="GetTCStoGCSvector(double, double)"/><br/>
    /// 15) <see cref="ConvertTo(Matrix, Vector, Vector)"/><br/>
    /// 16) <see cref="ConvertICStoGCS(double, Vector)"/><br/>
    /// 17) <see cref="ConvertGCStoICS(double, Vector)"/><br/>
    /// 18) <see cref="ConvertICStoHICS(double, Vector)"/><br/>
    /// 19) <see cref="ConvertHICStoICS(double, Vector)"/><br/>
    /// </remarks>
    public static class Coordinates
    {
        #region consts
        /// <summary>
        /// Полярное сжатие Земли.
        /// </summary>
        public const double EARTH_POLAR_COMPRESSION = 1 / 298.3;

        /// <summary>
        /// Средний радиус Земли.
        /// </summary>
        public const double EARTH_MEAN_RADIUS = 6371000.0;

        /// <summary>
        /// Экваториальный радиус Земли.
        /// </summary>
        public const double EARTH_MAX_RADIUS = 6378245.0;

        /// <summary>
        /// Полярный радиус Земли.
        /// </summary>
        public const double EARTH_MIN_RADIUS = EARTH_MAX_RADIUS * (1 - EARTH_POLAR_COMPRESSION);
        #endregion

        #region matrices
        /// <summary>
        /// Матрица земной прецессии в указанную юлианскую дату.
        /// </summary>
        /// 
        /// <param name="julianDate"> Юлианская дата.</param>
        /// 
        /// <returns>
        /// Матрица земной прецессии в указанную юлианскую дату.
        /// </returns>
        public static Matrix GetPrecessionMatrix(double julianDate)
        {
            double[,] matrix = new double[3, 3];
            iauPmat06(Date.J2000, julianDate - Date.J2000, matrix);
            return new Matrix(matrix);
        }

        /// <summary>
        /// Матрица земной нутации в указанную юлианскую дату.
        /// </summary>
        /// 
        /// <param name="julianDate"> Юлианская дата.</param>
        /// 
        /// <returns>
        /// Матрица земной нутации в указанную юлианскую дату.
        /// </returns>
        public static Matrix GetNutationMatrix(double julianDate)
        {
            double epsa, dpsi = 0, deps = 0;
            double[,] matrix = new double[3, 3];
            epsa = iauObl06(Date.J2000, julianDate - Date.J2000);
            iauNut06a(Date.J2000, julianDate - Date.J2000, ref dpsi, ref deps);
            iauNumat(epsa, dpsi, deps, matrix);
            return new Matrix(matrix);
        }

        /// <summary>
        /// Матрица полярного движения Земли в указанную юлианскую дату.<br/>
        /// Не найдены функции, описывающие это движение, но с высокой точностью можно считать,
        /// что этого движения не существует.
        /// /// </summary>
        /// 
        /// <param name="julianDate"> Юлианская дата.</param>
        /// 
        /// <returns>
        /// Матрица полярного движения Земли в указанную юлианскую дату.
        /// </returns>
        public static Matrix GetPolarMotionMatrix(double julianDate)
        {
            double xp = 0; // here should be function xp(julianDate)
            double yp = 0; // here should be function yp(julianDate)
            double sinxp = Sin(xp);
            double cosxp = Cos(xp);
            double sinyp = Sin(yp);
            double cosyp = Cos(yp);

            Matrix result = new Matrix();
            result[0, 0] = cosxp;
            result[0, 1] = 0;
            result[0, 2] = sinxp;
            result[1, 0] = sinxp * sinyp;
            result[1, 1] = cosyp;
            result[1, 2] = -cosxp * sinyp;
            result[2, 0] = -sinxp * cosyp;
            result[2, 1] = sinyp;
            result[2, 2] = cosxp * cosyp;
            return result;
        }

        /// <summary>
        /// Матрица земного вращения в указанную юлианскую дату.
        /// </summary>
        /// 
        /// <param name="julianDate"> Юлианская дата.</param>
        /// 
        /// <returns>
        /// Матрица земного вращения в указанную юлианскую дату.
        /// </returns>
        public static Matrix GetEarthRotationMatrix(double julianDate)
        {
            double sa = iauGst06a(Date.J2000,
                julianDate - Date.J2000, Date.J2000, julianDate - Date.J2000);

            double[,] matrix = new double[3, 3];
            matrix[0, 0] = Cos(sa);
            matrix[0, 1] = Sin(sa);
            matrix[0, 2] = 0;
            matrix[1, 0] = -Sin(sa);
            matrix[1, 1] = Cos(sa);
            matrix[1, 2] = 0;
            matrix[2, 0] = 0;
            matrix[2, 1] = 0;
            matrix[2, 2] = 1;

            return new Matrix(matrix);
        }

        /// <summary>
        /// Матрица перехода из второй экваториальной системы координат в первую.
        /// </summary>
        /// 
        /// <param name="julianDate"> Юлианская дата.</param>
        /// 
        /// <returns>
        /// Матрица перехода из второй экваториальной системы координат в первую.
        /// </returns>
        public static Matrix GetICStoGCSmatrix(double julianDate)
        {
            return GetPolarMotionMatrix(julianDate) * GetEarthRotationMatrix(julianDate) *
            GetNutationMatrix(julianDate) * GetPrecessionMatrix(julianDate);
        }

        /// <summary>
        /// Матрица перехода из первой экваториальной системы координат во вторую.
        /// </summary>
        /// 
        /// <param name="julianDate"> Юлианская дата.</param>
        /// 
        /// <returns>
        /// Матрица перехода из второй экваториальной системы координат в первую.
        /// </returns>
        public static Matrix GetGCStoICSmatrix(double julianDate)
        {
            return GetICStoGCSmatrix(julianDate).Inversed;
        }

        /// <summary>
        /// Матрица перехода из первой экваториальной системы координат в топоцентрическую.
        /// </summary>
        /// 
        /// <param name="vector"> Вектор в первой экваториальной системе координат, направленный на точку на поверхности.</param>
        /// 
        /// <returns>
        /// Матрица перехода из первой экваториальной системы координат в топоцентрическую.
        /// </returns>
        public static Matrix GetGCStoTCSmatrix(Vector vector)
        {
            double xst = vector.X;
            double yst = vector.Y;
            double zst = vector.Z;
            double c = 1 / (1 - EARTH_POLAR_COMPRESSION);
            double Rst = Sqrt(xst * xst + yst * yst + c * c * c * c * zst * zst);
            double f = Sqrt(xst * xst + yst * yst);

            Vector ez = new Vector();
            ez.X = xst;
            ez.Y = yst;
            ez.Z = c * c * zst;
            ez /= Rst;

            Vector ee = new Vector();
            ee.X = -yst;
            ee.Y = xst;
            ee.Z = 0.0;
            ee /= f;

            Vector en = ez * ee;

            Matrix result = new Matrix();
            result[0, 0] = en.X;
            result[0, 1] = en.Y;
            result[0, 2] = en.Z;
            result[1, 0] = ee.X;
            result[1, 1] = ee.Y;
            result[1, 2] = ee.Z;
            result[2, 0] = ez.X;
            result[2, 1] = ez.Y;
            result[2, 2] = ez.Z;

            return result;
        }

        /// <summary>
        /// Матрица перехода из топоцентрической системы координат в первую экваториальную.
        /// </summary>
        /// 
        /// <param name="vector"> Вектор в первой экваториальной системе координат, направленный на точку на поверхности.</param>
        /// 
        /// <returns>
        /// Матрица перехода из топоцентрической системы координат в первую экваториальную.
        /// </returns>
        public static Matrix GetTCStoGCSmatrix(Vector vector)
        {
            return GetGCStoTCSmatrix(vector).Inversed;
        }

        /// <summary>
        /// Матрица перехода из второй экваториальной системы координат в гелиоцентрическую (вторая экваториальная с центром в Солнце).
        /// </summary>
        /// 
        /// <param name="julianDate"> Юлианская дата.</param>
        /// 
        /// <returns>
        /// Матрица перехода из второй экваториальной системы координат в гелиоцентрическую (вторая экваториальная с центром в Солнце).
        /// </returns>
        public static Matrix GetICStoHICSmatrix(double julianDate)
        {
            /*double eps0 = 0, deps = 0;
            UnmanagedCoordinates.iauNut06a(Date.J2000, julianDate - Date.J2000, ref eps0, ref deps);
            eps0 = 84381.406 * DAS2R + deps; // from iauP06e
            return Matrix.GetRx(eps0);*/
            return new Matrix();
        }

        /// <summary>
        /// Матрица перехода из гелиоцентрической (вторая экваториальная с центром в Солнце) системы координат во вторую экваториальную.
        /// </summary>
        /// 
        /// <param name="julianDate"> Юлианская дата.</param>
        /// 
        /// <returns>
        /// Матрица перехода из гелиоцентрической (вторая экваториальная с центром в Солнце) системы координат во вторую экваториальную.
        /// </returns>
        public static Matrix GetHICStoICSmatrix(double julianDate)
        {
            return GetICStoHICSmatrix(julianDate).Inversed;
        }
        #endregion

        #region vectors
        /// <summary>
        /// Вектор в первой экваториальной системы координат, направленный в пункт с указанными широтой и долготой.
        /// </summary>
        /// 
        /// <param name="latitude"> Широта. Измеряется в радианах. -<see cref="Math.PI"/> / 2 &lt;= latitude &lt;= <see cref="Math.PI"/> / 2.</param>
        /// <param name="longitude">  Долгота. Измеряется в радианах. -<see cref="Math.PI"/> &lt;= latitude &lt;= <see cref="Math.PI"/>.</param>
        /// 
        /// <returns>
        /// Вектор в первой экваториальной системы координат, направленный в пункт с указанными широтой и долготой.
        /// </returns>
        /// 
        /// <exception cref="ArgumentException">
        /// Вызывается, если передается широта, по модулю большая <see cref="Math.PI"/> / 2 или
        /// долгота, по модулю большая <see cref="Math.PI"/>.
        /// </exception>
        public static Vector GetGCStoTCSvector(double latitude, double longitude)
        {
            if(latitude < -PI / 2.0 || latitude > PI / 2.0)
            {
                throw new ArgumentException("Latitude must be <= PI / 2 and >= -PI / 2");
            }
            if (longitude < -PI || longitude > PI)
            {
                throw new ArgumentException("Longitude must be <= PI and >= -PI");
            }

            latitude *= DD2R;
            longitude *= DD2R;
            Vector result = new Vector();
            result.X = EARTH_MEAN_RADIUS * Cos(latitude) * Cos(longitude);
            result.Y = EARTH_MEAN_RADIUS * Cos(latitude) * Sin(longitude);
            result.Z = EARTH_MEAN_RADIUS * Sin(latitude);
            return result;
        }

        /// <summary>
        /// Вектор в первой экваториальной системы координат, направленный от пункта с указанными широтой и долготой в начало координат.
        /// </summary>
        /// 
        /// <param name="latitude"> Широта. Измеряется в радианах. -<see cref="Math.PI"/> / 2 &lt;= latitude &lt;= <see cref="Math.PI"/> / 2.</param>
        /// <param name="longitude">  Долгота. Измеряется в радианах. -<see cref="Math.PI"/> &lt;= latitude &lt;= <see cref="Math.PI"/>.</param>
        /// 
        /// <returns>
        /// Вектор в первой экваториальной системы координат, направленный от пункта с указанными широтой и долготой в начало координат.
        /// </returns>
        /// 
        /// <exception cref="ArgumentException">
        /// Вызывается, если передается широта, по модулю большая <see cref="Math.PI"/> / 2 или
        /// долгота, по модулю большая <see cref="Math.PI"/>.
        /// </exception>
        public static Vector GetTCStoGCSvector(double latitude, double longitude)
        {
            return -GetGCStoTCSvector(latitude, longitude);
        }

        /// <summary>
        /// Вектор в гелиоцентрической (второй экваториальной с центром в Солнце) системе координат, направленный на Землю.
        /// </summary>
        /// 
        /// <param name="julianDate"> Юлианская дата.</param>
        /// 
        /// <returns>
        /// Вектор в гелиоцентрической (второй экваториальной с центром в Солнце) системе координат, направленный на Землю.
        /// </returns>
        public static Vector GetHICStoICSvector(double julianDate)
        {
            double[,] pvh = new double[2,3];
            double[,] pvb = new double[2, 3];
            iauEpv00(Date.J2000, julianDate - Date.J2000, pvh, pvb);
            Vector result = new Vector(pvh[0, 0], pvh[0, 1], pvh[0, 2]);
            result *= DAU;
            return result;
        }

        /// <summary>
        /// Вектор в гелиоцентрической (второй экваториальной с центром в Солнце) системе координат, направленный от Земли в начало координат.
        /// </summary>
        /// 
        /// <param name="julianDate"> Юлианская дата.</param>
        /// 
        /// <returns>
        /// Вектор в гелиоцентрической (второй экваториальной с центром в Солнце) системе координат, направленный от Земли в начало координат.
        /// </returns>
        public static Vector GetICStoHICSvector(double julianDate)
        {
            return GetHICStoICSmatrix(julianDate) * -GetHICStoICSvector(julianDate);
        }
        #endregion

        #region converting functions
        /// <summary>
        /// Переводит вектор в новую систему координат.
        /// </summary>
        /// 
        /// <param name="matrix"> Матрица перехода.</param>
        /// <param name="vector"> Векториз старой системы координат в новую.</param>
        /// <param name="point"> Переводимый вектор.</param>
        /// 
        /// <returns>
        /// Вектор в новой системе координат.
        /// </returns>
        public static Vector ConvertTo(Matrix matrix, Vector vector, Vector point)
        {
            return vector + matrix * point;
        }

        /// <summary>
        /// Переводит вектор из первой экваториальной системы координат во вторую.
        /// </summary>
        /// 
        /// <param name="julianDate"> Юлианская дата.</param>
        /// <param name="point"> Переводимый вектор.</param>
        /// 
        /// <returns>
        /// Переведенный вектор.
        /// </returns>
        public static Vector ConvertICStoGCS(double julianDate, Vector point)
        {
            return GetICStoGCSmatrix(julianDate) * point;
        }

        /// <summary>
        /// Переводит вектор из второй экваториальной системы координат в первую.
        /// </summary>
        /// 
        /// <param name="julianDate"> Юлианская дата.</param>
        /// <param name="point"> Переводимый вектор.</param>
        /// 
        /// <returns>
        /// Переведенный вектор.
        /// </returns>
        public static Vector ConvertGCStoICS(double julianDate, Vector point)
        {
            return GetGCStoICSmatrix(julianDate) * point;
        }

        /// <summary>
        /// Переводит вектор из второй экваториальной системы координат в гелиоцентрическую (вторую экваториальную с центром в Солнце).
        /// </summary>
        /// 
        /// <param name="julianDate"> Юлианская дата.</param>
        /// <param name="point"> Переводимый вектор.</param>
        /// 
        /// <returns>
        /// Переведенный вектор.
        /// </returns>
        public static Vector ConvertICStoHICS(double julianDate, Vector point)
        {
            return GetICStoHICSmatrix(julianDate) * (point - GetICStoHICSvector(julianDate));
        }

        /// <summary>
        /// Переводит вектор из гелиоцентрической (второй экваториальной с центром в Солнце) системы координат во вторую экваториальную.
        /// </summary>
        /// 
        /// <param name="julianDate"> Юлианская дата.</param>
        /// <param name="point"> Переводимый вектор.</param>
        /// 
        /// <returns>
        /// Переведенный вектор.
        /// </returns>
        public static Vector ConvertHICStoICS(double julianDate, Vector point)
        {
            return GetHICStoICSmatrix(julianDate) * (point - GetHICStoICSvector(julianDate));
        }
        #endregion
    }

}
