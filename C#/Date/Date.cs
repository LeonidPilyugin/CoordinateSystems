using System;

// Файл содержит класс Date

namespace Date
{
    /// <summary>
    /// Класс Date представляет дату и время и позволяет переводить их в Юлианскую дату и обратно.
    /// </summary>
    /// 
    /// <remarks>
    /// Типы:<br/>
    /// 1) структура <see cref="Calendar"/><br/>
    /// <br/>
    /// Константы:<br/>
    /// 1) <see cref="J2000"/><br/>
    /// 2) <see cref="JDtoSecond"/><br/>
    /// <br/>
    /// Поля:<br/>
    /// 1) <see cref="julianDate"/><br/>
    /// <br/>
    /// Конструкторы:<br/>
    /// 1) <see cref="Date()"/><br/>
    /// 2) <see cref="Date(Calendar)"/><br/>
    /// 3) <see cref="Date(int, int, int, int, int, double)"/><br/>
    /// 4) <see cref="Date(double)"/><br/>
    /// 5) <see cref="Date(Date)"/><br/>
    /// <br/>
    /// Свойства:<br/>
    /// 1) <see cref="GrigorianCalendar"/><br/>
    /// 2) <see cref="JulianDate"/><br/>
    /// 3) <see cref="Second"/><br/>
    /// 4) <see cref="Minute"/><br/>
    /// 5) <see cref="Hour"/><br/>
    /// 6) <see cref="Day"/><br/>
    /// 7) <see cref="Month"/><br/>
    /// 8) <see cref="Year"/><br/>
    /// <br/>
    /// Методы:<br/>
    /// 1) <see cref="GetJulianDate(Calendar)"/><br/>
    /// 2) <see cref="GetGrigorianCalendar(double)"/><br/>
    /// 3) <see cref="IsDateCorrect(int, int, int)"/><br/>
    /// 4) <see cref="IsYearLeap(int)"/><br/>
    /// 5) <see cref="IsCalendarCorrect(Date.Calendar)"/><br/>
    /// <br/>
    /// Операторы:<br/>
    /// 1) <see cref="operator +(Date, Date)"/><br/>
    /// 2) <see cref="operator -(Date, Date)"/><br/>
    /// 3) <see cref="operator +(Date, double)"/><br/>
    /// 4) <see cref="operator +(double, Date)"/><br/>
    /// 5) <see cref="operator -(Date, double)"/><br/>
    /// 6) <see cref="operator -(double, Date)"/><br/>
    /// 7) <see cref="operator ==(Date, Date)"/><br/>
    /// 8) <see cref="operator ==(double, Date)"/><br/>
    /// 9) <see cref="operator ==(Date, double)"/><br/>
    /// 10) <see cref="operator !=(Date, Date)"/><br/>
    /// 11) <see cref="operator !=(double, Date)"/><br/>
    /// 12) <see cref="operator !=(Date, double)"/><br/>
    /// 13) <see cref="operator >(Date, Date)"/><br/>
    /// 14) <see cref="operator >(double, Date)"/><br/>
    /// 15) <see cref="operator >(Date, double)"/><br/>
    /// 16) <see cref="operator &lt;(Date, Date)"/><br/>
    /// 17) <see cref="operator &lt;(double, Date)"/><br/>
    /// 18) <see cref="operator &lt;(Date, double)"/><br/>
    /// 19) <see cref="operator >=(Date, Date)"/><br/>
    /// 20) <see cref="operator >=(double, Date)"/><br/>
    /// 21) <see cref="operator >=(Date, double)"/><br/>
    /// 22) <see cref="operator &lt;=(Date, Date)"/><br/>
    /// 23) <see cref="operator &lt;=(double, Date)"/><br/>
    /// 24) <see cref="operator &lt;=(Date, double)"/><br/>
    /// </remarks>
    public class Date
    {
        /// <summary>
        /// Структура представляет календарь.
        /// </summary>
        /// 
        /// <remarks>
        /// Поля:<br/>
        /// 1) <see cref="Calendar.Year"/><br/>
        /// 1) <see cref="Calendar.Month"/><br/>
        /// 1) <see cref="Calendar.Day"/><br/>
        /// 1) <see cref="Calendar.Hour"/><br/>
        /// 1) <see cref="Calendar.Minute"/><br/>
        /// 1) <see cref="Calendar.Second"/><br/>
        /// </remarks>
        public struct Calendar
        {
            #region data
            /// <summary>
            /// Год
            /// </summary>
            public int Year;

            /// <summary>
            /// Месяц
            /// </summary>
            public int Month;

            /// <summary>
            /// День
            /// </summary>
            public int Day;

            /// <summary>
            /// Час
            /// </summary>
            public int Hour;

            /// <summary>
            /// Минута
            /// </summary>
            public int Minute;

            /// <summary>
            /// Секунда
            /// </summary>
            public double Second;
            #endregion
        }

        #region consts
        /// <summary>
        /// Юлианская дата эпохи J2000.0
        /// </summary>
        public const double J2000 = 2451545.0;

        /// <summary>
        /// Число секунд в дне
        /// </summary>
        public const double JDtoSecond = 86400.0;
        #endregion

        #region data
        /// <summary>
        /// Юлианская дата
        /// </summary>
        private double julianDate;
        #endregion

        #region constructors
        /// <summary>
        /// Конструктор по умолчанию. Задает значение <see cref="julianDate"/> = <see cref="J2000"/>.
        /// </summary>
        public Date()
        {
            julianDate = J2000;
        }

        /// <summary>
        /// Конструктор задает значение julianDate, соответствующее переданной дате Григорианского календаря.
        /// Дата должна быть позднее 23 ноября -4713 года.
        /// </summary>
        /// 
        /// <param name="calendar"> Григорианский календарь. Дата должна быть позднее 23 ноября -4713 года.</param>
        /// 
        /// <exception cref="ArgumentException">
        /// Вызывается при передаче некорректной даты.
        /// </exception>
        public Date(Calendar calendar)
        {
            julianDate = GetJulianDate(calendar);
        }

        /// <summary>
        /// Конструктор задает значение julianDate, соответствующее переданной дате Григорианского календаря.
        /// Дата должна быть позднее 23 ноября -4713 года.
        /// </summary>
        /// 
        /// <param name="year"> год</param>
        /// <param name="month"> месяц</param>
        /// <param name="day"> день</param>
        /// <param name="hour"> час</param>
        /// <param name="minute"> минута</param>
        /// <param name="second"> секунда</param>
        /// 
        /// <exception cref="ArgumentException">
        /// Вызывается при передаче некорректной даты.
        /// </exception>
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

        /// <summary>
        /// Конструктор задает значение julianDate, аргументу.
        /// Переданная Юлианская дата должна быть положительна.
        /// </summary>
        /// 
        /// <param name="julianDate"> Юлианская дата. должна быть положительна.</param>
        /// 
        /// <exception cref="ArgumentException">
        /// Вызывается при передаче отрицательной юлианской даты.
        /// </exception>
        public Date(double julianDate)
        {
            if(julianDate <= 0.0)
            {
                throw new ArgumentException("julianDate isn't correct");
            }
            this.julianDate = julianDate;
        }

        /// <summary>
        /// Конструктор копирования. Копируемая дата не должна быть null.
        /// </summary>
        /// 
        /// <param name="date"> Копируемая дата не должна быть null.</param>
        public Date(Date date)
        {
            this.julianDate = date.julianDate;
        }
        #endregion

        #region properties
        /// <summary>
        /// Дата Григорианского календаря. Должна быть позднее 23 Ноября -4713 года
        /// </summary>
        /// 
        /// <exception cref="ArgumentOutOfRangeException">
        /// Вызывается при несоответствии полей переданной структуры возможным.
        /// </exception>
        public Calendar GrigorianCalendar
        {
            get 
            {
                return GetGrigorianCalendar(julianDate);
            }

            set
            {
                if (value.Second < 0.0 || value.Second >= 60.0)
                {
                    throw new System.ArgumentOutOfRangeException("Second isn't correct");
                }

                if (value.Minute < 0 || value.Minute > 59)
                {
                    throw new System.ArgumentOutOfRangeException("Minute isn't correct");
                }

                if (value.Hour < 0 || value.Hour > 23)
                {
                    throw new System.ArgumentOutOfRangeException("Hour isn't correct");
                }

                if (!IsDateCorrect(value.Year, value.Month, value.Day))
                {
                    throw new System.ArgumentOutOfRangeException("Date isn't correct");
                }

                julianDate = GetJulianDate(value);
            }
        }

        /// <summary>
        /// Юлианская дата. Должна быть положительной.
        /// </summary>
        /// 
        /// <exception cref="ArgumentOutOfRangeException">
        /// Вызывается при несоответствии полей переданной структуры возможным.
        /// </exception>
        public double JulianDate
        {
            get
            {
                return julianDate;
            }

            set
            {
                if (value <= 0.0)
                {
                    throw new System.ArgumentOutOfRangeException("Julian date isn't correct");
                }

                julianDate = value;
            }
        }

        /// <summary>
        /// Секунда. 0 &lt;= Second &lt; 60
        /// </summary>
        /// 
        /// <exception cref="ArgumentOutOfRangeException">
        /// Вызывается при передаче отрицательного или большего или равного 60 значения.
        /// </exception>
        public double Second
        {
            get
            {
                return GrigorianCalendar.Second;
            }

            set
            {
                if (value < 0.0 || value >= 60.0)
                {
                    throw new System.ArgumentOutOfRangeException("second isn't correct");
                }

                Calendar calendar = GrigorianCalendar;
                calendar.Second = value;
                GrigorianCalendar = calendar;
            }
        }

        /// <summary>
        /// Минута. 0 &lt;= Second &lt; 60
        /// </summary>
        /// 
        /// <exception cref="ArgumentOutOfRangeException">
        /// Вызывается при передаче отрицательного или большего или равного 60 значения.
        /// </exception>
        public int Minute
        {
            get
            {
                return GrigorianCalendar.Minute;
            }

            set
            {
                if (value < 0 || value > 59)
                {
                    throw new System.ArgumentOutOfRangeException("minute isn't correct");
                }

                Calendar calendar = GrigorianCalendar;
                calendar.Minute = value;
                GrigorianCalendar = calendar;
            }
        }

        /// <summary>
        /// Минута. 0 &lt;= Second &lt; 24
        /// </summary>
        /// 
        /// <exception cref="ArgumentOutOfRangeException">
        /// Вызывается при передаче отрицательного или большего или равного 24 значения.
        /// </exception>
        public int Hour
        {
            get
            {
                return GrigorianCalendar.Hour;
            }

            set
            {
                if (value < 0 || value > 23)
                {
                    throw new System.ArgumentOutOfRangeException("hour isn't correct");
                }

                Calendar calendar = GrigorianCalendar;
                calendar.Hour = value;
                GrigorianCalendar = calendar;
            }
        }

        /// <summary>
        /// День.
        /// </summary>
        /// 
        /// <exception cref="ArgumentOutOfRangeException">
        /// Вызывается при передаче недопустимого значения.
        /// </exception>
        public int Day
        {
            get
            {
                return GrigorianCalendar.Day;
            }

            set
            {
                if (!IsDateCorrect(Year, Month, value))
                {
                    throw new System.ArgumentOutOfRangeException("Day isn't correct");
                }

                Calendar calendar = GrigorianCalendar;
                calendar.Day = value;
                GrigorianCalendar = calendar;
            }
        }

        /// <summary>
        /// Месяц. 1 &lt;= month &lt;= 12
        /// </summary>
        /// 
        /// <exception cref="ArgumentOutOfRangeException">
        /// Вызывается при передаче недопустимого значения.
        /// </exception>
        public int Month
        {
            get
            {
                return GrigorianCalendar.Month;
            }

            set
            {
                if(!IsDateCorrect(Year, value, Day))
                {
                    throw new System.ArgumentOutOfRangeException("Month isn't correct");
                }

                Calendar calendar = GrigorianCalendar;
                calendar.Month = value;
                GrigorianCalendar = calendar;
            }
        }

        /// <summary>
        /// Год.
        /// </summary>
        /// 
        /// <exception cref="ArgumentOutOfRangeException">
        /// Вызывается при передаче недопустимого значения.
        /// </exception>
        public int Year
        {
            get
            {
                return GrigorianCalendar.Year;
            }

            set
            {
                if (!IsDateCorrect(value, Month, Day))
                {
                    throw new System.ArgumentOutOfRangeException("Year isn't correct");
                }

                Calendar calendar = GrigorianCalendar;
                calendar.Year = value;
                GrigorianCalendar = calendar;
            }
        }
        #endregion

        #region methods
        /// <summary>
        /// Функция возвращает Юлианский день, соответствующий переданной дате Григорианского календаря.
        /// </summary>
        /// 
        /// <param name="calendar"> Дата григорианского календаря. Должна быть позднее 23 ноября -4713 года.</param>
        /// 
        /// <returns>
        /// Юлианский день, соответствующий переданной дате Григорианского календаря.
        /// </returns>
        /// 
        /// <exception cref="ArgumentException">
        /// Вызывается при передаче некорректной даты.
        /// </exception>
        public static double GetJulianDate(Calendar calendar)
        {
            if(IsCalendarCorrect(calendar))
            {
                throw new ArgumentException("calendar isn't correct");
            }

            int a = (14 - calendar.Month) / 12;
            int y = calendar.Year + 4800 - a;
            int m = calendar.Month + 12 * a - 3;
            double s = ((calendar.Hour * 60.0) + calendar.Minute * 60.0) + calendar.Second;
            return calendar.Day + (int)((153.0 * m + 2.0) / 5) + 365 * y +
                (int)(y / 4.0) - (int)(y / 100.0) + (int)(y / 400.0) - 32045 + s / JDtoSecond;
        }

        /// <summary>
        /// Функция возвращает дату Григорианского календаря, соответствующую переданной Юлианской дате.
        /// </summary>
        /// 
        /// <param name="julianDate"> Юлианская дата. Должна быть положительна.</param>
        /// 
        /// <returns>
        /// Дата Григорианского календаря, соответствующую переданной Юлианской дате.
        /// </returns>
        public static Calendar GetGrigorianCalendar(double julianDate)
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

        /// <summary>
        /// Проверяет, существует ли переданная дата в Григорианском календаре.
        /// </summary>
        /// 
        /// <param name="calendar"> Дата Григорианского календаря.</param>
        /// 
        /// <returns>
        /// true, если дата существует.<br/>
        /// false, если нет.
        /// </returns>
        public static bool IsCalendarCorrect(Calendar calendar)
        {
            return IsDateCorrect(calendar.Year, calendar.Month, calendar.Day) &&
                calendar.Hour < 24 && calendar.Hour >= 0 &&
                calendar.Minute < 60 && calendar.Minute >= 0 &&
                calendar.Second < 60 && calendar.Second >= 0;
        }

        /// <summary>
        /// Функция проверяет, могут ли переданные аргументы соответствовать дате Григорианского календаря,
        /// которую можно перевести в Юлианскую дату.
        /// </summary>
        /// 
        /// <param name="year"> Год</param>
        /// <param name="month"> Месяц</param>
        /// <param name="day"> День</param>
        /// 
        /// <returns>
        /// true, если переданная дата Григорианского календаря существует и ее можно перевести в Юлианскую дату<br/>
        /// false, если нет
        /// </returns>
        public static bool IsDateCorrect(int year, int month, int day)
        {
            // Проверка на то, что дата позже 23 ноября -4713
            if (year < -4713)
            {
                return false;
            }

            else if (year == 4713)
            {
                if (month < 11)
                {
                    return false;
                }
                else if (month == 11)
                {
                    if (day < 23)
                    {
                        return false;
                    }
                }
            }

            // Проверка на корректность месяца
            if(month > 12 || month < 1)
            {
                return false;
            }

            // Проверка на корректность дня
            if (day < 1)
            {
                return false;
            }

            if (month % 2 == 1 && month < 8)
            {
                return day < 32;
            }

            if (month % 2 == 1)
            {
                return day < 31;
            }

            if (month == 2)
            {
                if (IsYearLeap(year))
                {
                    return day < 30;
                }
                else
                    return day < 29;
            }

            if (month % 2 == 0 && month < 8)
                return day < 31;

            return day < 32;
        }

        /// <summary>
        /// Функция проверяет, является ли переданный год високосным в Григорианском календаре.
        /// </summary>
        /// 
        /// <param name="year"> Год Григорианского календаря.</param>
        /// 
        /// <returns>
        /// true, если переданный год является високосным в Григорианском календаре<br/>
        /// false, если нет
        /// </returns>
        public static bool IsYearLeap(int year)
        {
            if (year % 400 == 0)
            {
                return true;
            }

            if (year % 100 == 0)
            {
                return false;
            }

            return year % 4 == 0;
        }
        #endregion

        #region operators
        /// <summary>
        /// Складывает даты.
        /// </summary>
        /// 
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// 
        /// <returns>
        /// Сумма дат.
        /// </returns>
        public static Date operator +(Date date1, Date date2)
        {
            return new Date(date1.julianDate + date2.julianDate);
        }

        /// <summary>
        /// Вычитает даты. Уменьшаемая дата должна быть больше вычитаемой.
        /// </summary>
        /// 
        /// <param name="date1"> Уменьшаемая дата</param>
        /// <param name="date2"> Вычитаемая дата</param>
        /// 
        /// <returns>
        /// Разность дат.
        /// </returns>
        /// 
        /// <exception cref="ArgumentException">
        /// Вызывается, если уменьшаемая дата меньше вычитаемой.
        /// </exception>
        public static Date operator -(Date date1, Date date2)
        {
            if(date2.julianDate > date1.julianDate)
            {
                throw new ArgumentException("date2 > date1");
            }

            return new Date(date1.julianDate - date2.julianDate);
        }

        /// <summary>
        /// Складывает дату с Юлианской датой. Сумма дат должна быть положительна.
        /// </summary>
        /// 
        /// <param name="date"> Дата</param>
        /// <param name="julianDate"> Юлианская дата</param>
        /// 
        /// <returns>
        /// Сумма дат.
        /// </returns>
        /// 
        /// <exception cref="ArgumentException">
        /// Вызывается, если сумма дат неположительна.
        /// </exception>
        public static Date operator +(Date date, double julianDate)
        {
            if (julianDate + date.julianDate <= 0)
            {
                throw new ArgumentException("result < 0");
            }

            return new Date(date.julianDate + julianDate);
        }

        /// <summary>
        /// Складывает дату с Юлианской датой. Сумма дат должна быть положительна.
        /// </summary>
        /// 
        /// <param name="date"> Дата</param>
        /// <param name="julianDate"> Юлианская дата</param>
        /// 
        /// <returns>
        /// Сумма дат.
        /// </returns>
        /// 
        /// <exception cref="ArgumentException">
        /// Вызывается, если сумма дат неположительна.
        /// </exception>
        public static Date operator +(double julianDate, Date date)
        {
            return date + julianDate;
        }

        /// <summary>
        /// Вычитает даты. Уменьшаемая дата должна быть больше вычитаемой.
        /// </summary>
        /// 
        /// <param name="date"> Уменьшаемая дата</param>
        /// <param name="julianDate"> Вычитаемая Юлианская дата</param>
        /// 
        /// <returns>
        /// Разность дат.
        /// </returns>
        /// 
        /// <exception cref="ArgumentException">
        /// Вызывается, если уменьшаемая дата меньше вычитаемой.
        /// </exception>
        public static Date operator -(Date date, double julianDate)
        {
            if(date.julianDate < julianDate)
            {
                throw new ArgumentException("julianDate > date");
            }

            return new Date(date.julianDate - julianDate);
        }

        /// <summary>
        /// Вычитает даты. Уменьшаемая дата должна быть больше вычитаемой.
        /// </summary>
        /// 
        /// <param name="date"> Вычитаемая дата</param>
        /// <param name="julianDate"> Уменьшаемая Юлианская дата</param>
        /// 
        /// <returns>
        /// Разность дат.
        /// </returns>
        /// 
        /// <exception cref="ArgumentException">
        /// Вызывается, если уменьшаемая дата меньше вычитаемой.
        /// </exception>
        public static Date operator -(double julianDate, Date date)
        {
            if(julianDate < date.julianDate)
            {
                throw new ArgumentException("julianDate < date");
            }

            return new Date(julianDate - date.julianDate);
        }

        public static bool operator ==(Date date1, Date date2)
        {
            return date1.julianDate == date2.julianDate;
        }

        public static bool operator !=(Date date1, Date date2)
        {
            return date1.julianDate != date2.julianDate;
        }

        public static bool operator >(Date date1, Date date2)
        {
            return date1.julianDate > date2.julianDate;
        }

        public static bool operator <(Date date1, Date date2)
        {
            return date1.julianDate < date2.julianDate;
        }

        public static bool operator >=(Date date1, Date date2)
        {
            return date1.julianDate >= date2.julianDate;
        }

        public static bool operator <=(Date date1, Date date2)
        {
            return date1.julianDate <= date2.julianDate;
        }

        public static bool operator ==(double date1, Date date2)
        {
            return date1 == date2.julianDate;
        }

        public static bool operator ==(Date date1, double date2)
        {
            return date2 == date1.julianDate;
        }

        public static bool operator !=(double date1, Date date2)
        {
            return date1 != date2.julianDate;
        }

        public static bool operator !=(Date date1, double date2)
        {
            return date2 != date1.julianDate;
        }

        public static bool operator >(double date1, Date date2)
        {
            return date1 > date2.julianDate;
        }

        public static bool operator >(Date date1, double date2)
        {
            return date1.julianDate > date2;
        }

        public static bool operator <(double date1, Date date2)
        {
            return date1 < date2.julianDate;
        }

        public static bool operator <(Date date1, double date2)
        {
            return date1.julianDate < date2;
        }

        public static bool operator >=(double date1, Date date2)
        {
            return date1 >= date2.julianDate;
        }

        public static bool operator >=(Date date1, double date2)
        {
            return date1.julianDate >= date2;
        }

        public static bool operator <=(double date1, Date date2)
        {
            return date1 <= date2.julianDate;
        }

        public static bool operator <=(Date date1, double date2)
        {
            return date1.julianDate <= date2;
        }
        #endregion
    }
}
