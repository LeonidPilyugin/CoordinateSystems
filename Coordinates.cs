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
        }

        public class Date
        {
            public struct Calendar
            {
                public int Year;
                public int Month;
                public int Day;
                public int Hour;
                public int Minute;
                public double Second;
            }

            private double julianDate;
            public const double J2000 = 2451545.0;

            public Date()
            {
                julianDate = J2000;
            }
            public Date(Calendar calendar)
            {
                julianDate = GetJulianDate(calendar);
            }
            public Date(int year, int month, int day, int hour, int minute, double second)
            {
                Calendar calendar;
                calendar.Year = year;
                calendar.Month = month;
                calendar.Day = day;
                calendar.Hour = hour;
                calendar.Minute = minute;
                calendar.Second = second;
                julianDate = GetJulianDate(calendar);
            }
            public Date(double julianDate)
            {
                this.julianDate = julianDate;
            }

            private double GetJulianDate(Calendar calendar)
            {
                int a = (14 - calendar.Month) / 12;
                int y = calendar.Year + 4800 - a;
                int m = calendar.Month + 12 * a - 3;
                return calendar.Day + (int)((153 * m + 2) / 5) + 365 * y +
                    (int)(y / 4) - (int)(y / 100) + (int)(y / 400) - 32045;
            }
            private Calendar GetGrigorianCalendar(double julianDate)
            {
                int a = (int)julianDate + 32044;
                int b = (4 * a + 3) / 146097;
                int c = a - 146097 * b / 4;
                int d = (4 * c + 3) / 1461;
                int e = c - 1461 * d / 4;
                int m = (5 * e + 2) / 153;
                Calendar calendar;
                calendar.Day = e - (153 * m + 2) / 5 + 1;
                calendar.Month = m + 3 - 12 * (m / 10);
                calendar.Year = 100 * b + d - 4800 + m / 10;
                double time = julianDate - (int)julianDate;
                time *= 24;
                calendar.Hour = (int)(time);
                time = (time - calendar.Hour) * 60;
                calendar.Minute = (int)(time);
                calendar.Second = (time - calendar.Minute) * 60;
                return calendar;
            }
            private bool IsDayCorrect(int value)
            {
                if (value < 1)
                    return false;

                int month = GrigorianCalendar.Month;
                if (month % 2 == 1 && month < 8)
                    return value < 32;
                if (month % 2 == 1)
                    return value < 31;

                int year = GrigorianCalendar.Year;
                if (month == 2)
                {
                    if (IsLeap())
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
                get { return GetGrigorianCalendar(julianDate); }
                set
                {
                    if (value.Second < 0.0 || value.Second >= 60.0)
                        throw new System.Exception("second isn't correct");
                    if (value.Minute < 0 || value.Minute > 59)
                        throw new System.Exception("minute isn't correct");
                    if (value.Hour < 0 || value.Hour > 23)
                        throw new System.Exception("hour isn't correct");
                    if (value.Day < 0 || value.Day > 31)
                        throw new System.Exception("day isn't correct");
                    if (value.Month < 0 || value.Month > 12)
                        throw new System.Exception("month isn't correct");

                    julianDate = GetJulianDate(value);
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
                get { return GrigorianCalendar.Second; }
                set
                {
                    if (value < 0.0 || value >= 60.0)
                        throw new System.Exception("second isn't correct");
                    Calendar calendar = GrigorianCalendar;
                    calendar.Second = value;
                    GrigorianCalendar = calendar;
                }
            }
            public int Minute
            {
                get { return GrigorianCalendar.Minute; }
                set
                {
                    if (value < 0 || value > 59)
                        throw new System.Exception("minute isn't correct");
                    Calendar calendar = GrigorianCalendar;
                    calendar.Minute = value;
                    GrigorianCalendar = calendar;
                }
            }
            public int Hour
            {
                get { return GrigorianCalendar.Hour; }
                set
                {
                    if (value < 0 || value > 23)
                        throw new System.Exception("hour isn't correct");
                    Calendar calendar = GrigorianCalendar;
                    calendar.Hour = value;
                    GrigorianCalendar = calendar;
                }
            }
            public int Day
            {
                get { return GrigorianCalendar.Day; }
                set
                {
                    if (!IsDayCorrect(value))
                        throw new System.Exception("day isn't correct");
                    Calendar calendar = GrigorianCalendar;
                    calendar.Day = value;
                    GrigorianCalendar = calendar;
                }
            }
            public int Month
            {
                get { return GrigorianCalendar.Month; }
                set
                {
                    if (value < 1 || value > 12)
                        throw new System.Exception("month isn't correct");
                    Calendar calendar = GrigorianCalendar;
                    calendar.Month = value;
                    GrigorianCalendar = calendar;
                }
            }
            public int Year
            {
                get { return GrigorianCalendar.Year; }
                set
                {
                    Calendar calendar = GrigorianCalendar;
                    calendar.Year = value;
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

            public bool IsLeap()
            {
                int year = GrigorianCalendar.Year;
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
        public double Length
        {
            get { return Sqrt(x * x + y * y + z * z); }
            set
            {
                if (value <= 0)
                    throw new System.Exception("Value must be > 0");
                double t = Theta;
                double p = Phi;
                x = value * Sin(t) * Cos(p);
                y = value * Sin(t) * Sin(p);
                z = value * Cos(t);
            }
        }
        public double Theta
        {
            get { return Acos(z / Length); }
            set
            {
                double l = Length;
                double p = Phi;
                x = l * Sin(value) * Cos(p);
                y = l * Sin(value) * Sin(p);
                z = l * Cos(value);
            }
        }
        public double Phi
        {
            get { return Atan(y / x); }
            set
            {
                double l = Length;
                double t = Theta;
                x = l * Sin(t) * Cos(value);
                y = l * Sin(t) * Sin(value);
                z = l * Cos(t);
            }
        }

        public void TurnX(double angle)
        {
            Coordinates.UnmanagedCoordinates.turnX(ref x, ref y, ref z, angle);
        }
        public void TurnY(double angle)
        {
            Coordinates.UnmanagedCoordinates.turnY(ref x, ref y, ref z, angle);
        }
        public void TurnZ(double angle)
        {
            Coordinates.UnmanagedCoordinates.turnZ(ref x, ref y, ref z, angle);
        }
        public void TurnEuler(double precession, double nutation, double rotation)
        {
            TurnZ(precession);
            TurnX(nutation);
            TurnZ(rotation);
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
}
