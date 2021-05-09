using System;
using CoordinateSystems;
using Date;
using IAUSOFA;

// Файл содержии класс Planet.

namespace SunSystem
{
    /// <summary>
    /// Класс Planet описывает крупное небесное тело, имеющее форму эллипсоида вращения. Наследник класса <see cref="Body"/>.
    /// </summary>
    /// 
    /// <remarks>
    /// Типы:<br/>
    /// 1) <see cref="GetParams"/><br/>
    /// <br/>
    /// Поля:<br/>
    /// 1) <see cref="CoordinateSystem.vector"/><br/>
    /// 2) <see cref="CoordinateSystem.velocity"/><br/>
    /// 3) <see cref="CoordinateSystem.referenceSystem"/><br/>
    /// 4) <see cref="CoordinateSystem.basis"/><br/>
    /// 5) <see cref="CoordinateSystem.vector"/><br/>
    /// 6) <see cref="Body.id"/><br/>
    /// 7) <see cref="Body.bodies"/><br/>
    /// 8) <see cref="mass"/><br/>
    /// 9) <see cref="maxRadius"/><br/>
    /// 10) <see cref="minRadius"/><br/>
    /// 11) <see cref="GetParamsMethod"/><br/>
    /// <br/>
    /// Конструкторы:<br/>
    /// 1) <see cref="Planet(double, double, double, string, Vector, Basis, Vector, CoordinateSystem)"/><br/>
    /// <br/>
    /// Свойства:<br/>
    /// 1) <see cref="Mass"/><br/>
    /// <br/>
    /// Методы:<br/>
    /// 1) <see cref="IsInside(Vector)"/><br/>
    /// 2) <see cref="IsCrossing(Vector, Vector)"/><br/>
    /// 3) <see cref="GetDirectionVector(Vector, Vector)"/><br/>
    /// 4) <see cref="GetProjection(Vector, Vector, Vector)"/><br/>
    /// 5) <see cref="UpdateParams(double)"/><br/>
    /// 6) <see cref="ToString()"/><br/>
    /// </remarks>
    public class Planet : Body
    {
        /// <summary>
        /// Делегат описывает метод для получения положения и скорости тела.
        /// </summary>
        /// 
        /// <param name="julianDate"> Юлианская дата.</param>
        /// 
        /// <returns>
        /// Положение и скорость тела.
        /// </returns>
        public delegate (Vector Vector, Vector Velocity, Basis Basis) GetParams(double julianDate);

        #region data
        /// <summary>
        /// Масса. Должна быть положительна.
        /// </summary>
        protected double mass;

        /// <summary>
        /// Полярный радиус. Должен быть положителен.
        /// </summary>
        protected double minRadius;

        /// <summary>
        /// Экваториальный радиус. Должен быть положителен.
        /// </summary>
        protected double maxRadius;
        #endregion

        #region constructors
        /// <summary>
        /// Конструктор присваивает полям значения, переданные в аргументах.
        /// </summary>
        /// 
        /// <param name="mass"> Масса. Должна быть положительна.</param>
        /// <param name="minRadius"> Полярный радиус. Должен быть положителен.</param>
        /// <param name="maxRadius"> Экваториальный радиус. Должен быть положителен.</param>
        /// <param name="id"> Идентификатор. Не должен быть null.</param>
        /// <param name="vector"> Вектор в базовой системе координат, направленный в барицентр планеты. Не должен быть null.</param>
        /// <param name="basis"> Базис относительно базовой системы координат. Не должен быть null.</param>
        /// <param name="velocity"> Скорость относительно базовой системы координат. Не должна быть null.</param>
        /// <param name="referenceSystem"> Базовая система координат.</param>
        /// 
        /// <exception cref="ArgumentException">
        /// Вызывается при передаче некорректного значения.
        /// </exception>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Вызывается при передаче bull.
        /// </exception>
        public Planet(double mass, double minRadius, double maxRadius,
            string id, Vector vector, Basis basis, Vector velocity, CoordinateSystem referenceSystem) :
            base(id, vector, basis, velocity, referenceSystem)
        {
            this.mass = mass;
            MinRadius = minRadius;
            MaxRadius = maxRadius;
            Body.bodies.Add(this);
        }
        #endregion

        #region properties
        /// <summary>
        /// Масса. Должны быть положительной.
        /// </summary>
        /// 
        /// <exception cref="ArgumentException">
        /// Вызывается при передаче неположительного значения.
        /// </exception>
        public double Mass
        {
            get
            {
                return mass;
            }

            set
            {
                if(value <= 0.0)
                {
                    throw new ArgumentException("Mass must be positive");
                }

                mass = value;
            }
        }

        /// <summary>
        /// Полярный радиус. Должен быть положителен.
        /// </summary>
        /// 
        /// <exception cref="ArgumentException">
        /// Вызывается при передаче неположительного значения.
        /// </exception>
        public double MinRadius
        {
            get
            {
                return minRadius;
            }

            set
            {
                if (value <= 0.0)
                {
                    throw new ArgumentException("MinRadius must be positive");
                }

                minRadius = value;
            }
        }

        /// <summary>
        /// Экваториальный радиус. Должен быть положителен.
        /// </summary>
        /// 
        /// <exception cref="ArgumentException">
        /// Вызывается при передаче неположительного значения.
        /// </exception>
        public double MaxRadius
        {
            get
            {
                return maxRadius;
            }

            set
            {
                if (value <= 0.0)
                {
                    throw new ArgumentException("MaxRadius must be positive");
                }

                maxRadius = value;
            }
        }
        #endregion

        #region methods
        /// <summary>
        /// Функция, получающая положение и скорость тела.
        /// </summary>
        public GetParams GetParamsMethod;

        /// <inheritdoc/>
        public override bool IsInside(Vector point)
        {
            if(point == null)
            {
                throw new ArgumentNullException("point mustn,t be null");
            }

            return point.X * point.X / maxRadius / maxRadius +
            point.Y * point.Y / maxRadius / maxRadius +
            point.Z * point.Z / minRadius / minRadius < 1.0;
        }

        /// <inheritdoc/>
        public override bool IsCrossing(Vector vector1, Vector vector2)
        {
            Vector projection = GetProjection(vector1, vector2, new Vector(0.0, 0.0, 0.0));
            if (!IsInside(projection))
                return false;

            if (vector1.X != vector2.X)
                return (projection.X < vector1.X && projection.X > vector2.X) ||
                    (projection.X < vector2.X && projection.X > vector1.X);

            if (vector1.Y != vector2.Y)
                return (projection.Y < vector1.Y && projection.Y > vector2.Y) ||
                (projection.Y < vector2.Y && projection.Y > vector1.Y);

            return (projection.Z < vector1.Z && projection.Z > vector2.Z) ||
                (projection.Z < vector2.Z && projection.Z > vector1.Z);
        }

        /// <summary>
        /// Единичный вектор, сонаправленный с вектором point2 - point1.
        /// </summary>
        /// 
        /// <param name="point1"> Конец отрезка. Не должен быть null.</param>
        /// <param name="point2"> Конец отрезка. Не должен быть null.</param>
        /// 
        /// <returns>
        /// Единичный вектор, сонаправленный с вектором point2 - point1.
        /// </returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Вызывается, если хотя бы один параметр равен null.
        /// </exception>
        protected UnitVector GetDirectionVector(Vector point1, Vector point2)
        {
            return new UnitVector(point2 - point1);
        }

        /// <summary>
        /// Получает проекцию точки на прямую.
        /// </summary>
        /// 
        /// <param name="vector1"> Точка на прямой. Не должна быть null.</param>
        /// <param name="vector2"> Точка на прямой. Не должна быть null.</param>
        /// <param name="point"> Проецируемая точка. Не должна быть null.</param>
        /// 
        /// <returns>
        /// Проекция точки point на прямую vector1 vector2.
        /// </returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Вызывается, если хотя бы один параметр равен null.
        /// </exception>
        protected Vector GetProjection(Vector vector1, Vector vector2, Vector point)
        {
            Vector direction = GetDirectionVector(vector1, vector2);
            return vector1 - direction * Vector.MultiplyScalar((vector1 - point), direction)
                / direction.Length / direction.Length;
        }

        /// <summary>
        /// Обновляет поля vector, velocity и basis значениями для указанной юлианской даты.
        /// </summary>
        /// 
        /// <param name="julianDate"> Юлианская дата.</param>
        public void UpdateParams(double julianDate)
        {
            (vector, velocity, basis) = GetParamsMethod(julianDate);
        }

        /// <summary>
        /// Возвращает строку: "[x_posistion] [y_posistion] [z_posistion] [x_velocity] [y_velocity] [z_velocity]".
        /// </summary>
        /// 
        /// <returns>
        /// Строка: "[x_posistion] [y_posistion] [z_posistion] [x_velocity] [y_velocity] [z_velocity]".
        /// </returns>
        public override string ToString()
        {
            return vector.ToString() + " " + velocity.ToString();
        }
        #endregion
    }
}
