using System;
using static System.Math;

// Файл содержит класс Vector

namespace CoordinateSystems
{
    /// <summary>
    /// Класс Vector описывает вектор в пространстве и поддерживает некоторые операции над ним.
    /// <br/>
    /// Конструкторы:<br/>
    /// 1) <see cref="Vector()"/><br/>
    /// 2) <see cref="Vector(double)"/><br/>
    /// 3) <see cref="Vector(double, double, double)"/><br/>
    /// 4) <see cref="Vector(double, double)"/><br/>
    /// 5) <see cref="Vector(Vector)"/><br/>
    /// </summary>
    /// 
    /// <remarks>
    /// Поля:<br/>
    /// 1) <see cref="x"/><br/>
    /// 2) <see cref="y"/><br/>
    /// 3) <see cref="z"/><br/>
    /// <br/>
    /// Свойства:<br/>
    /// 1) <see cref="X"/><br/>
    /// 2) <see cref="Y"/><br/>
    /// 3) <see cref="Z"/><br/>
    /// 4) <see cref="Length"/><br/>
    /// 5) <see cref="Theta"/><br/>
    /// 6) <see cref="Phi"/><br/>
    /// 7) <see cref="UnitVector"/><br/>
    /// 8) <see cref="XYprojection"/><br/>
    /// 9) <see cref="XZprojection"/><br/>
    /// 10) <see cref="YZprojection"/><br/>
    /// <br/>
    /// Методы:<br/>
    /// 1) <see cref="TurnX(double)"/><br/>
    /// 2) <see cref="TurnY(double)"/><br/>
    /// 3) <see cref="TurnZ(double)"/><br/>
    /// 4) <see cref="TurnAxis(Vector, double)"/><br/>
    /// 5) <see cref="TurnEuler(double, double, double)"/><br/>
    /// 6) <see cref="GetAngle(Vector, Vector)"/><br/>
    /// 7) <see cref="MultiplyScalar(Vector, Vector)"/><br/>
    /// 8) <see cref="ToString()"/><br/>
    /// 9) <see cref="AreCoplanar(Vector, Vector, Vector)"/><br/>
    /// <br/>
    /// Операторы:<br/>
    /// 1) <see cref="operator !=(Vector, Vector)"/><br/>
    /// 2) <see cref="operator ==(Vector, Vector)"/><br/>
    /// 3) <see cref="operator *(Vector, double)"/><br/>
    /// 4) <see cref="operator *(Vector, Vector)"/><br/>
    /// 5) <see cref="operator +(Vector, Vector)"/><br/>
    /// 6) <see cref="operator -(Vector)"/><br/>
    /// 7) <see cref="operator -(Vector, Vector)"/><br/>
    /// 8) <see cref="operator /(Vector, double)"/><br/>
    /// 9) <see cref="this[int]"/><br/>
    /// </remarks>
    public class Vector
    {
        #region data
        /// <summary>
        /// Координата x
        /// </summary>
        protected double x;

        /// <summary>
        /// Координата y
        /// </summary>
        protected double y;

        /// <summary>
        /// Координата z
        /// </summary>
        protected double z;
        #endregion

        #region constructors
        /// <summary>
        /// Единичный вектор x = y = z.
        /// </summary>
        public Vector()
        {
            x = y = z = Sqrt(1.0 / 3.0);
        }

        /// <summary>
        /// Вектор x = y = z = value.
        /// </summary>
        /// 
        /// <param name="value"> Значение координат.</param>
        public Vector(double value)
        {
            x = y = z = value;
        }

        /// <summary>
        /// Вектор с координатами, соответствующимиаргументам.
        /// </summary>
        /// 
        /// <param name="x"> Координата x.</param>
        /// <param name="y"> Координата y.</param>
        /// <param name="z"> Координата z.</param>
        public Vector(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /// <summary>
        /// Единичный вектор со сферическими координатами равными аргументам.
        /// </summary>
        /// 
        /// <param name="theta"> Зенитный угол. Измеряется в радианах. 0 &lt;= Theta &lt;= <see cref="Math.PI"/></param>
        /// <param name="phi"> Азимутальный угол. Измеряется в радианах. -<see cref="Math.PI"/> &lt;= Phi &lt;= <see cref="Math.PI"/></param>
        /// 
        /// <exception cref="ArgumentException">
        /// Вызывается, если theta или phi не соответствуют возможным.
        /// </exception>
        public Vector(double theta, double phi)
        {
            this.x = this.y = this.z = 1.0;
            this.Length = 1.0;
            this.Theta = theta;
            this.Phi = phi;
        }

        /// <summary>
        /// Конструктор копирования.
        /// </summary>
        /// 
        /// <param name="vector"> Копируемый вектор. Не должен быть null.</param>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Вызывается при передаче null.
        /// </exception>
        public Vector(Vector vector)
        {
            if(vector == null)
            {
                throw new ArgumentNullException("vector mustn't be null");
            }

            this.x = vector.x;
            this.y = vector.y;
            this.z = vector.z;
        }
        #endregion

        #region properties
        /// <summary>
        /// Координата X.
        /// </summary>
        public double X 
        {
            get
            {
                return x;
            }

            set
            {
                x = value;
            }
        }

        /// <summary>
        /// Координата Y.
        /// </summary>
        public double Y
        {
            get
            {
                return y;
            }

            set
            {
                y = value;
            }
        }

        /// <summary>
        /// Координата Y.
        /// </summary>
        public double Z
        {
            get
            {
                return z;
            }

            set
            {
                z = value;
            }
        }

        /// <summary>
        /// Длина вектора. Должна быть положительной.
        /// </summary>
        /// 
        /// <exception cref="ArgumentOutOfRangeException">
        /// Вызывается, если передается неположительное значение
        /// </exception>
        public double Length
        {
            get
            {
                return Sqrt(x * x + y * y + z * z);
            }

            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("Value must be > 0");
                }

                // Сохранение углов.
                double theta = Theta;
                double phi = Phi;

                // Изменение координат.
                x = value * Sin(theta) * Cos(phi);
                y = value * Sin(theta) * Sin(phi);
                z = value * Cos(theta);
            }
        }

        /// <summary>
        /// Зенитный угол.<br/>
        /// Измеряется в радианах.<br/>
        /// 0 &lt;= Theta &lt;= <see cref="Math.PI"/>
        /// </summary>
        /// 
        /// <exception cref="ArgumentOutOfRangeException">
        /// Вызывается, если передается отрицательное или большее <see cref="Math.PI"/> значение.
        /// </exception>
        public double Theta
        {
            get
            {
                return Acos(z / Length);
            }

            set
            {
                if(value < 0.0 || value > PI)
                {
                    throw new ArgumentOutOfRangeException("Theta must be >= 0 and <= PI");
                }

                // Сохранение неизменяемых параметров.
                double length = Length;
                double phi = Phi;

                // Изменение координат.
                x = length * Sin(value) * Cos(phi);
                y = length * Sin(value) * Sin(phi);
                z = length * Cos(value);
            }
        }

        /// <summary>
        /// Азимутальный угол.<br/>
        /// Измеряется в радианах.<br/>
        /// -PI &lt;= Phi &lt;= <see cref="Math.PI"/>
        /// </summary>
        /// 
        /// <exception cref="ArgumentOutOfRangeException">
        /// Вызывается, если передается значение, меньшее -<see cref="Math.PI"/> или большее <see cref="Math.PI"/>.
        /// </exception>
        public double Phi
        {
            get 
            {
                if(x > 0.0)
                {
                    // x > 0
                    return Atan(y/x);
                }

                else if(x < 0.0)
                {
                    if(y > 0.0)
                    {
                        // x < 0, y > 0
                        return PI + Atan(y / x);
                    }
                    else if(y < 0.0)
                    {
                        // x < 0, y < 0
                        return -PI + Atan(y / x);
                    }
                    else
                    {
                        // x < 0, y == 0
                        return PI;
                    }
                }

                else
                {
                    if(y > 0.0)
                    {
                        // x == 0, y > 0
                        return PI / 2.0;
                    }
                    else if(y < 0.0)
                    {
                        // x == 0, y > 0
                        return -PI / 2.0;
                    }
                    else
                    {
                        // x == 0, y == 0 — any angle
                        // Return -PI to differ this case
                        return -PI;
                    }
                }
            }

            set
            {
                if(value < -PI || value > PI)
                {
                    throw new ArgumentOutOfRangeException("Theta must be >= -PI and <= PI");
                }    

                // Сохранение неизменяемых параметров.
                double length = Length;
                double theta = Theta;

                // Изменение координат.
                x = length * Sin(theta) * Cos(value);
                y = length * Sin(theta) * Sin(value);
                z = length * Cos(theta);
            }
        }

        /// <summary>
        /// Единичный вектор с такими же углами. Не должен быть null.
        /// </summary>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Вызывается при передаче значения null.
        /// </exception>
        public UnitVector UnitVector
        {
            get
            {
                return new UnitVector(x, y, z);
            }

            set
            {
                if(Equals(value, null))
                {
                    throw new ArgumentNullException("UnitVector mustn't be null");
                }

                this.Phi = value.Phi;
                this.Theta = value.Theta;
            }
        }

        /// <summary>
        /// Проекция вектора на ось XY.
        /// </summary>
        public Vector XYprojection
        {
            get
            {
                return new Vector(x, y, 0.0);
            }
        }

        /// <summary>
        /// Проекция вектора на ось XZ.
        /// </summary>
        public Vector XZprojection
        {
            get
            {
                return new Vector(x, 0.0, z);
            }
        }

        /// <summary>
        /// Проекция вектора на ось YZ.
        /// </summary>
        public Vector YZprojection
        {
            get
            {
                return new Vector(0.0, y, z);
            }
        }
        #endregion

        #region methods
        /// <summary>
        /// Поворачивает вектор вокруг оси X на заданный угол.
        /// </summary>
        /// 
        /// <param name="angle"> Угол поворота. Измеряется в радианах.</param>
        public void TurnX(double angle)
        {
            Vector result = Matrix.GetRx(angle) * this;
            this.X = result.X;
            this.Y = result.Y;
            this.Z = result.Z;
        }

        /// <summary>
        /// Поворачивает вектор вокруг оси Y на заданный угол.
        /// </summary>
        /// 
        /// <param name="angle"> Угол поворота. Измеряется в радианах.</param>
        public void TurnY(double angle)
        {
            Vector result = Matrix.GetRy(angle) * this;
            this.X = result.X;
            this.Y = result.Y;
            this.Z = result.Z;
        }

        /// <summary>
        /// Поворачивает вектор вокруг оси Z на заданный угол.
        /// </summary>
        /// 
        /// <param name="angle"> Угол поворота. Измеряется в радианах.</param>
        public void TurnZ(double angle)
        {
            Vector result = Matrix.GetRz(angle) * this;
            this.X = result.X;
            this.Y = result.Y;
            this.Z = result.Z;
        }

        /// <summary>
        /// Поворачивает вектор вокруг заданной оси на заданный угол.
        /// </summary>
        /// 
        /// <param name="angle"> Угол поворота. Измеряется в радианах.</param>
        /// <param name="axis"> Ось вращения. Не должна быть null.</param>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Вызывается при axis = null.
        /// </exception>
        public void TurnAxis(Vector axis, double angle)
        {
            if (Equals(axis, null))
            {
                throw new ArgumentNullException("axis mustn't be null");
            }

            Vector result = Matrix.GetAxisRotationMatrix(axis, angle) * this;
            this.X = result.X;
            this.Y = result.Y;
            this.Z = result.Z;
        }

        /// <summary>
        /// Поворачивает вектор при помощи углов Эйлера.
        /// </summary>
        /// 
        /// <param name="precession"> Прецессия. Измеряется в радианах.</param>
        /// <param name="nutation"> Нутация. Измеряется в радианах.</param>
        /// <param name="rotation"> Вращение. Измеряется в радианах.</param>
        public void TurnEuler(double precession, double nutation, double rotation)
        {
            TurnZ(precession);
            TurnX(nutation);
            TurnZ(rotation);
        }

        /// <summary>
        /// Возвращает скалярное произведение векторов.
        /// </summary>
        /// 
        /// <param name="vector1"> Вектор. Не должен быть null.</param>
        /// <param name="vector2"> Вектор. Не должен быть null.</param>
        /// 
        /// <returns>
        /// Скалярное произведение векторов.
        /// </returns>
        public static double MultiplyScalar(Vector vector1, Vector vector2)
        {
            if(Equals(vector1, null))
            {
                throw new ArgumentNullException("vector1 mustn't be null");
            }
            if (Equals(vector2, null))
            {
                throw new ArgumentNullException("vector2 mustn't be null");
            }

            return vector1.x * vector2.x + vector1.y * vector2.y + vector1.z * vector2.z;
        }

        /// <summary>
        /// Возвращает угол между векторами в радианах.
        /// </summary>
        /// 
        /// <param name="vector1"> Вектор. Не должен быть null.</param>
        /// <param name="vector2"> Вектор. Не должен быть null.</param>
        /// 
        /// <returns>
        /// Угол между векторами в радианах.
        /// </returns>
        public static double GetAngle(Vector vector1, Vector vector2)
        {
            if (Equals(vector1, null))
            {
                throw new ArgumentNullException("vector1 mustn't be null");
            }
            if (Equals(vector2, null))
            {
                throw new ArgumentNullException("vector2 mustn't be null");
            }

            return Acos(MultiplyScalar(vector1, vector2) / vector1.Length / vector2.Length);
        }

        /// <summary>
        /// Преобразует в строку: "[x] [y] [z]".
        /// </summary>
        /// 
        /// <returns>
        /// Строка: "[x] [y] [z]".
        /// </returns>
        public override string ToString()
        {
            return x.ToString() + " " + y.ToString() + " " + z.ToString();
        }

        /// <summary>
        /// Проверяет компланарность векторов.
        /// </summary>
        /// 
        /// <param name="vector1"> Вектор. Не должен быть null.</param>
        /// <param name="vector2"> Вектор. Не должен быть null.</param>
        /// <param name="vector3"> Вектор. Не должен быть null.</param>
        /// 
        /// <returns>
        /// true, если вектора компланарны.<br/>
        /// false, если нет.
        /// </returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Вызывается, если хотя б один из векторов равен null.
        /// </exception>
        public static bool AreCoplanar(Vector vector1, Vector vector2, Vector vector3)
        {
            if(vector1 == null)
            {
                throw new ArgumentNullException("vector1 mustn't be null");
            }
            if (vector2 == null)
            {
                throw new ArgumentNullException("vector2 mustn't be null");
            }
            if (vector3 == null)
            {
                throw new ArgumentNullException("vector3 mustn't be null");
            }

            var matrix = new Matrix(3);
            matrix[0, 0] = vector1.X;
            matrix[0, 1] = vector1.Y;
            matrix[0, 2] = vector1.Z;
            matrix[1, 0] = vector2.X;
            matrix[1, 1] = vector2.Y;
            matrix[1, 2] = vector2.Z;
            matrix[2, 0] = vector3.X;
            matrix[2, 1] = vector3.Y;
            matrix[2, 2] = vector3.Z;

            return matrix.Determinant == 0.0;
        }
        #endregion

        #region operators
        /// <summary>
        /// Складывает вектора.
        /// </summary>
        /// 
        /// <param name="vector1"> Вектор. Не должен быть null.</param>
        /// <param name="vector2"> Вектор. Не должен быть null.</param>
        /// 
        /// <returns>
        /// Сумма векторов.
        /// </returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Вызывается, если хотя бы один параметр равен null.
        /// </exception>
        public static Vector operator +(Vector vector1, Vector vector2)
        {
            if (Equals(vector1, null))
            {
                throw new ArgumentNullException("vector1 mustn't be null");
            }
            if (Equals(vector2, null))
            {
                throw new ArgumentNullException("vector2 mustn't be null");
            }

            Vector result = new Vector();
            result.x = vector1.x + vector2.x;
            result.y = vector1.y + vector2.y;
            result.z = vector1.z + vector2.z;
            return result;
        }

        /// <summary>
        /// Вычитает вектора.
        /// </summary>
        /// 
        /// <param name="vector1"> Уменьшаемый вектор. Не должен быть null.</param>
        /// <param name="vector2"> Вычитаемый вектор. Не должен быть null.</param>
        /// 
        /// <returns>
        /// Разность векторов.
        /// </returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Вызывается, если хотя бы один параметр равен null.
        /// </exception>
        public static Vector operator -(Vector vector1, Vector vector2)
        {
            if (Equals(vector1, null))
            {
                throw new ArgumentNullException("vector1 mustn't be null");
            }
            if (Equals(vector2, null))
            {
                throw new ArgumentNullException("vector2 mustn't be null");
            }

            Vector result = new Vector();
            result.x = vector1.x - vector2.x;
            result.y = vector1.y - vector2.y;
            result.z = vector1.z - vector2.z;
            return result;
        }

        /// <summary>
        /// Противоположный вектор.
        /// </summary>
        /// 
        /// <param name="vector"> Вектор. Не должен быть null.</param>
        /// 
        /// <returns>
        /// Противоположный вектор.
        /// </returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Вызывается, параметр равен null.
        /// </exception>
        public static Vector operator -(Vector vector)
        {
            if (Equals(vector, null))
            {
                throw new ArgumentNullException("vector mustn't be null");
            }

            Vector result = new Vector();
            result.x = -vector.x;
            result.y = -vector.y;
            result.z = -vector.z;
            return result;
        }

        /// <summary>
        /// Векторно умножает вектора.
        /// </summary>
        /// 
        /// <param name="vector1"> Вектор. Не должен быть null.</param>
        /// <param name="vector2"> Вектор. Не должен быть null.</param>
        /// 
        /// <returns>
        /// Векторное произведение векторов.
        /// </returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Вызывается, если хотя бы один параметр равен null.
        /// </exception>
        public static Vector operator *(Vector vector1, Vector vector2)
        {
            if (Equals(vector1, null))
            {
                throw new ArgumentNullException("vector1 mustn't be null");
            }
            if (Equals(vector2, null))
            {
                throw new ArgumentNullException("vector2 mustn't be null");
            }

            Vector result = new Vector();
            result.x = vector1.y * vector2.z - vector1.z * vector2.y;
            result.y = vector1.z * vector2.x - vector1.x * vector2.z;
            result.z = vector1.x * vector2.y - vector1.y * vector2.x;
            return result;
        }

        /// <summary>
        /// Умножает вектор на число.
        /// </summary>
        /// 
        /// <param name="vector"> Вектор. Не должен быть null.</param>
        /// <param name="value"> Число.</param>
        /// 
        /// <returns>
        /// Вектор, умноженный на число.
        /// </returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Вызывается, если вектор равен null.
        /// </exception>
        public static Vector operator *(Vector vector, double value)
        {
            if (Equals(vector, null))
            {
                throw new ArgumentNullException("vector mustn't be null");
            }

            Vector result = new Vector(vector);
            result.x *= value;
            result.y *= value;
            result.z *= value;
            return result;
        }

        /// <summary>
        /// Делит вектор на число.
        /// </summary>
        /// 
        /// <param name="vector"> Вектор. Не должен быть null.</param>
        /// <param name="value"> Число.</param>
        /// 
        /// <returns>
        /// Вектор, деленный на число.
        /// </returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Вызывается, если вектор равен null.
        /// </exception>
        public static Vector operator /(Vector vector, double value)
        {
            if (Equals(vector, null))
            {
                throw new ArgumentNullException("vector mustn't be null");
            }

            Vector result = new Vector(vector);
            result.x /= value;
            result.y /= value;
            result.z /= value;
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
        public double this[int i]
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
            set
            {
                switch (i)
                {
                    case 0: x = value; break;
                    case 1: y = value; break;
                    case 2: z = value; break;
                    default: throw new ArgumentOutOfRangeException("Index is out of range");
                }
            }
        }

        /// <summary>
        /// Проверяет равенство координат векторов. Если хотя бы один равен null, то проверяет равенство ссылок.
        /// </summary>
        /// 
        /// <param name="vector1"> Вектор.</param>
        /// <param name="vector2"> Вектор. </param>
        /// 
        /// <returns>
        /// true, если ветора равны или оба равны null.<br/>
        /// false, если вектора не равны или один равен null, а второй нет.
        /// </returns>
        public static bool operator ==(Vector vector1, Vector vector2)
        {
            if(Equals(vector1, null) || Equals(vector2, null))
            {
                return Equals(vector1, vector2);
            }

            return vector1.X == vector2.X && vector1.Y == vector2.Y && vector1.Z == vector2.Z;
        }

        /// <summary>
        /// Проверяет равенство координат векторов. Если хотя бы один равен null, то проверяет равенство ссылок.
        /// </summary>
        /// 
        /// <param name="vector1"> Вектор.</param>
        /// <param name="vector2"> Вектор. </param>
        /// 
        /// <returns>
        /// false, если ветора равны или оба равны null.<br/>
        /// true, если вектора не равны или один равен null, а второй нет.
        /// </returns>
        public static bool operator !=(Vector vector1, Vector vector2)
        {
            return !(vector1 == vector2);
        }
        #endregion
    }
}
