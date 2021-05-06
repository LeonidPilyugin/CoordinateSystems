using System;
using CoordinateSystems;
using IAUSOFA;
using SunSystem;
using static System.Math;

// Файл содержит класс NotParabolicKeplerian.

namespace Keplerian
{
    /// <summary>
    /// Абстрактный класс NotParabolicKeplerian описывает
    /// положение и движение тела по не параболической орбите
    /// с использованием Кеплеровых элементов орбиты.<br/>
    /// Наследник класса <see cref="Keplerian"/>.
    /// </summary>
    /// 
    /// <remarks>
    /// Константы:<br/>
    /// 1) <see cref="Keplerian.G"/><br/>
    /// 2) <see cref="ITERATION_CONSTANT"/><br/>
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
    /// 1) <see cref="NotParabolicKeplerian(double, double, double, double, double, double, double, Planet)"/><br/>
    /// 2) <see cref="NotParabolicKeplerian(NotParabolicKeplerian)"/><br/>
    /// <br/>
    /// Свойства:<br/>
    /// 1) <see cref="Vector"/><br/>
    /// 2) <see cref="Keplerian.MeanAngularVelocity"/><br/>
    /// 3) <see cref="Keplerian.Eccentricity"/><br/>
    /// 4) <see cref="Keplerian.TrueAnomaly"/><br/>
    /// 5) <see cref="Keplerian.Velocity"/><br/>
    /// 6) <see cref="Keplerian.JulianDate"/><br/>
    /// 7) <see cref="Keplerian.CentralBody"/><br/>
    /// 8) <see cref="Keplerian.PerifocusDistance"/><br/>
    /// 9) <see cref="Keplerian.Inclination"/><br/>
    /// 10) <see cref="Keplerian.AscendingNodeLongitude"/><br/>
    /// 11) <see cref="Keplerian.PeriapsisArgument"/><br/>
    /// 12) <see cref="SemimajorAxis"/><br/>
    /// <br/>
    /// Методы:<br/>
    /// 1) <see cref="Keplerian.GetTrueAnomaly(double)"/><br/>
    /// 2) <see cref="Keplerian.GetJulianDate(double)"/><br/>
    /// 3) <see cref="GetVector(double)"/><br/>
    /// 4) <see cref="GetVelocity(double)"/><br/>
    /// 5) <see cref="Keplerian.ToString()"/><br/>
    /// 6) <see cref="Keplerian.ToString(double)"/><br/>
    /// 7) <see cref="GetEccentricAnomaly(double)"/><br/>
    /// 8) <see cref="GetMeanAnomaly(double)"/><br/>
    /// 9) <see cref="GetTrueAnomaly2(double)"/><br/>
    /// </remarks>
    public abstract class NotParabolicKeplerian : Keplerian
    {
        #region consts
        /// <summary>
        /// Количество итераций в решении уравнения Кеплера.
        /// </summary>
        protected const int ITERATION_CONSTANT = 100;
        #endregion

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
        /// <seealso cref="SemimajorAxis"/>
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
        public NotParabolicKeplerian(double julianDate, double eccentricity, double semimajorAxis,
            double inclination, double ascendingNodeLongitude, double periapsisArgument,
            double trueAnomaly, Planet centralBody) : base(julianDate, eccentricity,
                semimajorAxis * (1.0 - eccentricity), inclination, ascendingNodeLongitude,
                periapsisArgument, trueAnomaly, centralBody)
        {

        }

        /// <summary>
        /// Конструктор копирования.<br/>
        /// См. также: <see cref="Keplerian(Keplerian)"/>
        /// </summary>
        /// 
        /// <param name="keplerian"> Копируемый NotParabolicKeplerian не должен быть null</param>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Вызывается при передаче null.
        /// </exception>
        public NotParabolicKeplerian(NotParabolicKeplerian keplerian) : base(keplerian)
        {

        }
        #endregion

        #region properties
        /// <summary>
        /// Большая полуось.<br/>
        /// Измеряется в метрах.<br/>
        /// Для гиперболы отрицательна, для эллипса положительна.
        /// </summary>
        /// 
        /// <exception cref="ArgumentOutOfRangeException">
        /// Вызывается при передаче отрицательного значения при передачече
        /// неположительного значения для эллипса и неотрицательного для гиперболы.
        /// </exception>
        public double SemimajorAxis
        {
            get
            {
                return perifocusDistance / (1.0 - eccentricity);
            }

            set
            {
                // Для эллипса (0 <= eccentricity < 1)
                // 1.0 - eccentricity > 0, а value должно быть > 0
                // Для гиперболы (eccentricity > 1)
                // 1.0 - eccentricity < 0, а value должно быть < 0
                // Тогда произведение value * (1.0 - eccentricity)
                // должно быть положительно для обоих случаев.

                if (value * (1.0 - eccentricity) <= 0.0)
                {
                    throw new ArgumentOutOfRangeException("SemimajorAxis must be > 0 for ellipse and < 0 for hyperbola");
                }

                perifocusDistance = value * (1.0 - eccentricity);
            }
        }

        /// <summary>
        /// Координаты в иннерциальной системе координат базового тела в начале движения.<br/>
        /// Координаты измеряются в метрах.<br/>
        /// См. также: <see cref="Keplerian.Vector"/>.
        /// </summary>
        public override Vector Vector
        {
            get
            {
                return GetVector(julianDate);
            }
        }
        #endregion

        #region methods
        /// <summary>
        /// Возвращает значение эксцентрической аномалии, соответствующее переданной истинной аномалии.<br/>
        /// Измеряется в радианах.<br/>
        /// </summary>
        /// 
        /// <param name="trueAnomaly"> Истинная аномалия в радианах.</param>
        /// 
        /// <returns>
        /// Эксцентрическая аномалия, соответствующая переданной истинной аномалии.<br/>
        /// Измеряется в радианах.
        /// </returns>
        protected abstract double GetEccentricAnomaly(double trueAnomaly);

        /// <summary>
        /// Возвращает значение средней аномалии, соответствующее переданной истинной аномалии.<br/>
        /// Измеряется в радианах.<br/>
        /// </summary>
        /// 
        /// <param name="trueAnomaly"> Истинная аномалия в радианах.</param>
        /// 
        /// <returns>
        /// Средняя аномалия, соответствующая переданной истинной аномалии.<br/>
        /// Измеряется в радианах.
        /// </returns>
        protected abstract double GetMeanAnomaly(double trueAnomaly);

        /// <summary>
        /// Возвращает значение истинной аномалии, соответствующее переданной средней аномалии.<br/>
        /// Измеряется в радианах.<br/>
        /// </summary>
        /// 
        /// <param name="meanAnomaly"> Средняя аномалия в радианах.</param>
        /// 
        /// <returns>
        /// Истинная аномалия, соответствующая переданной средней аномалии.<br/>
        /// Измеряется в радианах.
        /// </returns>
        protected abstract double GetTrueAnomaly2(double meanAnomaly);

        /// <inheritdoc/>
        public override Vector GetVector(double julianDate)
        {
            // Получение вектора
            double trueAnomaly = GetTrueAnomaly(julianDate);
            Vector result = new Vector(PI / 2.0, -trueAnomaly);
            result.Length = Abs(SemimajorAxis * (1.0 - eccentricity * eccentricity) /
                (1.0 + eccentricity * Cos(trueAnomaly)));

            // Поворот вектора
            result.TurnZ(-periapsisArgument);
            result.TurnX(-inclination);
            result.TurnZ(ascendingNodeLongitude);

            return result;
        }

        /// <inheritdoc/>
        public override double GetVelocity(double julianDate)
        {
            return Sqrt(G * CentralBody.Mass * (2.0 / GetVector(julianDate).Length - 1.0 / SemimajorAxis));
        }
        #endregion
    }
}
