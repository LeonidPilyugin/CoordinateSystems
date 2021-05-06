using System;
using SunSystem;
using CoordinateSystems;
using IAUSOFA;
using static System.Math;

// Файл содержит класс EllipticKeplerian

namespace Keplerian
{
    using Date;

    /// <summary>
    /// Абстрактный класс EllipticKeplerian описывает
    /// положение и движение тела по эллиптической орбите
    /// с использованием Кеплеровых элементов орбиты.<br/>
    /// Наследник класса <see cref="NotParabolicKeplerian"/>.
    /// </summary>
    /// 
    /// <remarks>
    /// Константы:<br/>
    /// 1) <see cref="Keplerian.G"/><br/>
    /// 2) <see cref="NotParabolicKeplerian.ITERATION_CONSTANT"/><br/>
    /// <br/>
    /// Поля:<br/>
    /// 1) <see cref="Keplerian.julianDate"/><br/>
    /// 2) <see cref="Keplerian.eccentricity"/><br/>
    /// 3) <see cref="Keplerian.perifocusDistance"/><br/>
    /// 4) <see cref="Keplerian.inclination"/><br/>
    /// 5) <see cref="Keplerian.ascendingNodeLongitude"/><br/>
    /// 6) <see cref="Keplerian.periapsisArgument"/><br/>
    /// 7) <see cref="Keplerian.trueAnomaly"/><br/>
    /// 8) <see cref="Keplerian.centralBody"/><br/>
    /// <br/>
    /// Конструкторы:<br/>
    /// 1) <see cref="EllipticKeplerian(double, double, double, double, double, double, double, Planet)"/><br/>
    /// 2) <see cref="EllipticKeplerian(EllipticKeplerian)"/><br/>
    /// <br/>
    /// Свойства:<br/>
    /// 1) <see cref="NotParabolicKeplerian.Vector"/><br/>
    /// 2) <see cref="MeanAngularVelocity"/><br/>
    /// 3) <see cref="Eccentricity"/><br/>
    /// 4) <see cref="TrueAnomaly"/><br/>
    /// 5) <see cref="Keplerian.Velocity"/><br/>
    /// 6) <see cref="Keplerian.JulianDate"/><br/>
    /// 7) <see cref="Keplerian.CentralBody"/><br/>
    /// 8) <see cref="Keplerian.PerifocusDistance"/><br/>
    /// 9) <see cref="Keplerian.Inclination"/><br/>
    /// 10) <see cref="Keplerian.AscendingNodeLongitude"/><br/>
    /// 11) <see cref="Keplerian.PeriapsisArgument"/><br/>
    /// 12) <see cref="NotParabolicKeplerian.SemimajorAxis"/><br/>
    /// 13) <see cref="Keplerian.PeriapsisArgument"/><br/>
    /// 14) <see cref="OrbitalPeriod"/><br/>
    /// <br/>
    /// Методы:<br/>
    /// 1) <see cref="GetTrueAnomaly(double)"/><br/>
    /// 2) <see cref="GetJulianDate(double)"/><br/>
    /// 3) <see cref="NotParabolicKeplerian.GetVector(double)"/><br/>
    /// 4) <see cref="NotParabolicKeplerian.GetVelocity(double)"/><br/>
    /// 5) <see cref="Keplerian.ToString()"/><br/>
    /// 6) <see cref="Keplerian.ToString(double)"/><br/>
    /// 7) <see cref="GetEccentricAnomaly(double)"/><br/>
    /// 8) <see cref="GetMeanAnomaly(double)"/><br/>
    /// 9) <see cref="GetTrueAnomaly2(double)"/><br/>
    /// </remarks>
    public class EllipticKeplerian : NotParabolicKeplerian
    {
        #region constructors
        /// <summary>
        /// Конструктор, параметры которого соответствуют полям,
        /// за исключением большей полуоси, которая пересчитывается в перифокусное расстояние.<br/>
        /// См. также:<br/>
        /// <see cref="Keplerian.julianDate"/><br/>
        /// <see cref="Keplerian.eccentricity"/><br/>
        /// <see cref="Keplerian.perifocusDistance"/><br/>
        /// <see cref="Keplerian.ascendingNodeLongitude"/><br/>
        /// <see cref="Keplerian.periapsisArgument"/><br/>
        /// <see cref="Keplerian.trueAnomaly"/><br/>
        /// <see cref="Keplerian.centralBody"/><br/>
        /// <seealso cref="Keplerian(double, double, double, double, double, double, double, Planet)"/><br/>
        /// <seealso cref="NotParabolicKeplerian(double, double, double, double, double, double, double, Planet)"/><br/>
        /// <seealso cref="NotParabolicKeplerian.SemimajorAxis"/>
        /// </summary>
        /// 
        /// <param name="julianDate"> Юлианская дата</param>
        /// <param name="eccentricity"> Эксцентриситет. Граничные значения зависят от типа орбиты</param>
        /// <param name="semimajorAxis"> Большая полуось. Измеряется в метрах, &gt; 0 для эллипса и &lt; 0 для гиперболы.</param>
        /// <param name="inclination"> Наклонение. Измеряется в радианах, 0 &lt;= inclination &lt;= <see cref="Math.PI"/></param>
        /// <param name="ascendingNodeLongitude"> Долгота восходящего узла. Измеряется в радианах, 0 &lt;= inclination &lt;= 2 * <see cref="Math.PI"/></param>
        /// <param name="periapsisArgument"> Аргумент перицентра. Измеряется в радианах, 0 &lt;= inclination &lt;= 2 * <see cref="Math.PI"/></param>
        /// <param name="trueAnomaly"> Истинная аномалия. Измеряется в радианах, граничные значения зависят от типа орбиты</param>
        /// <param name="centralBody"> Центр гравитации. Не должен быть null</param>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Вызывается при передаче null в centralBody.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Вызывается при передаче некорректных значений.
        /// </exception>
        public EllipticKeplerian(double julianDate, double eccentricity, double semimajorAxis,
            double inclination, double ascendingNodeLongitude, double periapsisArgument,
            double trueAnomaly, Planet centralBody) :
            base(julianDate, eccentricity, semimajorAxis, inclination, ascendingNodeLongitude, periapsisArgument,
            trueAnomaly, centralBody)
        {

        }

        /// <summary>
        /// Конструктор копирования.<br/>
        /// См. также:<br/>
        /// <see cref="Keplerian(Keplerian)"/><br/>
        /// <see cref="NotParabolicKeplerian(NotParabolicKeplerian)"/>
        /// </summary>
        /// 
        /// <param name="keplerian"> Копируемый EllipticKeplerian не должен быть null</param>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Вызывается при передаче null.
        /// </exception>
        public EllipticKeplerian(EllipticKeplerian keplerian) : base(keplerian)
        {

        }

        #endregion

        #region properties
        /// <inheritdoc/>
        protected override double MeanAngularVelocity
        {
            get
            {
                return Sqrt(G * centralBody.Mass / SemimajorAxis / SemimajorAxis / SemimajorAxis);
            }
        }

        /// <summary>
        /// Орбитальный период.<br/>
        /// Измеряется в секундах.
        /// </summary>
        public double OrbitalPeriod
        {
            get
            {
                return 2 * PI / MeanAngularVelocity;
            }
        }

        /// <summary>
        /// Эксцентриситет.<br/>
        /// 0 &lt;= eccentricity &lt; 1
        /// </summary>
        /// 
        /// <exception cref="ArgumentOutOfRangeException">
        /// Вызывается при передачи значения меньшего, чем 0 или большего или равного 1.
        /// </exception>
        public override double Eccentricity
        {
            get
            {
                return eccentricity;
            }

            set
            {
                if (value < 0.0 || value >= 1.0)
                {
                    throw new ArgumentOutOfRangeException("Eccentricity must be >=0 and < 1");
                }

                eccentricity = value;
            }
        }

        /// <summary>
        /// Истинная аномалия в начале движения.<br/>
        /// Измеряется в радианах.<br/>
        /// -<see cref="Math.PI"/> &lt;= trueAnomaly &lt;= <see cref="Math.PI"/>
        /// </summary>
        /// 
        /// <exception cref="ArgumentOutOfRangeException">
        /// Вызывается при записи значения большего, чем <see cref="Math.PI"/>
        /// или меньшего, чем <see cref="Math.PI"/>.
        /// </exception>
        public override double TrueAnomaly
        {
            get
            {
                return trueAnomaly;
            }

            set
            {
                if (value < -PI || value > PI)
                {
                    throw new Exception("TrueAnomaly must be <= PI and >= -PI");
                }

                trueAnomaly = value;
            }
        }
        #endregion

        #region methods
        /// <inheritdoc/>
        protected override double GetEccentricAnomaly(double trueAnomaly)
        {
            // Без этой проверки значение считается некорректно
            if (eccentricity == 0.0)
            {
                return trueAnomaly;
            }

            double eccentricAnomaly = 2.0 * Atan(Tan(trueAnomaly / 2.0) *
                Sqrt((1.0 - eccentricity) / (1.0 + eccentricity)));

            if (Sin(trueAnomaly) < 0)
            {
                eccentricAnomaly = -eccentricAnomaly;
            }

            return eccentricAnomaly;
        }

        /// <inheritdoc/>
        protected override double GetMeanAnomaly(double trueAnomaly)
        {
            double eccentricAnomaly = GetEccentricAnomaly(trueAnomaly);
            return eccentricAnomaly - eccentricity * Sin(eccentricAnomaly);
        }

        /// <inheritdoc/>
        protected override double GetTrueAnomaly2(double meanAnomaly)
        {
            // решение уравнения Кеплера методом итераций
            double eccentricAnomaly = meanAnomaly;
            for (int i = 0; i < ITERATION_CONSTANT; i++)
            {
                eccentricAnomaly = eccentricity * Sin(eccentricAnomaly) + meanAnomaly;
            }

            double trueAnomaly = Acos((Cos(eccentricAnomaly) - eccentricity) /
                (1 - eccentricity * Cos(eccentricAnomaly)));

            if (Sin(eccentricAnomaly) < 0)
            {
                trueAnomaly = -trueAnomaly;
            }

            return trueAnomaly;
        }

        /// <inheritdoc/>
        public override double GetTrueAnomaly(double julianDate)
        {
            double meanAnomaly = GetMeanAnomaly(this.trueAnomaly) +
                MeanAngularVelocity * (julianDate - this.julianDate) * Date.JDtoSecond;
            return GetTrueAnomaly2(meanAnomaly);
        }

        /// <inheritdoc/>
        public override double GetJulianDate(double trueAnomaly)
        {
            if (trueAnomaly < -PI || trueAnomaly > PI)
            {
                throw new Exception("TrueAnomaly must be <= PI and >= -PI");
            }

            double timeInterval = (GetMeanAnomaly(trueAnomaly) -
                GetMeanAnomaly(this.trueAnomaly)) / MeanAngularVelocity;

            if (timeInterval < 0)
            {
                timeInterval += OrbitalPeriod;
            }

            return timeInterval / Date.JDtoSecond + this.julianDate;
        }
        #endregion
    }
}
