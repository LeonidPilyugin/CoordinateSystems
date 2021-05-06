using System;
using CoordinateSystems;
using IAUSOFA;
using Date;
using static SunSystem.Planets;

// Файл содержит класс LagrangianPoints.

namespace SunSystem
{
    using IAUSOFA;
    using Date;

    /// <summary>
    /// Класс LagrangianPoints описывает точки Лагранжа для систем Солнце-Земля и Земля-Луна.
    /// </summary>
    /// 
    /// <remarks>
    /// Свойства:<br/>
    /// 1) <see cref="SunEarthAlpha"/>:<br/>
    /// 2) <see cref="EarthMoonAlpha"/>:<br/>
    /// 3) <see cref="SunEarth1"/>:<br/>
    /// 4) <see cref="SunEarth2"/>:<br/>
    /// 5) <see cref="SunEarth3"/>:<br/>
    /// 6) <see cref="EarthMoon1"/>:<br/>
    /// 7) <see cref="EarthMoon2"/>:<br/>
    /// 8) <see cref="EarthMoon3"/>:<br/>
    /// <br/>
    /// Функции:<br/>
    /// 1) <see cref="GetEps0(double)"/>:<br/>
    /// 2) <see cref="GetEarthRotationAxis(double)"/>:<br/>
    /// 3) <see cref="GetMoonRotationAxis(double)"/>:<br/>
    /// 4) <see cref="GetSunEarth4(double)"/>:<br/>
    /// 5) <see cref="GetSunEarth5(double)"/>:<br/>
    /// 6) <see cref="GetEarthMoon4(double)"/>:<br/>
    /// 7) <see cref="GetEarthMoon5(double)"/>:<br/>
    /// </remarks>
    public static class LagrangianPoints
    {
        #region auxiliary functions
        /// <summary>
        /// Угол наклона эклиптики к экватору.
        /// </summary>
        /// 
        /// <param name="julianDate"> Юлианская дата.</param>
        /// 
        /// <returns>
        /// Угол наклона эклиптики к экватору.
        /// </returns>
        private static double GetEps0(double julianDate)
        {
            double eps0 = 0, deps = 0;
            IAUSOFA.iauNut06a(Date.J2000, julianDate - Date.J2000, ref eps0, ref deps);
            // из iauP06e
            return 84381.406 * IAUSOFA.DAS2R + deps;
        }

        /// <summary>
        /// Ось вращения Земли вокруг Солнца в гелиоцентрической системе координат.
        /// </summary>
        /// 
        /// <param name="julianDate"> Юлианская дата.</param>
        /// 
        /// <returns>
        /// Ось вращения Земли вокруг Солнца в гелиоцентрической системе координат.
        /// </returns>
        private static Vector GetEarthRotationAxis(double julianDate)
        {
            // Вариант через угол неклона к экватору
            //return new Vector(GetEps0(julianDate), -Math.PI / 2.0);

            // Вариант через мгновенную ось вращения
            // (перемножаю два вектора координат, найденных через малый промежуток времени).
            return Earth.GetParamsMethod(julianDate).Vector * Earth.GetParamsMethod(julianDate + 1e-6).Vector;
        }

        /// <summary>
        /// Ось вращения Луны вокруг Земли в геоцентрической системе координат.
        /// </summary>
        /// 
        /// <param name="julianDate"> Юлианская дата.</param>
        /// 
        /// <returns>
        /// Ось вращения Луны вокруг Земли в геоцентрической системе координат.
        /// </returns>
        private static Vector GetMoonRotationAxis(double julianDate)
        {
            // Вариант через угол неклона к экватору
            //return new Vector(GetEps0(julianDate) + 5.15 / 180.0 * Math.PI, -Math.PI / 2.0);

            // Вариант через мгновенную ось вращения
            // (перемножаю два вектора координат, найденных через малый промежуток времени).
            return Moon.GetParamsMethod(julianDate).Vector * Moon.GetParamsMethod(julianDate + 1e-6).Vector;
        }

        /// <summary>
        /// Вспомогательное значение для системы Солнце-Земля.
        /// </summary>
        private static double SunEarthAlpha
        {
            get { return Earth.Mass / (Earth.Mass + Sun.Mass); }
        }

        /// <summary>
        /// Вспомогательное значение для системы Земля-Луна.
        /// </summary>
        private static double EarthMoonAlpha
        {
            get { return Moon.Mass / (Moon.Mass + Earth.Mass); }
        }
        #endregion

        #region Earth-Sun
        /// <summary>
        /// 1 точка Лагранжа для системы Солнце-Земля.
        /// </summary>
        public static Vector SunEarth1
        {
            get { return Earth.Vector * (1 - Math.Pow(SunEarthAlpha / 3.0, 1 / 3.0)); }
        }

        /// <summary>
        /// 2 точка Лагранжа для системы Солнце-Земля.
        /// </summary>
        public static Vector SunEarth2
        {
            get { return Earth.Vector * (1 + Math.Pow(SunEarthAlpha / 3.0, 1 / 3.0)); }
        }

        /// <summary>
        /// 3 точка Лагранжа для системы Солнце-Земля.
        /// </summary>
        public static Vector SunEarth3
        {
            get { return -Earth.Vector * (1 + SunEarthAlpha / 2.4); }
        }

        /// <summary>
        /// 4 точка Лагранжа для системы Солнце-Земля.
        /// </summary>
        /// 
        /// <param name="julianDate"> Юлианская дата.</param>
        public static Vector GetSunEarth4(double julianDate)
        {
            var result = new Vector(Earth.Vector);
            result.TurnAxis(GetEarthRotationAxis(julianDate), Math.PI / 3.0);
            return result;
        }

        /// <summary>
        /// 5 точка Лагранжа для системы Солнце-Земля.
        /// </summary>
        /// 
        /// <param name="julianDate"> Юлианская дата.</param>
        public static Vector GetSunEarth5(double julianDate)
        {
            var result = new Vector(Earth.Vector);
            result.TurnAxis(GetEarthRotationAxis(julianDate), -Math.PI / 3.0);
            return result;
        }
        #endregion

        #region Earth-Moon
        /// <summary>
        /// 1 точка Лагранжа для системы Земля-Луна.
        /// </summary>
        public static Vector EarthMoon1
        {
            get { return Moon.Vector * (1 - Math.Pow(EarthMoonAlpha / 3.0, 1 / 3.0)); }
        }

        /// <summary>
        /// 2 точка Лагранжа для системы Земля-Луна.
        /// </summary>
        public static Vector EarthMoon2
        {
            get { return Moon.Vector * (1 + Math.Pow(EarthMoonAlpha / 3.0, 1 / 3.0)); }
        }

        /// <summary>
        /// 3 точка Лагранжа для системы Земля-Луна.
        /// </summary>
        public static Vector EarthMoon3
        {
            get { return -Moon.Vector * (1 + EarthMoonAlpha / 2.4); }
        }

        /// <summary>
        /// 4 точка Лагранжа для системы Земля-Луна.
        /// </summary>
        /// 
        /// <param name="julianDate"> Юлианская дата.</param>
        public static Vector GetEarthMoon4(double julianDate)
        {
            var result = new Vector(Moon.Vector);
            result.TurnAxis(GetMoonRotationAxis(julianDate), Math.PI / 3.0);
            return result;
        }

        /// <summary>
        /// 5 точка Лагранжа для системы Земля-Луна.
        /// </summary>
        /// 
        /// <param name="julianDate"> Юлианская дата.</param>
        public static Vector GetEarthMoon5(double julianDate)
        {
            var result = new Vector(Moon.Vector);
            result.TurnAxis(GetMoonRotationAxis(julianDate), -Math.PI / 3.0);
            return result;
        }
        #endregion
    }
}
