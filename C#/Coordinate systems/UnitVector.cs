using System;

// Файл содержит класс UnitVector

namespace CoordinateSystems
{
    /// <summary>
    /// Класс UnitVector описывает единичный вектор в пространстве и поддерживает некоторые операции над ним.
    /// Наследник класса <see cref="Vector"/>.
    /// </summary>
    /// 
    /// <remarks>
    /// Поля:<br/>
    /// 1) <see cref="Vector.x"/><br/>
    /// 2) <see cref="Vector.y"/><br/>
    /// 3) <see cref="Vector.z"/><br/>
    /// <br/>
    /// Конструкторы:<br/>
    /// 1) <see cref="UnitVector()"/><br/>
    /// 2) <see cref="UnitVector(double, double, double)"/><br/>
    /// 3) <see cref="UnitVector(double, double)"/><br/>
    /// 4) <see cref="UnitVector(Vector)"/><br/>
    /// <br/>
    /// Свойства:<br/>
    /// 1) <see cref="X"/><br/>
    /// 2) <see cref="Y"/><br/>
    /// 3) <see cref="Z"/><br/>
    /// 4) <see cref="Length"/><br/>
    /// 5) <see cref="Vector.Theta"/><br/>
    /// 6) <see cref="Vector.Phi"/><br/>
    /// 7) <see cref="Vector.XYprojection"/><br/>
    /// 8) <see cref="Vector.XZprojection"/><br/>
    /// 9) <see cref="Vector.YZprojection"/><br/>
    /// 10) <see cref="UnitVectorX"/><br/>
    /// 11) <see cref="UnitVectorY"/><br/>
    /// 12) <see cref="UnitVectorZ"/><br/>
    /// <br/>
    /// Методы:<br/>
    /// 1) <see cref="Vector.TurnX(double)"/><br/>
    /// 2) <see cref="Vector.TurnY(double)"/><br/>
    /// 3) <see cref="Vector.TurnZ(double)"/><br/>
    /// 4) <see cref="Vector.TurnAxis(Vector, double)"/><br/>
    /// 5) <see cref="Vector.TurnEuler(double, double, double)"/><br/>
    /// 6) <see cref="Vector.GetAngle(Vector, Vector)"/><br/>
    /// 7) <see cref="Vector.MultiplyScalar(Vector, Vector)"/><br/>
    /// 8) <see cref="Vector.ToString()"/><br/>
    /// <br/>
    /// Операторы:<br/>
    /// 1) <see cref="Vector.operator !=(Vector, Vector)"/><br/>
    /// 2) <see cref="Vector.operator ==(Vector, Vector)"/><br/>
    /// 3) <see cref="Vector.operator *(Vector, double)"/><br/>
    /// 4) <see cref="Vector.operator *(Vector, Vector)"/><br/>
    /// 5) <see cref="Vector.operator +(Vector, Vector)"/><br/>
    /// 6) <see cref="Vector.operator -(Vector)"/><br/>
    /// 7) <see cref="Vector.operator -(Vector, Vector)"/><br/>
    /// 8) <see cref="Vector.operator /(Vector, double)"/><br/>
    /// 9) <see cref="operator *(UnitVector, UnitVector)"/><br/>
    /// 10) <see cref="operator +(UnitVector, UnitVector)"/><br/>
    /// 11) <see cref="operator -(UnitVector)"/><br/>
    /// 12) <see cref="operator -(UnitVector, UnitVector)"/><br/>
    /// 13) <see cref="this[int]"/><br/>
    /// </remarks>
    public class UnitVector : Vector
    {
        #region constructors
        /// <summary>
        /// Единичный вектор x = y = z.
        /// </summary>
        public UnitVector() : base()
        {

        }

        /// <summary>
        /// Единичный вектор с координатами, отношение которых равно отношению аргументов.
        /// </summary>
        /// 
        /// <param name="x"> Координата x.</param>
        /// <param name="y"> Координата y.</param>
        /// <param name="z"> Координата z.</param>
        public UnitVector(double x, double y, double z) : base(x, y, z)
        {
            base.Length = 1.0;
        }

        /// <summary>
        /// Единичный вектор со сферическими координатами равными аргументам.
        /// </summary>
        /// 
        /// <param name="theta"> Зенитный угол. Измеряется в радианах. 0 &lt;= Theta &lt;= <see cref="Math.PI"/></param>
        /// <param name="phi"> Азимутальный угол. Измеряется в радианах. -PI &lt;= Phi &lt;= <see cref="Math.PI"/></param>
        /// 
        /// <exception cref="ArgumentException">
        /// Вызывается, если theta или phi не соответствуют возможным.
        /// </exception>
        public UnitVector(double theta, double phi) : base(theta, phi)
        {

        }

        /// <summary>
        /// Конструктор копирования вектора. Сохраняются углы копируемого вектора.
        /// </summary>
        /// 
        /// <param name="vector"> Копируемый вектор. Не должен быть null.</param>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Вызывается при передаче null.
        /// </exception>
        public UnitVector(Vector vector) : base(vector)
        {
            base.Length = 1.0;
        }
        #endregion

        #region properties
        /// <summary>
        /// Длина вектора. Всегда равна 1.
        /// </summary>
        new public double Length
        {
            get
            {
                return 1.0;
            }
        }

        /// <summary>
        /// Координата X.
        /// </summary>
        new public double X
        {
            get
            {
                return x;
            }
        }

        /// <summary>
        /// Координата Z.
        /// </summary>
        new public double Y
        {
            get
            {
                return y;
            }
        }

        /// <summary>
        /// Координата X.
        /// </summary>
        new public double Z
        {
            get
            {
                return z;
            }
        }

        /// <summary>
        /// Единичный вектор с координатами {1; 0; 0}.
        /// </summary>
        public static UnitVector UnitVectorX
        {
            get
            {
                return new UnitVector(1.0, 0.0, 0.0);
            }
        }

        /// <summary>
        /// Единичный вектор с координатами {0; 1; 0}.
        /// </summary>
        public static UnitVector UnitVectorY
        {
            get
            {
                return new UnitVector(0.0, 1.0, 0.0);
            }
        }

        /// <summary>
        /// Единичный вектор с координатами {0; 0; 1}.
        /// </summary>
        public static UnitVector UnitVectorZ
        {
            get
            {
                return new UnitVector(0.0, 0.0, 1.0);
            }
        }
        #endregion

        #region operators
        /// <summary>
        /// Складывает единичные вектора.
        /// </summary>
        /// 
        /// <param name="unitVector1"> Единичный вектор. Не должен быть null.</param>
        /// <param name="unitVector2"> Единичный вектор. Не должен быть null.</param>
        /// 
        /// <returns>
        /// Единичный вектор, сонаправленный с суммой векторов.
        /// </returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Вызывается, если хотя бы один параметр равен null.
        /// </exception>
        public static UnitVector operator +(UnitVector unitVector1, UnitVector unitVector2)
        {
            if (Equals(unitVector1, null))
            {
                throw new ArgumentNullException("vector1 mustn't be null");
            }
            if (Equals(unitVector2, null))
            {
                throw new ArgumentNullException("vector2 mustn't be null");
            }

            var result = new UnitVector();
            result.x = unitVector1.x + unitVector2.x;
            result.y = unitVector1.y + unitVector2.y;
            result.z = unitVector1.z + unitVector2.z;

            double length = result.Length;
            result.x /= length;
            result.y /= length;
            result.z /= length;

            return result;
        }

        /// <summary>
        /// Вычитает единичные вектора.
        /// </summary>
        /// 
        /// <param name="unitVector1"> Уменьшаемый единичный вектор. Не должен быть null.</param>
        /// <param name="unitVector2"> Вычитаемый единичный вектор. Не должен быть null.</param>
        /// 
        /// <returns>
        /// Единичный вектор, сонаправленный с разностью векторов.
        /// </returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Вызывается, если хотя бы один параметр равен null.
        /// </exception>
        public static UnitVector operator -(UnitVector unitVector1, UnitVector unitVector2)
        {
            if (Equals(unitVector1, null))
            {
                throw new ArgumentNullException("vector1 mustn't be null");
            }
            if (Equals(unitVector2, null))
            {
                throw new ArgumentNullException("vector2 mustn't be null");
            }

            var result = new UnitVector();
            result.x = unitVector1.x - unitVector2.x;
            result.y = unitVector1.y - unitVector2.y;
            result.z = unitVector1.z - unitVector2.z;

            double length = result.Length;
            result.x /= length;
            result.y /= length;
            result.z /= length;

            return result;
        }

        /// <summary>
        /// Противоположный вектор.
        /// </summary>
        /// 
        /// <param name="unitVector"> Вектор. Не должен быть null.</param>
        /// 
        /// <returns>
        /// Противоположный вектор.
        /// </returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Вызывается, параметр равен null.
        /// </exception>
        public static UnitVector operator -(UnitVector unitVector)
        {
            if (Equals(unitVector, null))
            {
                throw new ArgumentNullException("vector mustn't be null");
            }

            var result = new UnitVector();
            result.x = -unitVector.x;
            result.y = -unitVector.y;
            result.z = -unitVector.z;

            return result.UnitVector;
        }

        /// <summary>
        /// Векторно умножает единичные вектора.
        /// </summary>
        /// 
        /// <param name="unitVector1"> Единичный вектор. Не должен быть null.</param>
        /// <param name="unitVector2"> Единичный вектор. Не должен быть null.</param>
        /// 
        /// <returns>
        /// Единичный вектор, сонаправленный с векторным произведением векторов.
        /// </returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Вызывается, если хотя бы один параметр равен null.
        /// </exception>
        public static UnitVector operator *(UnitVector unitVector1, UnitVector unitVector2)
        {
            if (Equals(unitVector1, null))
            {
                throw new ArgumentNullException("vector1 mustn't be null");
            }
            if (Equals(unitVector2, null))
            {
                throw new ArgumentNullException("vector2 mustn't be null");
            }

            var result = new UnitVector();
            result.x = unitVector1.y * unitVector2.z - unitVector1.z * unitVector2.y;
            result.y = unitVector1.z * unitVector2.x - unitVector1.x * unitVector2.z;
            result.z = unitVector1.x * unitVector2.y - unitVector1.y * unitVector2.x;

            double length = result.Length;
            result.x /= length;
            result.y /= length;
            result.z /= length;

            return result;
        }

        /// <summary>
        /// Соответствующее поле:<br/>
        /// 0 — x<br/>
        /// 1 — y<br/>
        /// 2 — z<br/>
        /// </summary>
        /// 
        /// <param name="i">
        /// Индекс.<br/>
        /// Соответствие полям:<br/>
        /// 0 — x<br/>
        /// 1 — y<br/>
        /// 2 — z<br/>
        /// </param>
        /// 
        /// <returns>
        /// Соответствующее поле:<br/>
        /// 0 — x<br/>
        /// 1 — y<br/>
        /// 2 — z<br/>
        /// </returns>
        /// 
        /// <exception cref="ArgumentOutOfRangeException">
        /// Вызывается, если индекс не равен 0, 1 или 2.
        /// </exception>
        new public double this[int i]
        {
            get
            {
                switch (i)
                {
                    case 0: return x;
                    case 1: return y;
                    case 2: return z;
                    default: throw new ArgumentOutOfRangeException("Index is out of range");
                }
            }
        }
        #endregion
    }
}
