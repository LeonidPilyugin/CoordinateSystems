using System;
using CoordinateSystems;
using Date;
using IAUSOFA;

// Файл содержит статический класс Planets.

namespace SunSystem
{
    using Date;
    using IAUSOFA;

    /// <summary>
    /// Статический класс Planets описывает основные планеты Солнечной системы, Луну и Солнце.
    /// </summary>
    /// 
    /// <remarks>
    /// Поля:<br/>
    /// 1) <see cref="sun"/><br/>
    /// 2) <see cref="mercury"/><br/>
    /// 3) <see cref="venus"/><br/>
    /// 4) <see cref="earth"/><br/>
    /// 5) <see cref="fixedEarth"/><br/>
    /// 6) <see cref="moon"/><br/>
    /// 7) <see cref="mars"/><br/>
    /// 8) <see cref="jupiter"/><br/>
    /// 9) <see cref="saturn"/><br/>
    /// 10) <see cref="uranus"/><br/>
    /// 11) <see cref="neptune"/><br/>
    /// 12) <see cref="julianDate"/><br/>
    /// <br/>
    /// Свойства:<br/>
    /// 1) <see cref="Sun"/><br/>
    /// 2) <see cref="Mercury"/><br/>
    /// 3) <see cref="Venus"/><br/>
    /// 4) <see cref="Earth"/><br/>
    /// 5) <see cref="FixedEarth"/><br/>
    /// 6) <see cref="Moon"/><br/>
    /// 7) <see cref="Mars"/><br/>
    /// 8) <see cref="Jupiter"/><br/>
    /// 9) <see cref="Saturn"/><br/>
    /// 10) <see cref="Uranus"/><br/>
    /// 11) <see cref="Neptune"/><br/>
    /// 11) <see cref="JulianDate"/><br/>
    /// <br/>
    /// Функции:<br/>
    /// 1) <see cref="SunParams(double)"/><br/>
    /// 2) <see cref="MercuryParams(double)"/><br/>
    /// 3) <see cref="VenusParams(double)"/><br/>
    /// 4) <see cref="EarthParams(double)"/><br/>
    /// 5) <see cref="FixedEarthParams(double)"/><br/>
    /// 6) <see cref="MoonParams(double)"/><br/>
    /// 7) <see cref="MarsParams(double)"/><br/>
    /// 8) <see cref="JupiterParams(double)"/><br/>
    /// 9) <see cref="SaturnParams(double)"/><br/>
    /// 10) <see cref="UranusParams(double)"/><br/>
    /// 11) <see cref="NeptuneParams(double)"/><br/>
    /// 12) <see cref="UpdateParams(double)"/><br/>
    /// </remarks>
    public static class Planets
    {
        #region data
        /// <summary>
        /// Юлианская дата.
        /// </summary>
        private static double julianDate;

        /// <summary>
        /// Солнце.
        /// </summary>
        private static Planet sun;

        /// <summary>
        /// Меркурий.
        /// </summary>
        private static Planet mercury;

        /// <summary>
        /// Венера.
        /// </summary>
        private static Planet venus;

        /// <summary>
        /// Невращающаяся Земля.
        /// </summary>
        private static Planet earth;

        /// <summary>
        /// Вращающаяся Земля.
        /// </summary>
        private static Planet fixedEarth;

        /// <summary>
        /// Луна.
        /// </summary>
        private static Planet moon;

        /// <summary>
        /// Марс.
        /// </summary>
        private static Planet mars;

        /// <summary>
        /// Юпитер.
        /// </summary>
        private static Planet jupiter;

        /// <summary>
        /// Сатурн.
        /// </summary>
        private static Planet saturn;

        /// <summary>
        /// Уран.
        /// </summary>
        private static Planet uranus;

        /// <summary>
        /// Нептун.
        /// </summary>
        private static Planet neptune;
        #endregion

        #region constructor
        static Planets()
        {
            julianDate = Date.J2000;

            sun = new Planet(1.9885e30, 6.9551e8, 6.955e8, "Sun",
                new Vector(0.0, 0.0, 0.0), new Basis(), new Vector(0.0, 0.0, 0.0), null);
            sun.GetParamsMethod += SunParams;

            mercury = new Planet(3.33022e23, 2.4397e6, 2.4397e6, "Mercury",
                new Vector(0.0, 0.0, 0.0), new Basis(), new Vector(0.0, 0.0, 0.0), sun);
            mercury.GetParamsMethod += MercuryParams;

            venus = new Planet(4.8675e24, 6.0518e6, 6.0518e6, "Venus",
                new Vector(0.0, 0.0, 0.0), new Basis(), new Vector(0.0, 0.0, 0.0), sun);
            venus.GetParamsMethod += VenusParams;

            earth = new Planet(5.9722e24, 6.3781e6, 6.3568e6, "Earth",
                new Vector(0.0, 0.0, 0.0), new Basis(), new Vector(0.0, 0.0, 0.0), sun);
            earth.GetParamsMethod += EarthParams;

            fixedEarth = new Planet(0.0, 6.3781e6, 6.3568e6, "Fixed Earth",
                new Vector(0.0, 0.0, 0.0), new Basis(), new Vector(0.0, 0.0, 0.0), earth);
            fixedEarth.GetParamsMethod += FixedEarthParams;

            moon = new Planet(7.342e22, 1.73814e6, 1.7371e6, "Moon",
                new Vector(0.0, 0.0, 0.0), new Basis(), new Vector(0.0, 0.0, 0.0), earth);
            moon.GetParamsMethod += MoonParams;

            mars = new Planet(6.4171e23, 3.3962e6, 3.3762e6, "Mars",
                new Vector(0.0, 0.0, 0.0), new Basis(), new Vector(0.0, 0.0, 0.0), sun);
            mars.GetParamsMethod += MarsParams;

            jupiter = new Planet(1.8986e27, 71.492e6, 66.854e6, "Jupiter",
                new Vector(0.0, 0.0, 0.0), new Basis(), new Vector(0.0, 0.0, 0.0), sun);
            jupiter.GetParamsMethod += JupiterParams;

            saturn = new Planet(5.6846e26, 60.268e6, 54.364e6, "Saturn",
                new Vector(0.0, 0.0, 0.0), new Basis(), new Vector(0.0, 0.0, 0.0), sun);
            saturn.GetParamsMethod += SaturnParams;

            uranus = new Planet(8.6813e25, 25.559e6, 24.973e6, "Uranus",
                new Vector(0.0, 0.0, 0.0), new Basis(), new Vector(0.0, 0.0, 0.0), sun);
            uranus.GetParamsMethod += UranusParams;

            neptune = new Planet(1.0243e26, 24.764e6, 24.341e6, "Neptune",
                new Vector(0.0, 0.0, 0.0), new Basis(), new Vector(0.0, 0.0, 0.0), sun);
            neptune.GetParamsMethod += NeptuneParams;
        }
        #endregion

        #region params functions
        /// <summary>
        /// Положение и скорость Солнца.
        /// </summary>
        /// 
        /// <param name="julianDate"> Юлианская дата.</param>
        /// 
        /// <returns>
        /// Положение и скорость Солнца.
        /// </returns>
        private static (Vector Vector, Vector Velocity, Basis Basis) SunParams(double julianDate)
        {
            return (new Vector(0.0, 0.0, 0.0), new Vector(0.0, 0.0, 0.0), new Basis());
        }

        /// <summary>
        /// Положение и скорость Мкркурия.
        /// </summary>
        /// 
        /// <param name="julianDate"> Юлианская дата.</param>
        /// 
        /// <returns>
        /// Положение и скорость Меркурия.
        /// </returns>
        private static (Vector Vector, Vector Velocity, Basis Basis) MercuryParams(double julianDate)
        {
            var pv = new double[2, 3];
            IAUSOFA.iauPlan94(Date.J2000, julianDate - Date.J2000, 1, pv);
            var vector = new Vector(pv[0, 0], pv[0, 1], pv[0, 2]);
            var velocity = new Vector(pv[1, 0], pv[1, 1], pv[1, 2]);
            return (vector * IAUSOFA.DAU, velocity * IAUSOFA.DAU / Date.JD_TO_SECOND, new Basis());
        }

        /// <summary>
        /// Положение и скорость Венеры.
        /// </summary>
        /// 
        /// <param name="julianDate"> Юлианская дата.</param>
        /// 
        /// <returns>
        /// Положение и скорость Венеры.
        /// </returns>
        private static (Vector Vector, Vector Velocity, Basis Basis) VenusParams(double julianDate)
        {
            var pv = new double[2, 3];
            IAUSOFA.iauPlan94(Date.J2000, julianDate - Date.J2000, 2, pv);
            var vector = new Vector(pv[0, 0], pv[0, 1], pv[0, 2]);
            var velocity = new Vector(pv[1, 0], pv[1, 1], pv[1, 2]);
            return (vector * IAUSOFA.DAU, velocity * IAUSOFA.DAU / Date.JD_TO_SECOND, new Basis());
        }

        /// <summary>
        /// Положение и скорость невращающейся Земли.
        /// </summary>
        /// 
        /// <param name="julianDate"> Юлианская дата.</param>
        /// 
        /// <returns>
        /// Положение и скорость невращающейся Земли.
        /// </returns>
        private static (Vector Vector, Vector Velocity, Basis Basis) EarthParams(double julianDate)
        {
            var pvh = new double[2, 3];
            var pvb = new double[2, 3];
            IAUSOFA.iauEpv00(Date.J2000, julianDate - Date.J2000, pvh, pvb);
            var vector = new Vector(pvh[0, 0], pvh[0, 1], pvh[0, 2]);
            var velocity = new Vector(pvh[1, 0], pvh[1, 1], pvh[1, 2]);
            return (vector * IAUSOFA.DAU, velocity * IAUSOFA.DAU / Date.JD_TO_SECOND, new Basis());
        }

        /// <summary>
        /// Положение и скорость вращающейся Земли.
        /// </summary>
        /// 
        /// <param name="julianDate"> Юлианская дата.</param>
        /// 
        /// <returns>
        /// Положение и скорость вращающейся Земли.
        /// </returns>
        private static (Vector Vector, Vector Velocity, Basis Basis) FixedEarthParams(double julianDate)
        {
            var basis = new Basis();
            var matrix = Coordinates.GetICStoGCSmatrix(julianDate);
            basis.I = matrix * basis.I;
            basis.J = matrix * basis.J;
            basis.K = matrix * basis.K;
            return (new Vector(0.0, 0.0, 0.0), new Vector(0.0, 0.0, 0.0), basis);
        }

        /// <summary>
        /// Положение и скорость Луны.
        /// </summary>
        /// 
        /// <param name="julianDate"> Юлианская дата.</param>
        /// 
        /// <returns>
        /// Положение и скорость Луны.
        /// </returns>
        private static (Vector Vector, Vector Velocity, Basis Basis) MoonParams(double julianDate)
        {
            (Vector EarthVector, Vector EarthVelocity, Basis basis) = EarthParams(julianDate);
            var pv = new double[2, 3];
            IAUSOFA.iauPlan94(Date.J2000, julianDate - Date.J2000, 3, pv);
            Vector EMBVector = new Vector(pv[0, 0], pv[0, 1], pv[0, 2]) * IAUSOFA.DAU;
            Vector EMBVelocity = new Vector(pv[1, 0], pv[1, 1], pv[1, 2]) * IAUSOFA.DAU / Date.JD_TO_SECOND;

            return (new Vector((EMBVector * (earth.Mass + moon.Mass) - EarthVector * earth.Mass) / moon.Mass - EarthVector),
                new Vector((EMBVelocity * (earth.Mass + moon.Mass) - EarthVelocity * earth.Mass) / moon.Mass - EarthVelocity),
                basis);
        }

        /// <summary>
        /// Положение и скорость Марса.
        /// </summary>
        /// 
        /// <param name="julianDate"> Юлианская дата.</param>
        /// 
        /// <returns>
        /// Положение и скорость Марса.
        /// </returns>
        private static (Vector Vector, Vector Velocity, Basis Basis) MarsParams(double julianDate)
        {
            var pv = new double[2, 3];
            IAUSOFA.iauPlan94(Date.J2000, julianDate - Date.J2000, 4, pv);
            var vector = new Vector(pv[0, 0], pv[0, 1], pv[0, 2]);
            var velocity = new Vector(pv[1, 0], pv[1, 1], pv[1, 2]);
            return (vector * IAUSOFA.DAU, velocity * IAUSOFA.DAU / Date.JD_TO_SECOND, new Basis());
        }

        /// <summary>
        /// Положение и скорость Юпитера.
        /// </summary>
        /// 
        /// <param name="julianDate"> Юлианская дата.</param>
        /// 
        /// <returns>
        /// Положение и скорость Юпитера.
        /// </returns>
        private static (Vector Vector, Vector Velocity, Basis Basis) JupiterParams(double julianDate)
        {
            var pv = new double[2, 3];
            IAUSOFA.iauPlan94(Date.J2000, julianDate - Date.J2000, 5, pv);
            var vector = new Vector(pv[0, 0], pv[0, 1], pv[0, 2]);
            var velocity = new Vector(pv[1, 0], pv[1, 1], pv[1, 2]);
            return (vector * IAUSOFA.DAU, velocity * IAUSOFA.DAU / Date.JD_TO_SECOND, new Basis());
        }

        /// <summary>
        /// Положение и скорость Сатурна.
        /// </summary>
        /// 
        /// <param name="julianDate"> Юлианская дата.</param>
        /// 
        /// <returns>
        /// Положение и скорость Сатурна.
        /// </returns>
        private static (Vector Vector, Vector Velocity, Basis Basis) SaturnParams(double julianDate)
        {
            var pv = new double[2, 3];
            IAUSOFA.iauPlan94(Date.J2000, julianDate - Date.J2000, 6, pv);
            var vector = new Vector(pv[0, 0], pv[0, 1], pv[0, 2]);
            var velocity = new Vector(pv[1, 0], pv[1, 1], pv[1, 2]);
            return (vector * IAUSOFA.DAU, velocity * IAUSOFA.DAU / Date.JD_TO_SECOND, new Basis());
        }

        /// <summary>
        /// Положение и скорость Урана.
        /// </summary>
        /// 
        /// <param name="julianDate"> Юлианская дата.</param>
        /// 
        /// <returns>
        /// Положение и скорость Урана.
        /// </returns>
        private static (Vector Vector, Vector Velocity, Basis Basis) UranusParams(double julianDate)
        {
            var pv = new double[2, 3];
            IAUSOFA.iauPlan94(Date.J2000, julianDate - Date.J2000, 7, pv);
            var vector = new Vector(pv[0, 0], pv[0, 1], pv[0, 2]);
            var velocity = new Vector(pv[1, 0], pv[1, 1], pv[1, 2]);
            return (vector * IAUSOFA.DAU, velocity * IAUSOFA.DAU / Date.JD_TO_SECOND, new Basis());
        }

        /// <summary>
        /// Положение и скорость Нептуна.
        /// </summary>
        /// 
        /// <param name="julianDate"> Юлианская дата.</param>
        /// 
        /// <returns>
        /// Положение и скорость Нептуна.
        /// </returns>
        private static (Vector Vector, Vector Velocity, Basis Basis) NeptuneParams(double julianDate)
        {
            var pv = new double[2, 3];
            IAUSOFA.iauPlan94(Date.J2000, julianDate - Date.J2000, 8, pv);
            var vector = new Vector(pv[0, 0], pv[0, 1], pv[0, 2]);
            var velocity = new Vector(pv[1, 0], pv[1, 1], pv[1, 2]);
            return (vector * IAUSOFA.DAU, velocity * IAUSOFA.DAU / Date.JD_TO_SECOND, new Basis());
        }
        #endregion

        #region properties
        /// <summary>
        /// Юлианская дата.
        /// </summary>
        public static double JulianDate
        {
            get
            {
                return julianDate;
            }
        }

        /// <summary>
        /// Солнце.
        /// </summary>
        public static Planet Sun
        {
            get
            {
                return sun;
            }
        }

        /// <summary>
        /// Меркурий.
        /// </summary>
        public static Planet Mercury
        {
            get
            {
                return mercury;
            }
        }

        /// <summary>
        /// Венера.
        /// </summary>
        public static Planet Venus
        {
            get
            {
                return venus;
            }
        }

        /// <summary>
        /// Невращающаяся Земля.
        /// </summary>
        public static Planet Earth
        {
            get
            {
                return earth;
            }
        }

        /// <summary>
        /// Вращающаяся Земля.
        /// </summary>
        public static Planet FixedEarth
        {
            get
            {
                return fixedEarth;
            }
        }

        /// <summary>
        /// Луна.
        /// </summary>
        public static Planet Moon
        {
            get
            {
                return moon;
            }
        }

        /// <summary>
        /// Марс.
        /// </summary>
        public static Planet Mars
        {
            get
            {
                return mars;
            }
        }

        /// <summary>
        /// Юпитер.
        /// </summary>
        public static Planet Jupiter
        {
            get
            {
                return jupiter;
            }
        }

        /// <summary>
        /// Сатурн.
        /// </summary>
        public static Planet Saturn
        {
            get
            {
                return saturn;
            }
        }

        /// <summary>
        /// Уран.
        /// </summary>
        public static Planet Uranus
        {
            get
            {
                return uranus;
            }
        }

        /// <summary>
        /// Нептун.
        /// </summary>
        public static Planet Neptune
        {
            get
            {
                return neptune;
            }
        }
        #endregion

        #region functions
        /// <summary>
        /// Обновляет положение и скорость всех тел, указанных в полях.
        /// </summary>
        /// 
        /// <param name="julianDate"> Юлианская дата.</param>
        public static void UpdateParams(double julianDate)
        {
            sun.UpdateParams(julianDate);
            mercury.UpdateParams(julianDate);
            venus.UpdateParams(julianDate);
            earth.UpdateParams(julianDate);
            fixedEarth.UpdateParams(julianDate);
            moon.UpdateParams(julianDate);
            mars.UpdateParams(julianDate);
            jupiter.UpdateParams(julianDate);
            saturn.UpdateParams(julianDate);
            uranus.UpdateParams(julianDate);
            neptune.UpdateParams(julianDate);
            Planets.julianDate = julianDate;
        }
        #endregion
    }
}
