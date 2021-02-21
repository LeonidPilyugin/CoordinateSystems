using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using static System.Math;

namespace CoordinateSystems
{
    public static class Coordinates
    {
        private static class UnmanagedCoordinates
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
        }

        public class Vector
        {
            private double x;
            private double y;
            private double z;

            public Vector()
            {
                x = y = z = 0.0;
            }

            public double X
            {
                get { return x; }
                set { x = value; }
            }
            public double Y
            {
                get { return y; }
                set { y = value; }
            }
            public double Z
            {
                get { return z; }
                set { z = value; }
            }
            public double length
            {
                get { return Sqrt(x * x + y * y + z * z); }
                set
                {
                    if (value <= 0)
                        throw new System.Exception("Value must be > 0");
                    double t = theta;
                    double p = phi;
                    x = value * Sin(t) * Cos(p);
                    y = value * Sin(t) * Sin(p);
                    z = value * Cos(t);
                }
            }
            public double theta
            {
                get { return Acos(z / length); }
                set
                {
                    double l = length;
                    double p = phi;
                    x = l * Sin(value) * Cos(p);
                    y = l * Sin(value) * Sin(p);
                    z = l * Cos(value);
                }
            }
            public double phi
            {
                get { return Atan(y / x); }
                set
                {
                    double l = length;
                    double t = theta;
                    x = l * Sin(t) * Cos(value);
                    y = l * Sin(t) * Sin(value);
                    z = l * Cos(t);
                }
            }

            public void turnX(double angle)
            {
                UnmanagedCoordinates.turnX(ref x, ref y, ref z, angle);
            }
            public void turnY(double angle)
            {
                UnmanagedCoordinates.turnY(ref x, ref y, ref z, angle);
            }
            public void turnZ(double angle)
            {
                UnmanagedCoordinates.turnZ(ref x, ref y, ref z, angle);
            }
            public void turnEuler(double precession, double nutation, double rotation)
            {
                turnZ(precession);
                turnX(nutation);
                turnZ(rotation);
            }

            public static Vector operator +(Vector vector1, Vector vector2)
            {
                Vector result = new Vector();
                result.x = vector1.x + vector2.x;
                result.y = vector1.y + vector2.y;
                result.z = vector1.z + vector2.z;
                return result;
            }
            public static Vector operator -(Vector vector1, Vector vector2)
            {
                Vector result = new Vector();
                result.x = vector1.x - vector2.x;
                result.y = vector1.y - vector2.y;
                result.z = vector1.z - vector2.z;
                return result;
            }
            public static Vector operator *(Vector vector1, Vector vector2)
            {
                Vector result = new Vector();
                result.x = vector1.y * vector2.z - vector1.z * vector2.y;
                result.y = vector1.z * vector2.x - vector1.x * vector2.z;
                result.z = vector1.x * vector2.y - vector1.y * vector2.x;
                return result;
            }
        }

        public class Date
        {
            public struct Calendar
            {
                public int year;
                public int month;
                public int day;
                public int hour;
                public int minute;
                public double second;
            }

            private double julianDate;
            public const double J2000 = 2451545.0;

            public Date()
            {
                julianDate = J2000;
            }
            public Date(Calendar calendar)
            {
                julianDate = getJulianDate(calendar);
            }
            public Date(int year, int month, int day, int hour, int minute, double second)
            {
                Calendar calendar;
                calendar.year = year;
                calendar.month = month;
                calendar.day = day;
                calendar.hour = hour;
                calendar.minute = minute;
                calendar.second = second;
                julianDate = getJulianDate(calendar);
            }
            public Date(double julianDate)
            {
                this.julianDate = julianDate;
            }

            private double getJulianDate(Calendar calendar)
            {
                int a = (14 - calendar.month) / 12;
                int y = calendar.year + 4800 - a;
                int m = calendar.month + 12 * a - 3;
                return calendar.day + (int)((153 * m + 2) / 5) + 365 * y +
                    (int)(y / 4) - (int)(y / 100) + (int)(y / 400) - 32045;
            }
            private Calendar getGrigorianCalendar(double julianDate)
            {
                int a = (int)julianDate + 32044;
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
            private bool isDayCorrect(int value)
            {
                if (value < 1)
                    return false;

                int month = GrigorianCalendar.month;
                if (month % 2 == 1 && month < 8)
                    return value < 32;
                if (month % 2 == 1)
                    return value < 31;

                int year = GrigorianCalendar.year;
                if (month == 2)
                {
                    if (isLeap())
                        return value < 30;
                    else
                        return value < 29;
                }

                if (month % 2 == 0 && month < 8)
                    return value < 31;

                return value < 32;
            }

            public Calendar GrigorianCalendar
            {
                get { return getGrigorianCalendar(julianDate); }
                set
                {
                    if (value.second < 0.0 || value.second >= 60.0)
                        throw new System.Exception("second isn't correct");
                    if (value.minute < 0 || value.minute > 59)
                        throw new System.Exception("minute isn't correct");
                    if (value.hour < 0 || value.hour > 23)
                        throw new System.Exception("hour isn't correct");
                    if (value.day < 0 || value.day > 31)
                        throw new System.Exception("day isn't correct");
                    if (value.month < 0 || value.month > 12)
                        throw new System.Exception("month isn't correct");

                    julianDate = getJulianDate(value);
                }
            }
            public double JulianDate
            { 
                get { return julianDate; }
                set
                {
                    if (value < 0.0)
                        throw new System.Exception("Julian date isn't correct");
                    julianDate = value;
                }
            }
            public double Second
            {
                get { return GrigorianCalendar.second; }
                set
                {
                    if (value < 0.0 || value >= 60.0)
                        throw new System.Exception("second isn't correct");
                    Calendar calendar = GrigorianCalendar;
                    calendar.second = value;
                    GrigorianCalendar = calendar;
                }
            }
            public int Minute
            {
                get { return GrigorianCalendar.minute; }
                set
                {
                    if (value < 0 || value > 59)
                        throw new System.Exception("minute isn't correct");
                    Calendar calendar = GrigorianCalendar;
                    calendar.minute = value;
                    GrigorianCalendar = calendar;
                }
            }
            public int Hour
            {
                get { return GrigorianCalendar.hour; }
                set
                {
                    if (value < 0 || value > 23)
                        throw new System.Exception("hour isn't correct");
                    Calendar calendar = GrigorianCalendar;
                    calendar.hour = value;
                    GrigorianCalendar = calendar;
                }
            }
            public int Day
            {
                get { return GrigorianCalendar.day; }
                set
                {
                    if (!isDayCorrect(value))
                        throw new System.Exception("day isn't correct");
                    Calendar calendar = GrigorianCalendar;
                    calendar.day = value;
                    GrigorianCalendar = calendar;
                }
            }
            public int Month
            {
                get { return GrigorianCalendar.month; }
                set
                {
                    if (value < 1 || value > 12)
                        throw new System.Exception("month isn't correct");
                    Calendar calendar = GrigorianCalendar;
                    calendar.month = value;
                    GrigorianCalendar = calendar;
                }
            }
            public int Year
            {
                get { return GrigorianCalendar.year; }
                set
                {
                    Calendar calendar = GrigorianCalendar;
                    calendar.year = value;
                    GrigorianCalendar = calendar;
                }
            }

            public static Date operator +(Date date1, Date date2)
            {
                Date result = new Date();
                result.julianDate = date1.julianDate + date2.julianDate;
                return result;
            }
            public static Date operator -(Date date1, Date date2)
            {
                Date result = new Date();
                result.julianDate = date1.julianDate - date2.julianDate;
                return result;
            }

            public bool isLeap()
            {
                int year = GrigorianCalendar.year;
                if (year % 400 == 0)
                    return true;
                if (year % 100 == 0)
                    return false;
                return year % 4 == 0;
            }
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
    }
}
