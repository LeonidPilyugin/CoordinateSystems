using System;
using System.Collections.Generic;

// Файл содержит класс Body.

namespace CoordinateSystems
{
    /// <summary>
    /// Класс Body описывает тело. Наследник класса <see cref="CoordinateSystem"/>.
    /// </summary>
    /// 
    /// <remarks>
    /// Поля:<br/>
    /// 1) <see cref="bodies"/><br/>
    /// 2) <see cref="id"/><br/>
    /// 3) <see cref="CoordinateSystem.vector"/><br/>
    /// 4) <see cref="CoordinateSystem.velocity"/><br/>
    /// 5) <see cref="CoordinateSystem.basis"/><br/>
    /// 6) <see cref="CoordinateSystem.referenceSystem"/><br/>
    /// <br/>
    /// Конструкторы:<br/>
    /// 1) <see cref="Body()"/><br/>
    /// 2) <see cref="Body(Body)"/><br/>
    /// 3) <see cref="Body(string, CoordinateSystem)"/><br/>
    /// 4) <see cref="Body(string, Vector, Basis, Vector, CoordinateSystem)"/><br/>
    /// <br/>
    /// Свойства:<br/>
    /// 1) <see cref="Bodies"/><br/>
    /// 1) <see cref="ID"/><br/>
    /// </remarks>
    public class Body : CoordinateSystem
    {
        #region data
        /// <summary>
        /// Остальные тела.
        /// </summary>
        protected static List<Body> bodies;

        /// <summary>
        /// Идентификатор.
        /// </summary>
        protected string id;
        #endregion

        #region constructors
        /// <summary>
        /// Конструктор. Задает поля значениями, переданными в аргументах. Они не должны быть null.
        /// </summary>
        /// 
        /// <param name="ID"> Идентификатор.</param>
        /// <param name="vector"> Вектор относительно базовой системы координат.</param>
        /// <param name="basis"> Базис относительно базовой системы координат.</param>
        /// <param name="velocity"> Скорость относительно базовой системы координат.</param>
        /// <param name="referenceSystem"> Базовая система координат. Если она null, то базовая система координат — гелиоцентрическая.</param>
        public Body(string ID, Vector vector, Basis basis, Vector velocity, CoordinateSystem referenceSystem = null)
            : base(vector, basis, velocity, referenceSystem)
        {
            id = ID;
        }

        /// <summary>
        /// Задает парметры конструкторами по умолчанию, referenceSystem = null, id = "New Body".
        /// </summary>
        public Body() : base()
        {
            id = "New body";
        }

        /// <summary>
        /// Конструктор копирования.
        /// </summary>
        /// 
        /// <param name="body"> Копируемое тело.</param>
        public Body(Body body) : base(body.vector, body.basis, body.velocity, body.referenceSystem)
        {
            id = body.id;
        }

        /// <summary>
        /// Конструктор коприует параметры системы координат и ID.
        /// </summary>
        /// 
        /// <param name="ID"> Идентификатор. Не должен быть null.</param>
        /// <param name="coordinateSystem"> Система координат. Не должна быть null.</param>
        public Body(string ID, CoordinateSystem coordinateSystem) :
            this(ID, coordinateSystem.Vector, coordinateSystem.Basis,
                coordinateSystem.Vector, coordinateSystem.ReferenceSystem)
        {

        }

        /// <summary>
        /// Статический конструктор создает список bodies.
        /// </summary>
        static Body()
        {
            bodies = new List<Body>();
        }
        #endregion

        #region properties
        /// <summary>
        /// Остальные тела.
        /// </summary>
        public static List<Body> Bodies
        {
            get
            {
                return bodies;
            }
        }

        /// <summary>
        /// Идентификатор. Не должен быть null.
        /// </summary>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Вызывается при передаче null.
        /// </exception>
        public string ID
        {
            get
            {
                return id;
            }

            set
            {
                if(value == null)
                {
                    throw new ArgumentNullException("ID mustn't be null");
                }

                id = value;
            }
        }
        #endregion

        #region functions
        /// <summary>
        /// Проверяет, находится ли точка внутри тела.
        /// </summary>
        /// 
        /// <param name="point"> Точка в системе координат этого тела.</param>
        /// 
        /// <returns>
        /// true, если точка внутри тела.
        /// false, если снаружи.
        /// </returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Вызывается, если точка равна null.
        /// </exception>
        public virtual bool IsInside(Vector point)
        {
            if(point == null)
            {
                throw new ArgumentNullException("point mustn't be null");
            }

            return false;
        }

        /// <summary>
        /// Проверяет, пересекает ли отрезок с переданными концами это тело.
        /// </summary>
        /// 
        /// <param name="vector1"> Конец отрезка, заданный в системе координат этого тела.</param>
        /// <param name="vector2"> Конец отрезка, заданный в системе координат этого тела.</param>
        /// 
        /// <returns>
        /// true, если пересекает, false — если нет.
        /// </returns>
        public virtual bool IsCrossing(Vector vector1, Vector vector2)
        {
            return false;
        }
        #endregion
    }
}
