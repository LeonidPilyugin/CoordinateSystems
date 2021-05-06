using System;
using CoordinateSystems;
using IAUSOFA;
using SunSystem;
using static System.Math;

// Файл содержит класс Keplerian.

namespace Keplerian
{
    /// <summary>
    /// Абстрактный класс Keplerian описывает положение и движение
    /// малого тела относительно центра гравитации
    /// с использованием Кеплеровых элементов орбиты.
    /// </summary>
    /// 
    /// <remarks>
    /// Константы:<br/>
    /// 1) <see cref="G"/><br/>
    /// <br/>
    /// Поля:<br/>
    /// 1) <see cref="julianDate"/><br/>
    /// 2) <see cref="eccentricity"/><br/>
    /// 3) <see cref="perifocusDistance"/><br/>
    /// 4) <see cref="inclination"/><br/>
    /// 5) <see cref="ascendingNodeLongitude"/><br/>
    /// 6) <see cref="periapsisArgument"/><br/>
    /// 7) <see cref="trueAnomaly"/><br/>
    /// 8) <see cref="centralBody"/><br/>
    /// <br/>
    /// Конструкторы:<br/>
    /// 1) <see cref="Keplerian(double, double, double, double, double, double, double, Planet)"/><br/>
    /// 2) <see cref="Keplerian(Keplerian)"/><br/>
    /// <br/>
    /// Свойства:<br/>
    /// 1) <see cref="Vector"/><br/>
    /// 2) <see cref="MeanAngularVelocity"/><br/>
    /// 3) <see cref="Eccentricity"/><br/>
    /// 4) <see cref="TrueAnomaly"/><br/>
    /// 5) <see cref="Velocity"/><br/>
    /// 6) <see cref="JulianDate"/><br/>
    /// 7) <see cref="CentralBody"/><br/>
    /// 8) <see cref="PerifocusDistance"/><br/>
    /// 9) <see cref="Inclination"/><br/>
    /// 10) <see cref="AscendingNodeLongitude"/><br/>
    /// 11) <see cref="PeriapsisArgument"/><br/>
    /// <br/>
    /// Методы:<br/>
    /// 1) <see cref="GetTrueAnomaly(double)"/><br/>
    /// 2) <see cref="GetJulianDate(double)"/><br/>
    /// 3) <see cref="GetVector(double)"/><br/>
    /// 4) <see cref="GetVelocity(double)"/><br/>
    /// 5) <see cref="ToString()"/><br/>
    /// 6) <see cref="ToString(double)"/><br/>
    /// </remarks>
    public abstract class Keplerian
    {
        #region consts
        /// <summary>
        /// Гравитационная постоянная. Измеряется в единицах СИ
        /// </summary>
        protected const double G = 6.6743e-11;
        #endregion

        #region data
        /// <summary>
        /// Юлианская дата начала движения.
        /// </summary>
        public double julianDate;

        /// <summary>
        /// Эксцентриситет.<br/>
        /// Граничные значения зависят от типа орбиты.
        /// </summary>
        public double eccentricity;

        /// <summary>
        /// Перифокусное расстояние.<br/>
        /// Измеряется в метрах.<br/>
        /// perifocusDistance &gt; 0
        /// </summary>
        public double perifocusDistance;

        /// <summary>
        /// Наклонение.<br/>
        /// Измеряется в радианах.<br/>
        /// 0 &lt;= inclination &lt;= <see cref="Math.PI"/>.
        /// </summary>
        public double inclination;

        /// <summary>
        /// Долгота восходящего узла.<br/>
        /// Измеряется в радианах.<br/>
        /// 0 &lt;= ascendingNodeLongitude &lt;= 2 * <see cref="Math.PI"/>.
        /// </summary>
        public double ascendingNodeLongitude;

        /// <summary>
        /// Аргумент перицентра.<br/>
        /// Измеряется в радианах.<br/>
        /// 0 &lt;= periapsisArgument &lt;= 2 * <see cref="Math.PI"/>.
        /// </summary>
        public double periapsisArgument;

        /// <summary>
        /// Истинная аномалия в момент начала движения.<br/>
        /// Измеряется в радианах.<br/>
        /// граничные значения зависят от типа орбиты.
        /// </summary>
        public double trueAnomaly;

        /// <summary>
        /// Центр гравитации.<br/>
        /// Не должен быть null.
        /// </summary>
        public Planet centralBody;
        #endregion

        #region constructors
        /// <summary>
        /// Конструктор, параметры которого соответствуют полям.<br/>
        /// См. также:<br/>
        /// <see cref="julianDate"/><br/>
        /// <see cref="eccentricity"/><br/>
        /// <see cref="perifocusDistance"/><br/>
        /// <see cref="ascendingNodeLongitude"/><br/>
        /// <see cref="periapsisArgument"/><br/>
        /// <see cref="trueAnomaly"/><br/>
        /// <see cref="centralBody"/><br/>
        /// </summary>
        /// 
        /// <param name="julianDate"> Юлианская дата</param>
        /// <param name="eccentricity"> Эксцентриситет. Граничные значения зависят от типа орбиты</param>
        /// <param name="perifocusDistance"> Перифокусное расстояние. Измеряется в метрах, &gt; 0</param>
        /// <param name="inclination"> Наклонение. Измеряется в радианах, 0 &lt;= inclination &lt;= <see cref="Math.PI"/></param>
        /// <param name="ascendingNodeLongitude"> Долгота восходящего узла. Измеряется в радианах, 0 &lt;= inclination &lt;= 2 * <see cref="Math.PI"/></param>
        /// <param name="periapsisArgument"> Аргумент перицентра. Измеряется в радианах, 0 &lt;= inclination &lt;= 2 * <see cref="Math.PI"/></param>
        /// <param name="trueAnomaly"> Истинная аномалия. Измеряется в радианах, граничные значения зависят от типа орбиты</param>
        /// <param name="centralBody"> Центр гравитации. Не должен быть null</param>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Вызывается при передаче null в <see cref="centralBody"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Вызывается при передаче некорректных значений.
        /// </exception>
        public Keplerian(double julianDate, double eccentricity, double perifocusDistance,
            double inclination, double ascendingNodeLongitude, double periapsisArgument,
            double trueAnomaly, Planet centralBody)
        {
            JulianDate = julianDate;
            Eccentricity = eccentricity;
            PerifocusDistance = perifocusDistance;
            Inclination = inclination;
            AscendingNodeLongitude = ascendingNodeLongitude;
            PeriapsisArgument = periapsisArgument;
            TrueAnomaly = trueAnomaly;
            CentralBody = centralBody;
        }

        /// <summary>
        /// Конструктор копирования.
        /// </summary>
        /// 
        /// <param name="keplerian"> Копируемый Keplerian не должен быть null</param>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Вызывается при передаче null.
        /// </exception>
        public Keplerian(Keplerian keplerian) : this(keplerian.julianDate, keplerian.eccentricity,
            keplerian.perifocusDistance, keplerian.inclination, keplerian.ascendingNodeLongitude,
            keplerian.periapsisArgument, keplerian.trueAnomaly, keplerian.centralBody)
        {

        }
        #endregion

        #region properties
        /// <summary>
        /// Координаты в иннерциальной системе координат базового тела в начале движения.<br/>
        /// Координаты измеряются в метрах.
        /// </summary>
        public abstract Vector Vector { get; }

        /// <summary>
        /// Среднее движение.<br/>
        /// Измеряется в радинах в секунду.
        /// </summary>
        protected abstract double MeanAngularVelocity { get; }

        /// <summary>
        /// Эксцентриситет.<br/>
        /// Граничные значения зависят от типа орбиты.
        /// </summary>
        public abstract double Eccentricity { get; set; }

        /// <summary>
        /// Истинная аномалия в начале движения.<br/>
        /// Измеряется в радианах.<br/>
        /// Граничные значения зависят от типа орбиты.
        /// </summary>
        public abstract double TrueAnomaly { get; set; }

        /// <summary>
        /// Модуль скорости в иннерциальной системе координат базового тела в начале движения.<br/>
        /// Измеряется в метрах в секунду.
        /// </summary>
        public double Velocity
        {
            get
            {
                return GetVelocity(julianDate);
            }
        }

        /// <summary>
        /// Юлианская дата начала движения.
        /// </summary>
        public double JulianDate 
        {
            get 
            {
                return julianDate;
            }

            set
            {
                julianDate = value;
            }
        }

        /// <summary>
        /// Центр гравитации.<br/>
        /// Не должен быть null.
        /// </summary>
        /// 
        /// <exception cref="System.ArgumentNullException">
        /// Вызывается передаче значения null.
        /// </exception>
        public Planet CentralBody
        {
            get
            {
                return centralBody;
            }

            set
            {
                if(value == null)
                {
                    throw new ArgumentNullException("CentralBody mustn't be null");
                }
                centralBody = value;
            }
        }

        /// <summary>
        /// Перифокусное расстоняние.<br/>
        /// Измеряется в метрах.<br/>
        /// perifocusDistance &gt; 0
        /// </summary>
        /// 
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Вызывается передаче неположительного значения.
        /// </exception>
        public double PerifocusDistance 
        {
            get
            {
                return perifocusDistance;
            }

            set
            {
                if(value < 0.0)
                {
                    throw new ArgumentOutOfRangeException("PerifocusDistance must be > 0");
                }

                perifocusDistance = value;
            }
        }

        /// <summary>
        /// Наклонение.<br/>
        /// Измеряется в радианах.<br/>
        /// 0 &lt;= inclination &lt;= <see cref="Math.PI"/>
        /// 
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Вызывается передаче отрицательного занчения или значения, большего, чем <see cref="Math.PI"/>
        /// </exception>
        public double Inclination
        {
            get
            {
                return inclination;
            }

            set
            {
                if(value < 0.0 || value > PI)
                {
                    throw new ArgumentOutOfRangeException("Inclination must be <= PI and >= 0");
                }

                inclination = value;
            }
        }

        /// <summary>
        /// Долгота восходящего узла.<br/>
        /// Измеряется в радианах.<br/>
        /// 0 &lt;= ascendingNodeLongitude &lt;= 2 * <see cref="Math.PI"/>
        /// </summary>
        /// 
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Вызывается передаче отрицательного занчения или значения, большего, чем 2 * <see cref="Math.PI"/>
        /// </exception>
        public double AscendingNodeLongitude
        {
            get
            {
                return ascendingNodeLongitude;
            }

            set
            {
                if (value < 0.0 || value > 2.0 * PI)
                {
                    throw new ArgumentOutOfRangeException("AscendingNodeLongitude must be <= 2PI and >= 0");
                }

                ascendingNodeLongitude = value;
            }
        }

        /// <summary>
        /// Аргумент перицентра.<br/>
        /// Измеряется в радианах.<br/>
        /// 0 &lt;= periapsisArgument &lt;= 2 * <see cref="Math.PI"/>
        /// </summary>
        /// 
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Вызывается передаче отрицательного занчения или значения, большего, чем 2 * <see cref="Math.PI"/>
        /// </exception>
        public double PeriapsisArgument
        {
            get
            {
                return periapsisArgument;
            }

            set
            {
                if (value < 0.0 || value > 2.0 * PI)
                {
                    throw new ArgumentOutOfRangeException("PeriapsisArgument must be <= 2PI and >= 0");
                }

                periapsisArgument = value;
            }
        }
        #endregion

        #region methods
        /// <summary>
        /// Возвращает истинную аномалию в данную юлианскую дату.
        /// </summary>
        /// 
        /// <param name="julianDate"> Юлианская дата.</param>
        /// 
        /// <returns>
        /// Истинная аномалия в указанную Юлианскую дату.<br/>
        /// Измеряется в радианах.
        /// </returns>
        public abstract double GetTrueAnomaly(double julianDate);

        /// <summary>
        /// Возвращает юлианскую дату, в который истинная аномалия равна данной.
        /// </summary>
        /// 
        /// <param name="trueAnomaly"> Истинная аномалия.</param>
        /// 
        /// <returns>
        /// Юлианская дата, в которую достигается указанная истинная аномалия.
        /// </returns>
        /// 
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Вызывается, если такая истинная аномалия не достигается никогда.
        /// </exception>
        public abstract double GetJulianDate(double trueAnomaly);

        /// <summary>
        /// Возвращает координаты в иннерциальной системе отсчета центра гравитации в данный Юлианский день.
        /// </summary>
        /// 
        /// <param name="julianDate"> Юлианская дата.</param>
        /// 
        /// <returns>
        /// Вектор, направленный от центра гравитации к телу в
        /// иннерциальной системе координат центра гравитации.<br/>
        /// Координаты вектора измеряются в метрах.
        /// </returns>
        public abstract Vector GetVector(double julianDate);

        /// <summary>
        /// Возвращает скорость в иннерциальной системе отсчета центра гравитации в данную Юлианскую дату.
        /// </summary>
        /// 
        /// <param name="julianDate"> Юлианская дата.</param>
        /// 
        /// <returns>
        /// Модуль скорости относительно центра гравитации.<br/>
        /// Измеряется в метрах в секунду.
        /// </returns>
        public abstract double GetVelocity(double julianDate);

        /// <summary>
        /// Преобразует в строку.
        /// </summary>
        /// 
        /// <returns>
        /// строка: "[дата] [координаты] [скорость]".
        /// </returns>
        public override string ToString()
        {
            return julianDate.ToString() + " " + Vector.ToString() + " " + Velocity.ToString();
        }

        /// <summary>
        /// Преобразует в строку.
        /// </summary>
        /// 
        /// <param name="julianDate"> Юлианская дата.</param>
        /// 
        /// <returns>
        /// Строка: "[дата] [координаты] [скорость]" с данными, соответствующими указанной Юлианской дате.
        /// </returns>
        public string ToString(double julianDate)
        {
            return julianDate.ToString() + " " + GetVector(julianDate).ToString() +
                " " + GetVelocity(julianDate).ToString();
        }
        #endregion
    }
}
