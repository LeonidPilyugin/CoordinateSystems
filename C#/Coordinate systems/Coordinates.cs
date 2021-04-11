using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using static System.Math;

namespace CoordinateSystems
{
    // набор методов для преобразования координат
    // ICS — иннерциальная система координат (вторая экваториальная)
    // GCS — неиннерциальная система координат (первая экваториальная)
    // TCS — топоцентрическая система координат (горизонтальная)
    // HICS — гелиоцентрическая иннерциальная
    public static class Coordinates
    {
        public const double EARTH_POLAR_COMPRESSION = 1 / 298.3;
        public const double DD2R = 1.745329251994329576923691e-2;
        public const double EARTH_MEAN_RADIUS = 6371000.0;
        public const double EARTH_MAX_RADIUS = 6378245.0;
        public const double EARTH_MIN_RADIUS = EARTH_MAX_RADIUS * (1 - EARTH_POLAR_COMPRESSION);
        public const double DAU = 149597870.7e3;
        public const double DAS2R = 4.848136811095359935899141e-6;
        // определение методов, написанных на С++
        // в коде ими лучше не пользоваться
        public static class UnmanagedCoordinates
        {
            /*[DllImport("coordsystems.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern void ICStoGCS(double julianDate, ref double x, ref double y, ref double z);

            [DllImport("coordsystems.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern void GCStoICS(double julianDate, ref double x, ref double y, ref double z);

            [DllImport("coordsystems.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern void HICStoICS(double julianDate, ref double x, ref double y, ref double z);

            [DllImport("coordsystems.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern void ICStoHICS(double julianDate, ref double x, ref double y, ref double z);

            [DllImport("coordsystems.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern void GCStoTCS(double latitude, double longitude, ref double x, ref double y, ref double z);

            [DllImport("coordsystems.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern void TCStoGCS(double latitude, double longitude, ref double x, ref double y, ref double z);

            [DllImport("coordsystems.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern void turnX(ref double x, ref double y, ref double z, double angle);

            [DllImport("coordsystems.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern void turnY(ref double x, ref double y, ref double z, double angle);

            [DllImport("coordsystems.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern void turnZ(ref double x, ref double y, ref double z, double angle);

            [DllImport("coordsystems.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern bool turnAxis(ref double x, ref double y, ref double z,
                                               double axisX, double axisY, double axisZ, double angle);

            [DllImport("coordsystems.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern bool isCrossingEarth(double x1, double y1, double z1,
                                                      double x2, double y2, double z2);*/

            [DllImport("iausofa.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern void iauPmat06(double date1, double date2, [In, Out] double[,] matrix);

            [DllImport("iausofa.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern double iauGst06a(double uta, double utb, double tta, double ttb);

            [DllImport("iausofa.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern double iauObl06(double date1, double date2);

            [DllImport("iausofa.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern void iauNut06a(double date1, double date2, ref double dpsi, ref double deps);

            [DllImport("iausofa.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern void iauNumat(double epsa, double dpsi, double deps, [In, Out] double[,] matrix);

            [DllImport("iausofa.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern int iauEpv00(double date1, double daye2,
                [In, Out] double[,] pvh, [In, Out] double[,] pvb);

            [DllImport("iausofa.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern int iauPlan94(double date1, double date2, int np, [In, Out] double[,] pv);
        }

        public static Vector ConvertTo(Matrix matrix, Vector vector, Vector point)
        {
            return vector + matrix * point;
        }

        public static Matrix GetPrecessionMatrix(double julianDate)
        {
            double[,] matrix = new double[3, 3];
            UnmanagedCoordinates.iauPmat06(Date.J2000, julianDate - Date.J2000, matrix);
            return new Matrix(matrix);
        }

        public static Matrix GetNutationMatrix(double julianDate)
        {
            double epsa, dpsi = 0, deps = 0;
            double[,] matrix = new double[3, 3];
            epsa = UnmanagedCoordinates.iauObl06(Date.J2000, julianDate - Date.J2000);
            UnmanagedCoordinates.iauNut06a(Date.J2000, julianDate - Date.J2000, ref dpsi, ref deps);
            UnmanagedCoordinates.iauNumat(epsa, dpsi, deps, matrix);
            return new Matrix(matrix);
        }

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

        public static Matrix GetEarthMatrix(double julianDate)
        {
            double sa = UnmanagedCoordinates.iauGst06a(Date.J2000,
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

        public static Matrix GetICStoGCSmatrix(double julianDate)
        {
            return GetPolarMotionMatrix(julianDate) * GetEarthMatrix(julianDate) *
            GetNutationMatrix(julianDate) * GetPrecessionMatrix(julianDate);
        }

        public static Matrix GetGCStoICSmatrix(double julianDate)
        {
            return GetICStoGCSmatrix(julianDate).Inversed;
        }

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

        public static Matrix GetTCStoGCSmatrix(Vector vector)
        {
            return GetGCStoTCSmatrix(vector).Inversed;
        }

        public static Matrix GetICStoHICSmatrix(double julianDate)
        {
            /*double eps0 = 0, deps = 0;
            UnmanagedCoordinates.iauNut06a(Date.J2000, julianDate - Date.J2000, ref eps0, ref deps);
            eps0 = 84381.406 * DAS2R + deps; // from iauP06e
            return Matrix.GetRx(eps0);*/
            return new Matrix();
        }

        public static Matrix GetHICStoICSmatrix(double julianDate)
        {
            return GetICStoHICSmatrix(julianDate).Inversed;
        }

        public static Vector GetGCStoTCSvector(double latitude, double longitude)
        {
            latitude *= DD2R;
            longitude *= DD2R;
            Vector result = new Vector();
            result.X = EARTH_MEAN_RADIUS * Cos(latitude) * Cos(longitude);
            result.Y = EARTH_MEAN_RADIUS * Cos(latitude) * Sin(longitude);
            result.Z = EARTH_MEAN_RADIUS * Sin(latitude);
            return result;
        }

        public static Vector GetTCStoGCSvector(double latitude, double longitude)
        {
            return -GetGCStoTCSvector(latitude, longitude);
        }

        public static Vector GetHICStoICSvector(double julianDate)
        {
            double[,] pvh = new double[2,3];
            double[,] pvb = new double[2, 3];
            UnmanagedCoordinates.iauEpv00(Date.J2000, julianDate - Date.J2000, pvh, pvb);
            Vector result = new Vector(pvh[0, 0], pvh[0, 1], pvh[0, 2]);
            result *= DAU;
            return result;
        }

        public static Vector GetICStoHICSvector(double julianDate)
        {
            return GetHICStoICSmatrix(julianDate) * -GetHICStoICSvector(julianDate);
        }

        public static Vector ConvertICStoGCS(double julianDate, Vector point)
        {
            return GetICStoGCSmatrix(julianDate) * point;
        }

        public static Vector ConvertGCStoICS(double julianDate, Vector point)
        {
            return GetGCStoICSmatrix(julianDate) * point;
        }

        /*public static Vector ConvertGCStoTCS(Vector vector, Vector point)
        {
            return ConvertTo(GetGCStoTCSmatrix(vector), vector, point);
        }

        public static Vector ConvertGCStoTCS(double latitude, double longitude, Vector point)
        {
            Vector vector = GetGCStoTCSvector(latitude, longitude);
            return ConvertTo(GetGCStoTCSmatrix(vector), vector, point);
        }

        public static Vector ConvertTCStoGCS(Vector vector, Vector point)
        {
            return ConvertTo(GetTCStoGCSmatrix(vector), vector, point);
        }

        public static Vector ConvertTCStoGCS(double latitude, double longitude, Vector point)
        {
            Vector vector = GetTCStoGCSvector(latitude, longitude);
            return ConvertTo(GetTCStoGCSmatrix(vector), vector, point);
        }
        */
        public static Vector ConvertICStoHICS(double julianDate, Vector point)
        {
            return GetICStoHICSmatrix(julianDate) * (point - GetICStoHICSvector(julianDate));
        }

        public static Vector ConvertHICStoICS(double julianDate, Vector point)
        {
            return GetHICStoICSmatrix(julianDate) * (point - GetHICStoICSvector(julianDate));
        }
    }

}
