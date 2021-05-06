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
    /// 2) <see cref="ID"/><br/>
    /// 3) <see cref="CoordinateSystem.Vector"/><br/>
    /// 4) <see cref="CoordinateSystem.Basis"/><br/>
    /// 5) <see cref="CoordinateSystem.ReferenceSystem"/><br/>
    /// 6) <see cref="CoordinateSystem.Velocity"/><br/>
    /// 7) <see cref="CoordinateSystem.TransitionMatrix"/><br/>
    /// 8) <see cref="CoordinateSystem.TransitionMatrixRelativelyRoot"/><br/>
    /// 9) <see cref="CoordinateSystem.BasisRelativelyReferenceSystem"/><br/>
    /// 10) <see cref="CoordinateSystem.BasisRelativelyRoot"/><br/>
    /// 11) <see cref="CoordinateSystem.RootBasis"/><br/>
    /// 12) <see cref="CoordinateSystem.ReferenceSystemBasis"/><br/>
    /// 13) <see cref="CoordinateSystem.VectorFromRoot"/><br/>
    /// 14) <see cref="CoordinateSystem.VelocityFromRoot"/><br/>
    /// <br/>
    /// Методы:<br/>
    /// 1) <see cref="IsInside(Vector)"/><br/>
    /// 2) <see cref="IsCrossing(Vector, Vector)"/><br/>
    /// 3) <see cref="IsCrossing(CoordinateSystem, CoordinateSystem, Vector, Vector)"/><br/>
    /// 4) <see cref="CoordinateSystem.ConvertTo(CoordinateSystem, Vector)"/><br/>
    /// 5) <see cref="CoordinateSystem.TurnTo(Vector, List&lt;Vector&gt;)"/><br/>
    /// 6) <see cref="CoordinateSystem.GetVectorFromRoot(Vector)"/><br/>
    /// 7) <see cref="CoordinateSystem.GetVectorRelativelyReferenceSystem(Vector)"/><br/>
    /// 8) <see cref="CoordinateSystem.GetVelocityFromRoot(Vector)"/><br/>
    /// 9) <see cref="CoordinateSystem.GetVelocityRelativelyReferenceSystem(Vector)"/><br/>
    /// </remarks>
    public class Body : CoordinateSystem
    {
        #region data
        /// <summary>
        /// Все тела.
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
        /// <param name="id"> Идентификатор.</param>
        /// <param name="vector"> Вектор относительно базовой системы координат.</param>
        /// <param name="basis"> Базис относительно базовой системы координат.</param>
        /// <param name="velocity"> Скорость относительно базовой системы координат.</param>
        /// <param name="referenceSystem"> Базовая система координат. Если она null, то базовая система координат — гелиоцентрическая.</param>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Вызывается, если vector, basis, velocity или id равны null.
        /// </exception>
        public Body(string id, Vector vector, Basis basis, Vector velocity, CoordinateSystem referenceSystem = null)
            : base(vector, basis, velocity, referenceSystem)
        {
            if(id == null)
            {
                throw new ArgumentNullException("id mustn't be null");
            }

            this.id = id;
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
        /// 
        /// <exception cref="ArgumentNullException">
        /// Вызывается при передаче null.
        /// </exception>
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
        /// 
        /// <exception cref="ArgumentNullException">
        /// Вызывается при передаче null.
        /// </exception>
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

        #region methods
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

        /// <summary>
        /// Проверяет, пересекает ли отрезок с переданными концами это тело.
        /// </summary>
        /// 
        /// <param name="cs1"> Система координат, в которой задается vector1.</param>м
        /// <param name="cs2"> Система координат, в которой задается vector2.</param>
        /// <param name="vector1"> Конец отрезка, заданный в системе координат cs1. Если равен null, то принимается за нулевой вектор.</param>
        /// <param name="vector2"> Конец отрезка, заданный в системе координат cs2. Если равен null, то принимается за нулевой вектор.</param>
        /// 
        /// <returns>
        /// true, если пересекает, false — если нет.
        /// </returns>
        public static bool IsCrossing(CoordinateSystem cs1, CoordinateSystem cs2,
            Vector vector1 = null, Vector vector2 = null)
        {
            if (cs1 == null)
            {
                throw new ArgumentNullException("body1 mustn't be null");
            }
            if (cs2 == null)
            {
                throw new ArgumentNullException("body2 mustn't be null");
            }

            Vector v1 = cs1.GetVectorFromRoot(vector1);
            Vector v2 = cs2.GetVectorFromRoot(vector2);

            foreach (Body body in Body.Bodies)
            {
                if (body.VectorFromRoot != v1 && body.VectorFromRoot != v2 &&
                    body.IsCrossing(cs1.ConvertTo(body, vector1), cs2.ConvertTo(body, vector2)))
                {
                    return true;
                }
            }

            return false;
        }
        #endregion
    }
}
