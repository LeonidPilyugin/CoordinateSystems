using System;
using SunSystem;
using CoordinateSystems;
using IAUSOFA;
using static System.Math;

// Файл содержит класс ParabolicKeplerian.

namespace Keplerian
{
    using Date;

    /// <summary>
    /// Класс ParabolicKeplerian описывает
    /// положение и движение тела по параболической орбите
    /// с использованием Кеплеровых элементов орбиты.<br/>
    /// Наследник класса <see cref="Keplerian"/>.
    /// </summary>
    /// 
    /// <remarks>
    /// Константы:<br/>
    /// 1) <see cref="Keplerian.G"/><br/>
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
    /// 1) <see cref="ParabolicKeplerian(double, double, double, double, double, double, Planet)"/><br/>
    /// 2) <see cref="ParabolicKeplerian(ParabolicKeplerian)"/><br/>
    /// <br/>
    /// Свойства:<br/>
    /// 1) <see cref="Vector"/><br/>
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
    /// <br/>
    /// Методы:<br/>
    /// 1) <see cref="GetTrueAnomaly(double)"/><br/>
    /// 2) <see cref="GetJulianDate(double)"/><br/>
    /// 3) <see cref="GetVector(double)"/><br/>
    /// 4) <see cref="GetVelocity(double)"/><br/>
    /// 5) <see cref="Keplerian.ToString()"/><br/>
    /// 6) <see cref="Keplerian.ToString(double)"/><br/>
    /// </remarks>
    public class ParabolicKeplerian : Keplerian
    {
        #region constructors
        /// <summary>
        /// Конструктор, параметры которого соответствуют полям,<br/>
        /// за исключением эксцентриситета, который принимается равным 1.<br/>
        /// См. также:<br/>
        /// <see cref="Keplerian.julianDate"/><br/>
        /// <see cref="Keplerian.eccentricity"/><br/>
        /// <see cref="Keplerian.perifocusDistance"/><br/>
        /// <see cref="Keplerian.ascendingNodeLongitude"/><br/>
        /// <see cref="Keplerian.periapsisArgument"/><br/>
        /// <see cref="Keplerian.trueAnomaly"/><br/>
        /// <see cref="Keplerian.centralBody"/><br/>
        /// </summary>
        /// 
        /// <param name="julianDate"> Юлианская дата</param>
        /// <param name="perifocusDistance"> Перифокусное расстояние. Измеряется в метрах, &gt; 0</param>
        /// <param name="inclination"> Наклонение. Измеряется в радианах, 0 &lt;= inclination &lt;= <see cref="Math.PI"/></param>
        /// <param name="ascendingNodeLongitude"> Долгота восходящего узла. Измеряется в радианах, 0 &lt;= inclination &lt;= 2 * <see cref="Math.PI"/></param>
        /// <param name="periapsisArgument"> Аргумент перицентра. Измеряется в радианах, 0 &lt;= inclination &lt;= 2 * <see cref="Math.PI"/></param>
        /// <param name="trueAnomaly"> Истинная аномалия. Измеряется в радианах, граничные значения зависят от типа орбиты</param>
        /// <param name="centralBody"> Центр гравитации. Не должен быть null</param>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Вызывается при передаче null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Вызывается при передаче некорректных значений.
        /// </exception>
        public ParabolicKeplerian(double julianDate, double perifocusDistance,
            double inclination, double ascendingNodeLongitude, double periapsisArgument,
            double trueAnomaly, Planet centralBody) : base(julianDate, 1.0,
                perifocusDistance, inclination, ascendingNodeLongitude,
                periapsisArgument, trueAnomaly, centralBody)
        {

        }

        /// <summary>
        /// Конструктор копирования.
        /// </summary>
        /// 
        /// <param name="keplerian"> Копируемый ParabolicKeplerian не должен быть null</param>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Вызывается при передаче null.
        /// </exception>
        public ParabolicKeplerian(ParabolicKeplerian keplerian) : base(keplerian)
        {

        }
        #endregion

        #region properties
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

        /// <summary>
        /// Среднее движение.<br/>
        /// Измеряется в радинах в секунду.<br/>
        /// См. также: <see cref="Keplerian.MeanAngularVelocity"/>.
        /// </summary>
        protected override double MeanAngularVelocity
        {
            get
            {
                return Sqrt(centralBody.Mass * G / 2.0 / perifocusDistance / perifocusDistance / perifocusDistance);
            }
        }

        /// <summary>
        /// Эксцентриситет.<br/>
        /// Всегда равен 1, поэтому недоступен для записи.
        /// </summary>
        public override double Eccentricity
        {
            get
            {
                return 1.0;
            }
            set => throw new NotImplementedException();
        }

        /// <summary>
        /// Истинная аномалия в начале движения.<br/>
        /// Измеряется в радианах.<br/>
        /// -<see cref="Math.PI"/> &lt; trueAnomaly &lt; <see cref="Math.PI"/>
        /// </summary>
        /// 
        /// <exception cref="ArgumentOutOfRangeException">
        /// Вызывается при записи значения большего или равного <see cref="Math.PI"/>
        /// или меньшего или равного <see cref="Math.PI"/>.
        /// </exception>
        public override double TrueAnomaly
        {
            get
            {
                return trueAnomaly;
            }

            set
            {
                if (value >= PI || value <= -PI)
                {
                    throw new ArgumentOutOfRangeException("TrueAnomaly must be < PI and > -PI");
                }
            }
        }
        #endregion

        #region methods
        /// <summary>
        /// Возвращает Юлианскую дату, в который истинная аномалия равна данной.
        /// </summary>
        /// 
        /// <param name="trueAnomaly">
        /// Истинная аномалия
        /// </param>
        /// 
        /// <returns>
        /// Юлианская дата, в которую достигается указанная истинная аномалия.
        /// </returns>
        /// 
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Вызывается, если такая истинная аномалия не достигается никогда.
        /// </exception>
        public override double GetJulianDate(double trueAnomaly)
        {
            if (trueAnomaly >= PI || trueAnomaly <= -PI)
            {
                throw new ArgumentOutOfRangeException("TrueAnomaly must be < PI and > -PI");
            }

            // Решение уравнения Баркера
            double tan1 = Tan(trueAnomaly / 2.0);
            double tan0 = Tan(this.trueAnomaly / 2.0);
            return (tan1 - tan0 + tan1 * tan1 * tan1 / 3.0 - tan0 * tan0 * tan0 / 3.0) /
                MeanAngularVelocity / Date.JD_TO_SECOND + julianDate;
        }

        /// <summary>
        /// Возвращает истинную аномалию в данную Юлианскую дату.
        /// </summary>
        /// 
        /// <param name="julianDate">
        /// Юлианская дата
        /// </param>
        /// 
        /// <returns>
        /// Истинная аномалия в указанную Юлианскую дату.<br/>
        /// Измеряется в радианах.
        /// </returns>
        public override double GetTrueAnomaly(double julianDate)
        {
            // Решение уравнения Баркера

            // Юлианская дата на момент прохождения перицентра
            double jd0 = GetJulianDate(0.0);
            // Средняя аномалия
            double m = MeanAngularVelocity * (julianDate - jd0) * Date.JD_TO_SECOND;
            // Вспомогательная величина
            double x = 0.5 * Pow(12.0 * m + 4.0 * Sqrt(9.0 * m * m + 4.0), 1 / 3.0);
            // Результат
            return 2.0 * Atan(x - 1.0 / x);
        }

        /// <inheritdoc/>
        public override Vector GetVector(double julianDate)
        {
            // Получение вектора
            double trueAnomaly = GetTrueAnomaly(julianDate);
            Vector result = new Vector(PI / 2.0, -trueAnomaly);
            result.Length = perifocusDistance * 2.0 / (1.0 + Cos(trueAnomaly));

            // Поворот вектора
            result.TurnEuler(-periapsisArgument, -inclination, -ascendingNodeLongitude);

            return result;
        }

        /// <inheritdoc/>
        public override Vector GetVelocity(double julianDate)
        {
            // Получение вектора
            double trueAnomaly = GetTrueAnomaly(julianDate);
            Vector velocity = new Vector(PI / 2.0, trueAnomaly);
            velocity.Length = Sqrt(2.0 * G * CentralBody.Mass / GetVector(julianDate).Length);
            double angle = Asin((1 + eccentricity * Cos(trueAnomaly)) /
                Sqrt(1 + eccentricity * eccentricity + 2 * eccentricity * Cos(trueAnomaly)));

            // Поворот вектора
            velocity.TurnZ(PI - angle);
            velocity.TurnEuler(-periapsisArgument, -inclination, -ascendingNodeLongitude);

            return velocity;
        }
        #endregion
    }
}
