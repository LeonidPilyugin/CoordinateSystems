using System;
using CoordinateSystems;
using IAUSOFA;
using Date;
using static SunSystem.Planets;

namespace SunSystem
{
    using IAUSOFA;
    using Date;
    public static class LagrangianPoints
    {
        #region auxiliary functions
        private static double GetEps0(double julianDate)
        {
            double eps0 = 0, deps = 0;
            IAUSOFA.iauNut06a(Date.J2000, julianDate - Date.J2000, ref eps0, ref deps);
            // from iauP06e
            return 84381.406 * IAUSOFA.DAS2R + deps;
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
        #endregion

        #region Earth-Sun
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
            var result = new Vector(Earth.Vector);
            result.TurnAxis(GetEarthRotationAxis(julianDate), Math.PI / 3.0);
            return result;
        }

        public static Vector GetSunEarth5(double julianDate)
        {
            var result = new Vector(Earth.Vector);
            result.TurnAxis(GetEarthRotationAxis(julianDate), -Math.PI / 3.0);
            return result;
        }
        #endregion

        #region Earth-Moon
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
            var result = new Vector(Moon.Vector);
            result.TurnAxis(GetMoonRotationAxis(julianDate), Math.PI / 3.0);
            return result;
        }

        public static Vector GetEarthMoon5(double julianDate)
        {
            var result = new Vector(Moon.Vector);
            result.TurnAxis(GetMoonRotationAxis(julianDate), -Math.PI / 3.0);
            return result;
        }
        #endregion
    }
}
