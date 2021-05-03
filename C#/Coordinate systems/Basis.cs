using System;

// Файл содержит класс Basis.

namespace CoordinateSystems
{
    /// <summary>
    /// Класс описывает базис.
    /// </summary>
    /// 
    /// <remarks>
    /// Поля:<br/>
    /// 1) <see cref="i"/><br/>
    /// 2) <see cref="j"/><br/>
    /// 3) <see cref="k"/><br/>
    /// <br/>
    /// Конструкторы:<br/>
    /// 1) <see cref="Basis()"/><br/>
    /// 2) <see cref="Basis(UnitVector, UnitVector, UnitVector)"/><br/>
    /// 3) <see cref="Basis(UnitVector, UnitVector)"/><br/>
    /// 4) <see cref="Basis(Matrix)"/><br/>
    /// 5) <see cref="Basis(Basis)"/><br/>
    /// <br/>
    /// Свойства:<br/>
    /// 1) <see cref="I"/><br/>
    /// 2) <see cref="J"/><br/>
    /// 3) <see cref="K"/><br/>
    /// 4) <see cref="Matrix"/><br/>
    /// <br/>
    /// Функции:<br/>
    /// 1) <see cref="TurnX(double)"/><br/>
    /// 2) <see cref="TurnY(double)"/><br/>
    /// 3) <see cref="TurnZ(double)"/><br/>
    /// 4) <see cref="TurnI(double)"/><br/>
    /// 5) <see cref="TurnJ(double)"/><br/>
    /// 6) <see cref="TurnK(double)"/><br/>
    /// 7) <see cref="TurnAxis(Vector, double)"/><br/>
    /// 8) <see cref="TurnEuler(double, double, double)"/><br/>
    /// <br/>
    /// Операторы:<br/>
    /// 1) <see cref="this[int]"/><br/>
    /// 2) <see cref="operator *(Matrix, Basis)"/><br/>
    /// </remarks>
    public class Basis
    {
        #region data
        /// <summary>
        /// Единичный вектор i. Выражается через единичные вектора, направленные вдоль X, Y, Z.
        /// </summary>
        protected UnitVector i;

        /// <summary>
        /// Единичный вектор j. Выражается через единичные вектора, направленные вдоль X, Y, Z.
        /// </summary>
        protected UnitVector j;

        /// <summary>
        /// Единичный вектор k. Выражается через единичные вектора, направленные вдоль X, Y, Z.
        /// </summary>
        protected UnitVector k;
        #endregion

        #region constructors
        /// <summary>
        /// i {1; 0; 0}<br/>
        /// j {0; 1; 0}<br/>
        /// k {0; 0; 1}<br/>
        /// </summary>
        public Basis()
        {
            i = UnitVector.UnitVectorX;
            j = UnitVector.UnitVectorY;
            k = UnitVector.UnitVectorZ;
        }

        /// <summary>
        /// Копирует вектора из параметров. Вектора не должны быть компланарны или null.
        /// </summary>
        /// 
        /// <param name="i"> Единичный вектор i. Не должен быть равен null.</param>
        /// <param name="j"> Единичный вектор j. Не должен быть равен null.</param>
        /// <param name="k"> Единичный вектор k. Не должен быть равен null.</param>
        public Basis(UnitVector i, UnitVector j, UnitVector k)
        {
            this.i = new UnitVector(i);
            this.j = new UnitVector(j);
            this.k = new UnitVector(k);
        }

        /// <summary>
        /// Правая система координат по единичным векторам i и j. Эти вектора не должны быть коллинеарными.
        /// </summary>
        /// 
        /// <param name="i"> Единичный вектор i. Не должен быть равен null.</param>
        /// <param name="j"> Единичный вектор j. Не должен быть равен null.</param>
        public Basis(UnitVector i, UnitVector j) : this(i, j, i * j)
        {

        }

        /// <summary>
        /// Копирует базис из матрицы:<br/>
        /// Ix, Iy, Iz<br/>
        /// Jx, Jy, Jz<br/>
        /// Kx, Ky, Kz<br/>
        /// </summary>
        /// 
        /// <param name="matrix"> Копируемая матрица. Должна быть 3 * 3, не должна быть null.</param>
        public Basis(Matrix matrix)
        {
            for (int i = 0; i < 3; i++)
            {
                this[i] = new UnitVector(matrix[i, 0], matrix[i, 1], matrix[i, 2]);
            }
        }

        /// <summary>
        /// Копирование базиса.
        /// </summary>
        /// 
        /// <param name="basis"> Копируемый базис. Не должен быть null.</param>
        public Basis(Basis basis)
        {
            this.i = new UnitVector(basis.i);
            this.j = new UnitVector(basis.j);
            this.k = new UnitVector(basis.k);
        }
        #endregion

        #region properties
        /// <summary>
        /// Единичный вектор I. Выражается через единичные вектора, направленные вдоль X, Y, Z.
        /// Не должен быть null. Вектора не должны быть компланарны.
        /// Возвращает копию поля.
        /// </summary>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Вызывается при передаче null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Вызывается, если передаваемый вектор компланарен J и K.
        /// </exception>
        public UnitVector I
        {
            get
            {
                return new UnitVector(i);
            }

            set
            {
                if(value == null)
                {
                    throw new ArgumentNullException("I mustn't be null");
                }

                if(Vector.AreCoplanar(value, j, k))
                {
                    throw new ArgumentException("Vectors mustn,t be coplanar");
                }

                i = value;
            }
        }

        /// <summary>
        /// Единичный вектор J. Выражается через единичные вектора, направленные вдоль X, Y, Z.
        /// Не должен быть null. Вектора не должны быть компланарны.
        /// Возвращает копию поля.
        /// </summary>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Вызывается при передаче null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Вызывается, если передаваемый вектор компланарен I и K.
        /// </exception>
        public UnitVector J
        {
            get
            {
                return new UnitVector(j);
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("J mustn't be null");
                }

                if (Vector.AreCoplanar(value, i, k))
                {
                    throw new ArgumentException("Vectors mustn,t be coplanar");
                }

                j = value;
            }
        }

        /// <summary>
        /// Единичный вектор K. Выражается через единичные вектора, направленные вдоль X, Y, Z.
        /// Не должен быть null. Вектора не должны быть компланарны.
        /// Возвращает копию поля.
        /// </summary>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Вызывается при передаче null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Вызывается, если передаваемый вектор компланарен I и K.
        /// </exception>
        public UnitVector K
        {
            get
            {
                return new UnitVector(k);
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("K mustn't be null");
                }

                if (Vector.AreCoplanar(value, j, i))
                {
                    throw new ArgumentException("Vectors mustn,t be coplanar");
                }

                k = value;
            }
        }

        /// <summary>
        /// Матрица:<br/>
        /// Ix, Iy, Iz<br/>
        /// Jx, Jy, Jz<br/>
        /// Kx, Ky, Kz<br/>
        /// </summary>
        public Matrix Matrix
        {
            get
            {
                var result = new Matrix(3);

                for (int m = 0; m < 3; m++)
                {
                    for (int n = 0; n < 3; n++)
                    {
                        result[m, n] = this[m][n];
                    }
                }

                return result;
            }
        }
        #endregion

        #region functions
        #region turn
        /// <summary>
        /// Поворачивает базис вокруг оси X на заданный угол.
        /// </summary>
        /// 
        /// <param name="angle"> Угол поворота. Измеряется в радианах.</param>
        public void TurnX(double angle)
        {
            I.TurnX(angle);
            J.TurnX(angle);
            K.TurnX(angle);
        }

        /// <summary>
        /// Поворачивает базис вокруг оси Y на заданный угол.
        /// </summary>
        /// 
        /// <param name="angle"> Угол поворота. Измеряется в радианах.</param>
        public void TurnY(double angle)
        {
            I.TurnY(angle);
            J.TurnY(angle);
            K.TurnY(angle);
        }

        /// <summary>
        /// Поворачивает базис вокруг оси Z на заданный угол.
        /// </summary>
        /// 
        /// <param name="angle"> Угол поворота. Измеряется в радианах.</param>
        public void TurnZ(double angle)
        {
            I.TurnZ(angle);
            J.TurnZ(angle);
            K.TurnZ(angle);
        }

        /// <summary>
        /// Поворачивает базис вокруг оси I на заданный угол.
        /// </summary>
        /// 
        /// <param name="angle"> Угол поворота. Измеряется в радианах.</param>
        public void TurnI(double angle)
        {
            J.TurnAxis(I, angle);
            K.TurnAxis(I, angle);
        }

        /// <summary>
        /// Поворачивает базис вокруг оси J на заданный угол.
        /// </summary>
        /// 
        /// <param name="angle"> Угол поворота. Измеряется в радианах.</param>
        public void TurnJ(double angle)
        {
            I.TurnAxis(J, angle);
            K.TurnAxis(J, angle);
        }

        /// <summary>
        /// Поворачивает базис вокруг оси K на заданный угол.
        /// </summary>
        /// 
        /// <param name="angle"> Угол поворота. Измеряется в радианах.</param>
        public void TurnK(double angle)
        {
            I.TurnAxis(K, angle);
            J.TurnAxis(K, angle);
        }

        /// <summary>
        /// Поворачивает базис вокруг заданной оси на заданный угол.
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
            I.TurnAxis(axis, angle);
            J.TurnAxis(axis, angle);
            K.TurnAxis(axis, angle);
        }

        /// <summary>
        /// Поворачивает базис при помощи углов Эйлера.
        /// </summary>
        /// 
        /// <param name="precession"> Прецессия. Измеряется в радианах.</param>
        /// <param name="nutation"> Нутация. Измеряется в радианах.</param>
        /// <param name="rotation"> Вращение. Измеряется в радианах.</param>
        public void TurnEuler(double precession, double nutation, double rotation)
        {
            I.TurnEuler(precession, nutation, rotation);
            J.TurnEuler(precession, nutation, rotation);
            K.TurnEuler(precession, nutation, rotation);
        }
        #endregion
        #endregion

        #region operators
        /// <summary>
        /// Соответствующее поле:<br/>
        /// 0 — i<br/>
        /// 1 — j<br/>
        /// 2 — k<br/>
        /// </summary>
        /// 
        /// <param name="m">
        /// Индекс.<br/>
        /// Соответствие полям:<br/>
        /// 0 — i<br/>
        /// 1 — j<br/>
        /// 2 — k<br/>
        /// </param>
        /// 
        /// <returns>
        /// Соответствующее поле:<br/>
        /// 0 — i<br/>
        /// 1 — j<br/>
        /// 2 — k<br/>
        /// </returns>
        /// 
        /// <exception cref="ArgumentOutOfRangeException">
        /// Вызывается, если индекс не равен 0, 1 или 2.
        /// </exception>
        public UnitVector this[int m]
        {
            get
            {
                switch (m)
                {
                    case 0: return i;
                    case 1: return j;
                    case 2: return k;
                    default: throw new ArgumentOutOfRangeException("Index is out of range");
                }
            }

            set
            {
                switch (m)
                {
                    case 0: I = value; break;
                    case 1: J = value; break;
                    case 2: K = value; break;
                    default: throw new ArgumentOutOfRangeException("Index is out of range");
                }
            }
        }

        /// <summary>
        /// Умножение матрицы на базис. Эквивалентно умножению двух матриц 3 * 3.
        /// </summary>
        /// 
        /// <param name="matrix"> Матрица 3 * 3. Не должна быть null.</param>
        /// <param name="basis"> Базис. Не должен быть null.</param>
        /// 
        /// <returns>
        /// Произведение матрицы на базис.
        /// </returns>
        /// 
        /// <exception cref="ArgumentException">
        /// Вызывается, если матрица не 3 * 3.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Вызывается, если матрица или базис равны null.
        /// </exception>
        public static Basis operator *(Matrix matrix, Basis basis)
        {
            if (matrix == null)
            {
                throw new ArgumentNullException("matrix mustn't be null");
            }
            if (basis == null)
            {
                throw new ArgumentNullException("basis mustn't be null");
            }
            if (matrix.M != 3 || matrix.N != 3)
            {
                throw new ArgumentException("matrix must be 3 * 3");
            }

            var basisMatrix = basis.Matrix;

            basisMatrix = matrix * basisMatrix;

            return new Basis(basisMatrix);
        }
        #endregion
    }
}
