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
        // определение методов, написанных на С++
        // в коде ими лучше не пользоваться
        public static class UnmanagedCoordinates
        {
            [DllImport("coordsystems.dll", CallingConvention = CallingConvention.Cdecl)]
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
                                                      double x2, double y2, double z2);
        }

        public static Vector ICStoGCS(Vector vector, double julianDate)
        {
            Vector result = new Vector();

            double x = vector.X;
            double y = vector.Y;
            double z = vector.Z;

            UnmanagedCoordinates.ICStoGCS(julianDate, ref x, ref y, ref z);

            result.X = x;
            result.Y = y;
            result.Z = z;

            return result;
        }

        public static Vector GCStoICS(Vector vector, double julianDate)
        {
            Vector result = new Vector();

            double x = vector.X;
            double y = vector.Y;
            double z = vector.Z;

            UnmanagedCoordinates.GCStoICS(julianDate, ref x, ref y, ref z);

            result.X = x;
            result.Y = y;
            result.Z = z;

            return result;
        }

        public static Vector HICStoICS(Vector vector, double julianDate)
        {
            Vector result = new Vector();

            double x = vector.X;
            double y = vector.Y;
            double z = vector.Z;

            UnmanagedCoordinates.HICStoICS(julianDate, ref x, ref y, ref z);

            result.X = x;
            result.Y = y;
            result.Z = z;

            return result;
        }

        public static Vector ICStoHICS(Vector vector, double julianDate)
        {
            Vector result = new Vector();

            double x = vector.X;
            double y = vector.Y;
            double z = vector.Z;

            UnmanagedCoordinates.ICStoHICS(julianDate, ref x, ref y, ref z);

            result.X = x;
            result.Y = y;
            result.Z = z;

            return result;
        }

        public static Vector GCStoTCS(Vector vector, double latitude, double longitude)
        {
            Vector result = new Vector();

            double x = vector.X;
            double y = vector.Y;
            double z = vector.Z;

            UnmanagedCoordinates.GCStoTCS(latitude, longitude, ref x, ref y, ref z);

            result.X = x;
            result.Y = y;
            result.Z = z;

            return result;
        }

        public static Vector TCStoGCS(Vector vector, double latitude, double longitude)
        {
            Vector result = new Vector();

            double x = vector.X;
            double y = vector.Y;
            double z = vector.Z;

            UnmanagedCoordinates.TCStoGCS(latitude, longitude, ref x, ref y, ref z);

            result.X = x;
            result.Y = y;
            result.Z = z;

            return result;
        }

        // vector1 и vector2 заданы в GCS
        public static bool IsCrossingEarth(Vector vector1, Vector vector2)
        {
            return UnmanagedCoordinates.isCrossingEarth(vector1.X, vector1.Y, vector1.Z,
                                                        vector2.X, vector2.Y, vector2.Z);
        }
    }

}
