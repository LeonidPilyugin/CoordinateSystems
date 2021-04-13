using System;
using System.Collections.Generic;
using System.Text;
using CoordinateSystems;

namespace SunSystem
{
    public static class LagrangianPoints
    {
        // на месте Earth, Sun и Moon должны стоять объекты этих класов (если они не статические,
        // иначе оставь, как есть)
        // на месте Mass должна стоять масса. Это должно быть поле класса

        private static double GetEps0(double julianDate)
        {
            double eps0 = 0, deps = 0;
            IAUSOFA.iauNut06a(Date.J2000, julianDate - Date.J2000, ref eps0, ref deps);
            // from iauP06e
            return 84381.406 * Coordinates.DAS2R + deps;
        }

        private static Vector GetEarthRotationAxis(double julianDate)
        {
            return new Vector(GetEps0(julianDate), -Math.PI / 2.0);
        }

        private static Vector GetMoonRotationAxis(double julianDate)
        {
            return new Vector(GetEps0(julianDate) + 5.15 / 180.0 * Math.PI, -Math.PI / 2.0);
        }

        private static double SunEarthAlpha
        {
            get { return Earth.Mass / (Earth.Mass + Sun.Mass); }
        }

        private static double EarthMoonAlpha
        {
            get { return Moon.Mass / (Moon.Mass + Earth.Mass); }
        }

        public static Vector SunEarth1
        {
            get { return Earth.Vector * (1 - Math.Pow(SunEarthAlpha / 3.0, 1 / 3.0)); }
        }

        public static Vector SunEarth2
        {
            get { return Earth.Vector * (1 + Math.Pow(SunEarthAlpha / 3.0, 1 / 3.0)); }
        }

        public static Vector SunEarth3
        {
            get { return -Earth.Vector * (1 + SunEarthAlpha / 2.4); }
        }

        public static Vector GetSunEarth4(double julianDate)
        {
            return new Vector(Earth.Vector).TurnAxis(GetEarthRotationAxis(julianDate), Math.PI / 3.0);
        }

        public static Vector GetSunEarth5(double julianDate)
        {
            return new Vector(Earth.Vector).TurnAxis(GetEarthRotationAxis(julianDate), -Math.PI / 3.0);
        }



        public static Vector EarthMoon1
        {
            get { return Moon.Vector * (1 - Math.Pow(EarthMoonAlpha / 3.0, 1 / 3.0)); }
        }

        public static Vector EarthMoon2
        {
            get { return Moon.Vector * (1 + Math.Pow(EarthMoonAlpha / 3.0, 1 / 3.0)); }
        }

        public static Vector EarthMoon3
        {
            get { return -Moon.Vector * (1 + EarthMoonAlpha / 2.4); }
        }

        public static Vector GetEarthMoon4(double julianDate)
        {
            return new Vector(Moon.Vector).TurnAxis(GetMoonRotationAxis(julianDate), Math.PI / 3.0);
        }

        public static Vector GetEarthMoon5(double julianDate)
        {
            return new Vector(Moon.Vector).TurnAxis(GetMoonRotationAxis(julianDate), -Math.PI / 3.0);
        }
    }
}
