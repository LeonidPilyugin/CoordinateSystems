using System;
using System.Collections.Generic;

// Файл содержит класс CoordinateSystem

namespace CoordinateSystems
{
    /// <summary>
    /// Класс CoordinateSystem описывает систему координат.
    /// </summary>
    /// 
    /// <remarks>
    /// Поля:<br/>
    /// 1) <see cref="vector"/><br/>
    /// 2) <see cref="basis"/><br/>
    /// 3) <see cref="velocity"/><br/>
    /// 4) <see cref="referenceSystem"/><br/>
    /// <br/>
    /// Конструкторы:<br/>
    /// 1) <see cref="CoordinateSystem(Vector, Basis, Vector, CoordinateSystem)"/><br/>
    /// 2) <see cref="CoordinateSystem()"/><br/>
    /// 3) <see cref="CoordinateSystem(CoordinateSystem)"/><br/>
    /// <br/>
    /// Свойства:<br/>
    /// 1) <see cref="Vector"/><br/>
    /// 2) <see cref="Basis"/><br/>
    /// 3) <see cref="ReferenceSystem"/><br/>
    /// 4) <see cref="Velocity"/><br/>
    /// 5) <see cref="TransitionMatrix"/><br/>
    /// 6) <see cref="TransitionMatrixRelativelyRoot"/><br/>
    /// 7) <see cref="BasisRelativelyReferenceSystem"/><br/>
    /// 8) <see cref="BasisRelativelyRoot"/><br/>
    /// 9) <see cref="RootBasis"/><br/>
    /// 10) <see cref="ReferenceSystemBasis"/><br/>
    /// 11) <see cref="VectorFromRoot"/><br/>
    /// 12) <see cref="VelocityFromRoot"/><br/>
    /// <br/>
    /// Функции:<br/>
    /// 1) <see cref="ConvertTo(CoordinateSystem, Vector)"/><br/>
    /// 2) <see cref="TurnTo(Vector, List&lt;Vector&gt;)"/><br/>
    /// 3) <see cref="GetVectorFromRoot(Vector)"/><br/>
    /// 4) <see cref="GetVectorRelativelyReferenceSystem(Vector)"/><br/>
    /// 3) <see cref="GetVelocityFromRoot(Vector)"/><br/>
    /// 4) <see cref="GetVelocityRelativelyReferenceSystem(Vector)"/><br/>
    /// </remarks>
    public class CoordinateSystem
    {
        #region data
        /// <summary>
        /// Вектор от базовой системы координат
        /// </summary>
        protected Vector vector;

        /// <summary>
        /// Базис относительно базовой системы координат
        /// </summary>
        protected Basis basis;

        /// <summary>
        /// Скорость относительно базовой системы координат.
        /// </summary>
        protected Vector velocity;

        /// <summary>
        /// Базовая система координат. Если значение null, то положение задано относительно корня.
        /// В этом проекте корнем является гелиоцентрическая (вторая экваториальная с центром в Солнце) система координат.
        /// </summary>
        protected CoordinateSystem referenceSystem;
        #endregion

        #region constructors
        /// <summary>
        /// Конструктор, задающий поля. они не должны быть null.
        /// </summary>
        /// 
        /// <param name="vector"> Вектор относительно базовой системы координат.</param>
        /// <param name="basis"> Базис относительно базовой системы координат.</param>
        /// <param name="velocity"> Скорость относительно базовой системы координат.</param>
        /// <param name="referenceSystem"> Базовая система координат. Если она null, то базовая система координат — гелиоцентрическая.</param>
        public CoordinateSystem(Vector vector, Basis basis, Vector velocity, CoordinateSystem referenceSystem = null)
        {
            this.vector = vector;
            this.basis = basis;
            this.velocity = velocity;
            this.referenceSystem = referenceSystem;
        }

        /// <summary>
        /// Задает парметры конструкторами по умолчанию, а referenceSystem — null.
        /// </summary>
        public CoordinateSystem()
        {
            this.vector = new Vector();
            this.basis = new Basis();
            this.velocity = new Vector();
            this.referenceSystem = null;
        }

        /// <summary>
        /// Конструктор копирования.
        /// </summary>
        /// 
        /// <param name="coordinateSystem"> Копируемая система координат. Не должна быть null.</param>
        public CoordinateSystem(CoordinateSystem coordinateSystem)
        {
            this.vector = new Vector(coordinateSystem.vector);
            this.basis = new Basis(coordinateSystem.basis);
            this.referenceSystem = coordinateSystem.referenceSystem;
            this.velocity = new Vector(referenceSystem.velocity);
        }
        #endregion

        #region properties
        /// <summary>
        /// Вектор в базовой системе координат, направленный на эту систему координат. Не должен быть null.
        /// </summary>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Вызывается при передаче null.
        /// </exception>
        public Vector Vector
        {
            get
            {
                return vector;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Vector can't be null");
                }
                vector = value;
            }
        }

        /// <summary>
        /// Базис относительно базовой системы координат. Не должен быть null.
        /// </summary>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Вызывается при передаче null.
        /// </exception>
        public Basis Basis
        {
            get
            {
                return basis;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Basis can't be null");
                }
                basis = value;
            }
        }

        /// <summary>
        /// Базовая система координат. Если равна null, то базовой системой координат считается корень
        /// (в этом проекте корень — гелиоцентрическая система координат).
        /// </summary>
        public CoordinateSystem ReferenceSystem
        {
            get
            {
                return referenceSystem;
            }

            set
            {
                referenceSystem = value;
            }
        }

        /// <summary>
        /// Скорость относительно базовой системы координат. Не должна быть null.
        /// </summary>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Вызывается при передаче null.
        /// </exception>
        public Vector Velocity
        {
            get
            {
                return velocity;
            }

            set
            {
                if(value == null)
                {
                    throw new ArgumentNullException("Vector mustn't be null");
                }

                velocity = value;
            }
        }

        /// <summary>
        /// Матрица перехода от базовой к этой системе координат.
        /// </summary>
        protected Matrix TransitionMatrix
        {
            get
            {
                return new Matrix(basis).Transposed;
            }
        }

        /// <summary>
        /// Матрица перехода от корня к этой системе координат.
        /// </summary>
        protected Matrix TransitionMatrixRelativelyRoot
        {
            get
            {
                Matrix result = new Matrix();
                var temp = this;
                var matrices = new Stack<Matrix>();

                do
                {
                    matrices.Push(temp.TransitionMatrix);
                    temp = temp.ReferenceSystem;
                } while (temp != null);

                do
                {
                    result *= matrices.Pop();
                } while (matrices.Count > 0);

                return result;
            }
        }

        /// <summary>
        /// Базис относительно базовой системы координат.
        /// </summary>
        protected Basis BasisRelativelyReferenceSystem
        {
            get
            {
                return new Basis(TransitionMatrix);
            }
        }

        /// <summary>
        /// Базис относительно корня.
        /// </summary>
        protected Basis BasisRelativelyRoot
        {
            get
            {
                return new Basis(TransitionMatrixRelativelyRoot);
            }
        }

        /// <summary>
        /// Базис корня в этой системе координат.
        /// </summary>
        protected Basis RootBasis
        {
            get
            {
                return new Basis(TransitionMatrixRelativelyRoot.Inversed);
            }
        }

        /// <summary>
        /// Базис базовой системы координат в этой системе координат.
        /// </summary>
        protected Basis ReferenceSystemBasis
        {
            get
            {
                return new Basis(TransitionMatrix.Inversed);
            }
        }

        /// <summary>
        /// Вектор к этой системе координат относительно корня.
        /// </summary>
        public Vector VectorFromRoot
        {
            get
            {
                return GetVectorFromRoot();
            }
        }

        /// <summary>
        /// Скорость относительно корня.
        /// </summary>
        public Vector VelocityFromRoot
        {
            get
            {
                return GetVelocityFromRoot();
            }
        }
        #endregion

        #region functions
        /// <summary>
        /// Переводит вектор point, заданный в этой системе координат в систему координат System.
        /// </summary>
        /// 
        /// <param name="system"> Новая система координат.</param>
        /// <param name="point"> Переводимый вектор. Если отсутствует, то принимается за нулевой вектор.</param>
        /// 
        /// <returns>
        /// Переведенный вектор.
        /// </returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Вызывается при передаче null в параметр system.
        /// </exception>
        public Vector ConvertTo(CoordinateSystem system, Vector point = null)
        {
            if(system == null)
            {
                throw new ArgumentNullException("system mustn't be null");
            }

            if (point == null)
            {
                point = new Vector(0.0, 0.0, 0.0);
            }

            return system.TransitionMatrixRelativelyRoot.Inversed * (GetVectorFromRoot(point) - system.GetVectorFromRoot());
        }

        /// <summary>
        /// Поворачивает эту систему координат осью X на точку target.
        /// Точки из списка поворачиваются так, что их координаты в этой
        /// системе координат не меняются.
        /// </summary>
        /// 
        /// <param name="target"> Точка в этой системе координат, на которую поворачивается эта.</param>
        /// <param name="points"> Список точек, которые поворачиваются вместе с этой системой координат.</param>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Вызывается, если в параметр target передается null.
        /// </exception>
        public void TurnTo(Vector target, List<Vector> points = null) 
        {
            if(target == null)
            {
                throw new ArgumentNullException("target mustn't be null");
            }

            UnitVector direction = new UnitVector(target);
            UnitVector axis = direction * basis.I;
            double angle = -Vector.GetAngle(direction, basis.I);

            if(points != null)
            {
                foreach (Vector point in points)
                {
                    point.TurnAxis(axis, angle);
                }
            }

            basis.TurnAxis(axis, angle);
        }

        /// <summary>
        /// Возвращает вектор, переданный в параметре относительно корня. 
        /// </summary>
        /// 
        /// <param name="point"> Переводимый вектор. Если равен null, то принимается за нулевой вектор.</param>
        /// 
        /// <returns>
        /// Вектор, переведенный в систему координат корня.
        /// </returns>
        protected Vector GetVectorFromRoot(Vector point = null)
        {
            var result = point;
            if(result == null)
            {
                result = new Vector(0.0, 0.0, 0.0);
            }
            var temp = this;

            do
            {
                result = temp.GetVectorRelativelyReferenceSystem(result);
                temp = temp.ReferenceSystem;
            } while (temp != null);
            return result;
        }

        /// <summary>
        /// Возвращает вектор, переданный в параметре относительно базовой системы координат. 
        /// </summary>
        /// 
        /// <param name="point"> Переводимый вектор. Если равен null, то принимается за нулевой вектор.</param>
        /// 
        /// <returns>
        /// Вектор, переведенный в базовую систему координат.
        /// </returns>
        protected Vector GetVectorRelativelyReferenceSystem(Vector point = null)
        {
            var result = new Vector(0.0, 0.0, 0.0);

            if (point != null)
            {
                result = TransitionMatrix * point;
            }

            return result + vector;
        }

        /// <summary>
        /// Возвращает вектор скорости, переданный в параметре относительно корня. 
        /// </summary>
        /// 
        /// <param name="point"> Переводимый вектор скорости. Если равен null, то принимается за нулевой вектор.</param>
        /// 
        /// <returns>
        /// Вектор скорости, переведенный в систему координат корня.
        /// </returns>
        protected Vector GetVelocityFromRoot(Vector velocity = null)
        {
            var result = velocity;
            if (result == null)
            {
                result = new Vector(0.0, 0.0, 0.0);
            }
            var temp = this;

            do
            {
                result = temp.GetVelocityRelativelyReferenceSystem(result);
                temp = temp.ReferenceSystem;
            } while (temp != null);
            return result;
        }

        /// <summary>
        /// Возвращает вектор скорости, переданный в параметре относительно базовой системы координат. 
        /// </summary>
        /// 
        /// <param name="point"> Переводимый вектор скорости. Если равен null, то принимается за нулевой вектор.</param>
        /// 
        /// <returns>
        /// Вектор скорости, переведенный в базовую систему координат.
        /// </returns>
        protected Vector GetVelocityRelativelyReferenceSystem(Vector point = null)
        {
            var result = new Vector(0.0, 0.0, 0.0);

            if (point != null)
            {
                result = TransitionMatrix * point;
            }

            return result + velocity;
        }
        #endregion
    }
}
