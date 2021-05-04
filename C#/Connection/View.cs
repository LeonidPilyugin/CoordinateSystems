using System;
using CoordinateSystems;
using static System.Math;

// Файл содержит классы View, ConicView и PyramidView.

namespace Connection
{
    /// <summary>
    /// Абстрактный класс View описывает область видимости.
    /// </summary>
    /// 
    /// <remarks>
    /// Поля:<br/>
    /// 1) <see cref="length"/><br/>
    /// <br/>
    /// Конструкторы:<br/>
    /// 1) <see cref="View(double)"/><br/>
    /// <br/>
    /// Свойства:<br/>
    /// 1) <see cref="Axis"/><br/>
    /// 2) <see cref="Length"/><br/>
    /// <br/>
    /// Методы:<br/>
    /// 1) <see cref="IsInside(Vector)"/><br/>
    /// </remarks>
    public abstract class View
    {
        #region data
        /// <summary>
        /// Максимальное расстояние.
        /// </summary>
        protected double length;
        #endregion

        #region constructors
        /// <summary>
        /// Конструктор задает полям переданные значения.
        /// </summary>
        /// 
        /// <param name="length"> Максимальное расстояние. Должно быть положительно.</param>
        public View(double length)
        {
            this.length = length;
        }
        #endregion

        #region properties
        /// <summary>
        /// Ось симметрии.
        /// </summary>
        public Vector Axis
        {
            get
            {
                return new Vector(length, 0.0, 0.0);
            }
        }

        /// <summary>
        /// Максимальное расстояние. Должно быть положительным.
        /// </summary>
        /// 
        /// <exception cref="ArgumentException">
        /// Вызывается при передаче неположительного значения.
        /// </exception>
        public double Length
        {
            get
            {
                return length;
            }

            set
            {
                if (value <= 0.0)
                {
                    throw new ArgumentException("Length must be > 0");
                }

                length = value;
            }
        }
        #endregion

        #region methods
        /// <summary>
        /// Проверяет принадлежность точки области видимости.
        /// </summary>
        /// 
        /// <param name="point"> Точка.</param>
        /// 
        /// <returns>
        /// true, если точка внутри области видимости.<br/>
        /// false, если нет.
        /// </returns>
        public abstract bool IsInside(Vector point);
        #endregion
    }

    /// <summary>
    /// Класс ConicView описывает область видимости в виде конуса. Наследник класса <see cref="View"/>.
    /// </summary>
    /// 
    /// <remarks>
    /// Поля:<br/>
    /// 1) <see cref="View.length"/><br/>
    /// 2) <see cref="angle"/><br/>
    /// <br/>
    /// Конструкторы:<br/>
    /// 1) <see cref="ConicView(double, double)"/><br/>
    /// 2) <see cref="ConicView(ConicView)"/><br/>
    /// <br/>
    /// Свойства:<br/>
    /// 1) <see cref="View.Axis"/><br/>
    /// 2) <see cref="View.Length"/><br/>
    /// 3) <see cref="Angle"/><br/>
    /// <br/>
    /// Методы:<br/>
    /// 1) <see cref="IsInside(Vector)"/><br/>
    /// </remarks>
    public class ConicView : View
    {
        #region data
        /// <summary>
        /// Угол полураствора.
        /// </summary>
        protected double angle;
        #endregion

        #region constructors
        /// <summary>
        /// Конструктор задает значениям переданные параметры.
        /// </summary>
        /// 
        /// <param name="angle"> Угол полураствора. Измеряется в радианах. 0 &lt; angle &lt;= <see cref="Math.PI"/> / 2.</param>
        /// <param name="length"> Максимальное расстояние. Должно быть положительным.</param>
        public ConicView(double length, double angle) : base(length)
        {
            this.angle = angle;
        }

        /// <summary>
        /// Конструктор копирования.
        /// </summary>
        /// 
        /// <param name="view"> Копируемый ConicView. Не должен быть null.</param>
        public ConicView(ConicView view) : this(view.angle, view.length)
        {

        }
        #endregion

        #region properties
        /// <summary>
        /// Угол полураствора. Измеряется в радианах. 0 &lt; angle &lt;= <see cref="Math.PI"/> / 2.
        /// </summary>
        /// 
        /// <exception cref="ArgumentException">
        /// Вызывается при передаче неположительного или большего <see cref="Math.PI"/> / 2 значения.
        /// </exception>
        public double Angle
        {
            get
            {
                return angle;
            }

            set
            {
                if(value <= 0.0 || value > PI / 2.0)
                {
                    throw new Exception("Angle must be <= PI / 2.0 and > 0.0");
                }

                angle = value;
            }
        }
        #endregion

        #region functions
        /// <inheritdoc/>
        public override bool IsInside(Vector point)
        {
            return (Abs(Cos(Vector.GetAngle(point, Axis))) >= Cos(angle)) && (point.Length <= length);
        }
        #endregion
    }

    /// <summary>
    /// Класс PyramidView описывает область видимости в виде пирамиды. Наследник класса <see cref="View"/>.
    /// </summary>
    /// 
    /// <remarks>
    /// Поля:<br/>
    /// 1) <see cref="View.length"/><br/>
    /// 2) <see cref="angle1"/><br/>
    /// 3) <see cref="angle2"/><br/>
    /// <br/>
    /// Конструкторы:<br/>
    /// 1) <see cref="PyramidView(double, double, double)"/><br/>
    /// 2) <see cref="PyramidView(PyramidView)"/><br/>
    /// <br/>
    /// Свойства:<br/>
    /// 1) <see cref="View.Axis"/><br/>
    /// 2) <see cref="View.Length"/><br/>
    /// 3) <see cref="Angle1"/><br/>
    /// 4) <see cref="Angle2"/><br/>
    /// <br/>
    /// Методы:<br/>
    /// 1) <see cref="IsInside(Vector)"/><br/>
    /// </remarks>
    public class PyramidView : View
    {
        #region data
        /// <summary>
        /// Угол полураствора, лежащий в плоскости XY.
        /// </summary>
        protected double angle1;

        /// <summary>
        /// Угол полураствора, лежащий в плоскости XZ.
        /// </summary>
        protected double angle2;
        #endregion

        #region constructors
        /// <summary>
        /// Конструктор задает значениям переданные параметры.
        /// </summary>
        /// 
        /// <param name="angle1"> Угол полураствора, лежащий в плоскости XY. Измеряется в радианах. 0 &lt; angle1 &lt;= <see cref="Math.PI"/> / 2.</param>
        /// <param name="angle2"> Угол полураствора, лежащий в плоскости XZ. Измеряется в радианах. 0 &lt; angle2 &lt;= <see cref="Math.PI"/> / 2.</param>
        /// <param name="length"> Максимальное расстояние. Должно быть положительно.</param>
        public PyramidView(double angle1, double angle2, double length) : base(length)
        {
            this.angle1 = angle1;
            this.angle2 = angle2;
        }

        /// <summary>
        /// Конструктор копирования.
        /// </summary>
        /// 
        /// <param name="view"> Копируемый PyramidView. Не должен быть null.</param>
        public PyramidView(PyramidView view) : this(view.angle1, view.angle2, view.length)
        {

        }
        #endregion

        #region properties
        /// <summary>
        /// Угол полураствора, лежащий в плоскости XY. Измеряется в радианах. 0 &lt; angle1 &lt;= <see cref="Math.PI"/> / 2.
        /// </summary>
        /// 
        /// <exception cref="ArgumentException">
        /// Вызывается при передаче неположительного или большего <see cref="Math.PI"/> / 2 значения.
        /// </exception>
        public double Angle1
        {
            get
            {
                return angle1;
            }

            set
            {
                if (value <= 0.0 || value > PI / 2.0)
                {
                    throw new ArgumentException("Angle1 must be <= PI / 2.0 and > 0.0");
                }

                angle1 = value;
            }
        }

        /// <summary>
        /// Угол полураствора, лежащий в плоскости XZ. Измеряется в радианах. 0 &lt; angle1 &lt;= <see cref="Math.PI"/> / 2.
        /// </summary>
        /// 
        /// <exception cref="ArgumentException">
        /// Вызывается при передаче неположительного или большего <see cref="Math.PI"/> / 2 значения.
        /// </exception>
        public double Angle2
        {
            get
            {
                return angle2;
            }

            set
            {
                if (value <= 0.0 || value > PI / 2.0)
                {
                    throw new ArgumentException("Angle1 must be <= PI / 2.0 and > 0.0");
                }

                angle2 = value;
            }
        }
        #endregion

        #region functions
        /// <inheritdoc/>
        public override bool IsInside(Vector point)
        {
            return (point.X * Tan(angle1) >= Abs(point.Y)) &&
                (point.X * Tan(angle2) >= Abs(point.Z)) && (point.Length <= length);
        }
        #endregion
    }
}
