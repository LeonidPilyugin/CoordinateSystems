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
    /// Типы:<br/>
    /// 1) <see cref="LagrangianPoint"/><br/>
    /// <br/>
    /// Свойства:<br/>
    /// 1) <see cref="SunEarthAlpha"/>:<br/>
    /// 2) <see cref="EarthMoonAlpha"/>:<br/>
    /// 3) <see cref="SunEarth1"/>:<br/>
    /// 4) <see cref="SunEarth2"/>:<br/>
    /// 5) <see cref="SunEarth3"/>:<br/>
    /// 6) <see cref="SunEarth4"/>:<br/>
    /// 7) <see cref="SunEarth5"/>:<br/>
    /// 8) <see cref="EarthMoon1"/>:<br/>
    /// 9) <see cref="EarthMoon2"/>:<br/>
    /// 10) <see cref="EarthMoon3"/>:<br/>
    /// 11) <see cref="EarthMoon4"/>:<br/>
    /// 12) <see cref="EarthMoon5"/>:<br/>
    /// <br/>
    /// Функции:<br/>
    /// 1) <see cref="GetEps0(double)"/>:<br/>
    /// 2) <see cref="GetEarthRotationAxis(double)"/>:<br/>
    /// 3) <see cref="GetMoonRotationAxis(double)"/>:<br/>
    /// 4) <see cref="GetSunEarth1(double)"/>:<br/>
    /// 5) <see cref="GetSunEarth2(double)"/>:<br/>
    /// 6) <see cref="GetSunEarth3(double)"/>:<br/>
    /// 7) <see cref="GetSunEarth4(double)"/>:<br/>
    /// 8) <see cref="GetSunEarth5(double)"/>:<br/>
    /// 9) <see cref="GetEarthMoon1(double)"/>:<br/>
    /// 10) <see cref="GetEarthMoon2(double)"/>:<br/>
    /// 11) <see cref="GetEarthMoon3(double)"/>:<br/>
    /// 12) <see cref="GetEarthMoon4(double)"/>:<br/>
    /// 13) <see cref="GetEarthMoon5(double)"/>:<br/>
    /// </remarks>
    public static class LagrangianPoints
    {
        #region types
        /// <summary>
        /// Точки Лагранжа.
        /// </summary>
        public enum LagrangianPoint
        {
            /// <summary> 1 точка Лагранжа системы Солнце-Земля.</summary>
            SE1,
            /// <summary> 2 точка Лагранжа системы Солнце-Земля.</summary>
            SE2,
            /// <summary> 3 точка Лагранжа системы Солнце-Земля.</summary>
            SE3,
            /// <summary> 4 точка Лагранжа системы Солнце-Земля.</summary>
            SE4,
            /// <summary> 5 точка Лагранжа системы Солнце-Земля.</summary>
            SE5,
            /// <summary> 1 точка Лагранжа системы Земля-Луна.</summary>
            EM1,
            /// <summary> 2 точка Лагранжа системы Земля-Луна.</summary>
            EM2,
            /// <summary> 3 точка Лагранжа системы Земля-Луна.</summary>
            EM3,
            /// <summary> 4 точка Лагранжа системы Земля-Луна.</summary>
            EM4,
            /// <summary> 5 точка Лагранжа системы Земля-Луна.</summary>
            EM5
        }
        #endregion

        #region data
        /// <summary>
        /// Время в юлианских днях, через которое определяется вектор
        /// координат для определения мгновенной оси вращения.
        /// </summary>
        private const double TIME_CONST = 1e-6;
        #endregion

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
            return Earth.GetParamsMethod(julianDate).Vector * Earth.GetParamsMethod(julianDate + TIME_CONST).Vector;
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
            return Moon.GetParamsMethod(julianDate).Vector * Moon.GetParamsMethod(julianDate + TIME_CONST).Vector;
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
        public static (Vector Vector, Vector Velocity) SunEarth1
        {
            get
            {
                Vector vector = Earth.Vector * (1 - Math.Pow(SunEarthAlpha / 3.0, 1 / 3.0));
                Vector velocity = Earth.Velocity * (1 - Math.Pow(SunEarthAlpha / 3.0, 1 / 3.0));
                return (vector, velocity);
            }
        }

        /// <summary>
        /// 2 точка Лагранжа для системы Солнце-Земля.
        /// </summary>
        public static (Vector Vector, Vector Velocity) SunEarth2
        {
            get
            {
                Vector vector = Earth.Vector * (1 + Math.Pow(SunEarthAlpha / 3.0, 1 / 3.0));
                Vector velocity = Earth.Velocity * (1 + Math.Pow(SunEarthAlpha / 3.0, 1 / 3.0));
                return (vector, velocity);
            }
        }

        /// <summary>
        /// 3 точка Лагранжа для системы Солнце-Земля.
        /// </summary>
        public static (Vector Vector, Vector Velocity) SunEarth3
        {
            get
            {
                Vector vector = -Earth.Vector * (1 + SunEarthAlpha / 2.4);
                Vector velocity = -Earth.Velocity * (1 + SunEarthAlpha / 2.4);
                return (vector, velocity);
            }
        }

        /// <summary>
        /// 4 точка Лагранжа для системы Солнце-Земля.
        /// </summary>
        public static (Vector Vector, Vector Velocity) SunEarth4
        {
            get
            {
                Vector vector = new Vector(Earth.Vector);
                Vector velocity = new Vector(Earth.Velocity);
                vector.TurnAxis(GetEarthRotationAxis(Planets.JulianDate), Math.PI / 3.0);
                velocity.TurnAxis(GetEarthRotationAxis(Planets.JulianDate), Math.PI / 3.0);
                return (vector, velocity);
            }
        }

        /// <summary>
        /// 5 точка Лагранжа для системы Солнце-Земля.
        /// </summary>
        public static (Vector Vector, Vector Velocity) SunEarth5
        {
            get
            {
                Vector vector = new Vector(Earth.Vector);
                Vector velocity = new Vector(Earth.Velocity);
                vector.TurnAxis(GetEarthRotationAxis(Planets.JulianDate), -Math.PI / 3.0);
                velocity.TurnAxis(GetEarthRotationAxis(Planets.JulianDate), -Math.PI / 3.0);
                return (vector, velocity);
            }
        }

        /// <summary>
        /// 1 точка Лагранжа для системы Солнце-Земля.
        /// </summary>
        /// 
        /// <param name="julianDate"> Юлианская дата.</param>
        public static (Vector Vector, Vector Velocity) GetSunEarth1(double julianDate)
        {
            Vector vec, vel;
            Basis basis;
            (vec, vel, basis) = Earth.GetParamsMethod(julianDate);
            Vector vector = vec * (1 - Math.Pow(SunEarthAlpha / 3.0, 1 / 3.0));
            Vector velocity = vel * (1 - Math.Pow(SunEarthAlpha / 3.0, 1 / 3.0));
            return (vector, velocity);
        }

        /// <summary>
        /// 2 точка Лагранжа для системы Солнце-Земля.
        /// </summary>
        /// 
        /// <param name="julianDate"> Юлианская дата.</param>
        public static (Vector Vector, Vector Velocity) GetSunEarth2(double julianDate)
        {
            Vector vec, vel;
            Basis basis;
            (vec, vel, basis) = Earth.GetParamsMethod(julianDate);
            Vector vector = vec * (1 + Math.Pow(SunEarthAlpha / 3.0, 1 / 3.0));
            Vector velocity = vel * (1 + Math.Pow(SunEarthAlpha / 3.0, 1 / 3.0));
            return (vector, velocity);
        }

        /// <summary>
        /// 3 точка Лагранжа для системы Солнце-Земля.
        /// </summary>
        /// 
        /// <param name="julianDate"> Юлианская дата.</param>
        public static (Vector Vector, Vector Velocity) GetSunEarth3(double julianDate)
        {
            Vector vec, vel;
            Basis basis;
            (vec, vel, basis) = Earth.GetParamsMethod(julianDate);
            Vector vector = -vec * (1 + SunEarthAlpha / 2.4);
            Vector velocity = -vel * (1 + SunEarthAlpha / 2.4);
            return (vector, velocity);
        }

        /// <summary>
        /// 4 точка Лагранжа для системы Солнце-Земля.
        /// </summary>
        /// 
        /// <param name="julianDate"> Юлианская дата.</param>
        public static (Vector Vector, Vector Velocity) GetSunEarth4(double julianDate)
        {
            Vector vector, velocity;
            Basis basis;
            (vector, velocity, basis) = Earth.GetParamsMethod(julianDate);
            vector.TurnAxis(GetEarthRotationAxis(julianDate), Math.PI / 3.0);
            velocity.TurnAxis(GetEarthRotationAxis(julianDate), Math.PI / 3.0);
            return (vector, velocity);
        }

        /// <summary>
        /// 5 точка Лагранжа для системы Солнце-Земля.
        /// </summary>
        /// 
        /// <param name="julianDate"> Юлианская дата.</param>
        public static (Vector Vector, Vector Velocity) GetSunEarth5(double julianDate)
        {
            Vector vector, velocity;
            Basis basis;
            (vector, velocity, basis) = Earth.GetParamsMethod(julianDate);
            vector.TurnAxis(GetEarthRotationAxis(julianDate), -Math.PI / 3.0);
            velocity.TurnAxis(GetEarthRotationAxis(julianDate), -Math.PI / 3.0);
            return (vector, velocity);
        }
        #endregion

        #region Earth-Moon
        /// <summary>
        /// 1 точка Лагранжа для системы Земля-Луна.
        /// </summary>
        public static (Vector Vector, Vector Velocity) EarthMoon1
        {
            get
            {
                Vector vector = Moon.Vector * (1 - Math.Pow(EarthMoonAlpha / 3.0, 1 / 3.0));
                Vector velocity = Moon.Velocity * (1 - Math.Pow(EarthMoonAlpha / 3.0, 1 / 3.0));
                return (vector, velocity);
            }
        }

        /// <summary>
        /// 2 точка Лагранжа для системы Земля-Луна.
        /// </summary>
        public static (Vector Vector, Vector Velocity) EarthMoon2
        {
            get
            {
                Vector vector = Moon.Vector * (1 + Math.Pow(EarthMoonAlpha / 3.0, 1 / 3.0));
                Vector velocity = Moon.Velocity * (1 + Math.Pow(EarthMoonAlpha / 3.0, 1 / 3.0));
                return (vector, velocity);
            }
        }

        /// <summary>
        /// 3 точка Лагранжа для системы Земля-Луна.
        /// </summary>
        /// 
        public static (Vector Vector, Vector Velocity) EarthMoon3
        {
            get
            {
                Vector vector = -Moon.Vector * (1 + EarthMoonAlpha / 2.4);
                Vector velocity = -Moon.Velocity * (1 + EarthMoonAlpha / 2.4);
                return (vector, velocity);
            }
        }

        /// <summary>
        /// 4 точка Лагранжа для системы Земля-Луна.
        /// </summary>
        public static (Vector Vector, Vector Velocity) EarthMoon4
        {
            get
            {
                Vector vector = new Vector(Moon.Vector);
                Vector velocity = new Vector(Moon.Velocity);
                vector.TurnAxis(GetMoonRotationAxis(Planets.JulianDate), Math.PI / 3.0);
                velocity.TurnAxis(GetMoonRotationAxis(Planets.JulianDate), Math.PI / 3.0);
                return (vector, velocity);
            }
        }

        /// <summary>
        /// 5 точка Лагранжа для системы Земля-Луна.
        /// </summary>
        public static (Vector Vector, Vector Velocity) EarthMoon5
        {
            get
            {
                Vector vector = new Vector(Moon.Vector);
                Vector velocity = new Vector(Moon.Velocity);
                vector.TurnAxis(GetMoonRotationAxis(Planets.JulianDate), -Math.PI / 3.0);
                velocity.TurnAxis(GetMoonRotationAxis(Planets.JulianDate), -Math.PI / 3.0);
                return (vector, velocity);
            }
        }
        /// <summary>
        /// 1 точка Лагранжа для системы Земля-Луна.
        /// </summary>
        /// 
        /// <param name="julianDate"> Юлианская дата.</param>
        public static (Vector Vector, Vector Velocity) GetEarthMoon1(double julianDate)
        {
            Vector vec, vel;
            Basis basis;
            (vec, vel, basis) = Moon.GetParamsMethod(julianDate);
            Vector vector = vec * (1 - Math.Pow(EarthMoonAlpha / 3.0, 1 / 3.0));
            Vector velocity = vel * (1 - Math.Pow(EarthMoonAlpha / 3.0, 1 / 3.0));
            return (vector, velocity);
        }

        /// <summary>
        /// 2 точка Лагранжа для системы Земля-Луна.
        /// </summary>
        /// 
        /// <param name="julianDate"> Юлианская дата.</param>
        public static (Vector Vector, Vector Velocity) GetEarthMoon2(double julianDate)
        {
            Vector vec, vel;
            Basis basis;
            (vec, vel, basis) = Moon.GetParamsMethod(julianDate);
            Vector vector = vec * (1 + Math.Pow(EarthMoonAlpha / 3.0, 1 / 3.0));
            Vector velocity = vel * (1 + Math.Pow(EarthMoonAlpha / 3.0, 1 / 3.0));
            return (vector, velocity);
        }

        /// <summary>
        /// 3 точка Лагранжа для системы Земля-Луна.
        /// </summary>
        /// 
        /// <param name="julianDate"> Юлианская дата.</param>
        public static (Vector Vector, Vector Velocity) GetEarthMoon3(double julianDate)
        {
            Vector vec, vel;
            Basis basis;
            (vec, vel, basis) = Moon.GetParamsMethod(julianDate);
            Vector vector = -vec * (1 + EarthMoonAlpha / 2.4);
            Vector velocity = -vel * (1 + EarthMoonAlpha / 2.4);
            return (vector, velocity);
        }

        /// <summary>
        /// 4 точка Лагранжа для системы Земля-Луна.
        /// </summary>
        /// 
        /// <param name="julianDate"> Юлианская дата.</param>
        public static (Vector Vector, Vector Velocity) GetEarthMoon4(double julianDate)
        {
            Vector vector, velocity;
            Basis basis;
            (vector, velocity, basis) = Moon.GetParamsMethod(julianDate);
            vector.TurnAxis(GetMoonRotationAxis(julianDate), Math.PI / 3.0);
            velocity.TurnAxis(GetMoonRotationAxis(julianDate), Math.PI / 3.0);
            return (vector, velocity);
        }

        /// <summary>
        /// 5 точка Лагранжа для системы Земля-Луна.
        /// </summary>
        /// 
        /// <param name="julianDate"> Юлианская дата.</param>
        public static (Vector Vector, Vector Velocity) GetEarthMoon5(double julianDate)
        {
            Vector vector, velocity;
            Basis basis;
            (vector, velocity, basis) = Moon.GetParamsMethod(julianDate);
            vector.TurnAxis(GetMoonRotationAxis(julianDate), -Math.PI / 3.0);
            velocity.TurnAxis(GetMoonRotationAxis(julianDate), -Math.PI / 3.0);
            return (vector, velocity);
        }
        #endregion
    }
}
