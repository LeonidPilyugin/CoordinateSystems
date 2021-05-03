using System;
using System.Collections.Generic;
using static System.Math;

// Файл содержит класс Matrix

namespace CoordinateSystems
{
    /// <summary>
    /// Класс Matrix представляет матрицу чисел.
    /// </summary>
    /// 
    /// <remarks>
    /// Поля:<br/>
    /// 1) <see cref="matrix"/><br/>
    /// <br/>
    /// Конструкторы:<br/>
    /// 1) <see cref="Matrix(int, int)"/><br/>
    /// 2) <see cref="Matrix(Matrix)"/><br/>
    /// 3) <see cref="Matrix(double[,])"/><br/>
    /// 4) <see cref="Matrix(int)"/><br/>
    /// 5) <see cref="Matrix(Basis)"/><br/>
    /// 6) <see cref="Matrix()"/><br/>
    /// <br/>
    /// Свойства:<br/>
    /// 1) <see cref="M"/><br/>
    /// 2) <see cref="N"/><br/>
    /// 3) <see cref="Determinant"/><br/>
    /// 4) <see cref="Transposed"/><br/>
    /// 5) <see cref="Inversed"/><br/>
    /// <br/>
    /// Функции:<br/>
    /// 1) <see cref="Transpose()"/><br/>
    /// 2) <see cref="Inverse()"/><br/>
    /// 3) <see cref="Print()"/><br/>
    /// 4) <see cref="Copy(Matrix)"/><br/>
    /// 5) <see cref="GetRx(double)"/><br/>
    /// 6) <see cref="GetRy(double)"/><br/>
    /// 7) <see cref="GetRz(double)"/><br/>
    /// 8) <see cref="GetEulerMatrix(double, double, double)"/><br/>
    /// 9) <see cref="GetAxisRotationMatrix(Vector, double)"/><br/>
    /// <br/>
    /// Операторы:<br/>
    /// 1) <see cref="this[int, int]"/><br/>
    /// 2) <see cref="operator *(double, Matrix)"/><br/>
    /// 3) <see cref="operator *(Matrix, double)"/><br/>
    /// 4) <see cref="operator *(Matrix, Matrix)"/><br/>
    /// 5) <see cref="operator *(Matrix, UnitVector)"/><br/>
    /// 6) <see cref="operator *(Matrix, Vector)"/><br/>
    /// 7) <see cref="operator +(Matrix, Matrix)"/><br/>
    /// 8) <see cref="operator -(Matrix, Matrix)"/><br/>
    /// 9) <see cref="operator /(Matrix, double)"/><br/>
    /// 10) <see cref="operator ^(Matrix, uint)"/><br/>
    /// </remarks>
    public class Matrix
    {
        #region data
        /// <summary>
        /// Матрица.
        /// </summary>
        protected List<List<double>> matrix;
        #endregion

        #region constructors
        /// <summary>
        /// Матрица m * n
        /// </summary>
        /// 
        /// <param name="m"> Число строк. Должно быть положительно.</param>
        /// <param name="n"> Число столбцов. Должно быть положительно.</param>
        public Matrix(int m, int n)
        {
            matrix = new List<List<double>>();

            for (int i = 0; i < m; i++)
            {
                matrix.Add(new List<double>());
                for (int j = 0; j < n; j++)
                {
                    matrix[i].Add(0.0);
                }
            }
        }

        /// <summary>
        /// Конструктор копирования.
        /// </summary>
        /// 
        /// <param name="matrix"> Копируемая матрица. Не должна быть null.</param>
        public Matrix(Matrix matrix)
        {
            Copy(matrix);
        }

        /// <summary>
        /// Копирование матрицы из массива.
        /// </summary>
        /// 
        /// <param name="matrix"> Копируемый массив. Не должна быть null.</param>
        public Matrix(double[,] matrix)
        {
            this.matrix = new List<List<double>>();

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                this.matrix.Add(new List<double>());
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    this.matrix[i].Add(0.0);
                }
            }
        }

        /// <summary>
        /// Единичная матрица m * m
        /// </summary>
        /// 
        /// <param name="m"> Число строк. должно быть положительно.</param>
        public Matrix(int m) : this(m, m)
        {
            for(int i = 0; i < m; i++)
            {
                matrix[i][i] = 1.0;
            }
        }

        /// <summary>
        /// Копирование матрицы из базиса путем совмещения векторов: matrix = (i, j, k)
        /// </summary>
        /// 
        /// <param name="basis"> Копируемый базис.</param>
        public Matrix(Basis basis)
        {
            this.matrix = new List<List<double>>(3);

            for (int i = 0; i < 3; i++)
            {
                matrix.Add(new List<double>(3));
                for(int j = 0; j < 3; j++)
                {
                    matrix[i].Add(basis[i][j]);
                }
            }
        }

        /// <summary>
        /// Единичная матрица 3 * 3
        /// </summary>
        public Matrix() : this(3)
        {

        }
        #endregion

        #region properties
        /// <summary>
        /// Число строк
        /// </summary>
        public int M
        {
            get
            {
                return matrix.Count;
            }
        }

        /// <summary>
        /// Число столбцов
        /// </summary>
        public int N
        {
            get
            {
                return matrix[0].Count;
            }
        }

        /// <summary>
        /// Определитель матрицы. 
        /// </summary>
        /// 
        /// <exception cref="Exception">
        /// Вызывается, если матрица не квадратная.
        /// </exception>
        public double Determinant
        {
            get
            {
                if (M != N)
                {
                    throw new Exception("Can't get determinant");
                }

                int l;
                double result;
                double sum11 = 1, sum12 = 0, sum21 = 1, sum22 = 0;
                int size = matrix.Count;

                for (int i = 0; i < size; i++)
                {
                    sum11 = 1; l = 2 * size - 1 - i; sum21 = 1;
                    for (int j = 0; j < size; j++)
                    {
                        sum21 *= matrix[j][l % size];
                        l--;
                        sum11 *= matrix[j][(j + i) % (size)];
                    }
                    sum22 += sum21;
                    sum12 += sum11;
                }
                result = sum12 - sum22;

                return result;
            }
        }

        /// <summary>
        /// Транспонированная матрица
        /// </summary>
        public Matrix Transposed
        {
            get
            {
                Matrix result = new Matrix(N, M);
                for (int i = 0; i < result.M; i++)
                {
                    for (int j = 0; j < result.N; j++)
                    {
                        result.matrix[i][j] = this.matrix[j][i];
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Инвертированная матрица
        /// </summary>
        /// 
        /// <exception cref="Exception">
        /// Вызывается, если определитель равен 0 или матрица не квадратная.
        /// </exception>
        public Matrix Inversed
        {
            get
            {
                if (Determinant == 0.0)
                {
                    throw new Exception("can't inverse matrix");
                }

                int size = M;
                Matrix result = new Matrix(M);
                for (int i = 0; i < size; i++)
                    result[i, i] = 1;

                var bigMatrix = new Matrix(size, size * 2);
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        bigMatrix[i, j] = matrix[i][j];
                        bigMatrix[i, j + size] = result[i, j];
                    }
                }

                double K;

                for (int k = 0; k < size; k++)
                {
                    for (int i = 0; i < 2 * size; i++)
                    {
                        bigMatrix[k, i] = bigMatrix[k, i] / matrix[k][k];
                    }

                    for (int i = k + 1; i < size; i++)
                    {
                        K = bigMatrix[i, k] / bigMatrix[k, k];
                        for (int j = 0; j < 2 * size; j++)
                        {
                            bigMatrix[i, j] = bigMatrix[i, j] - bigMatrix[k, j] * K;
                        }
                    }

                    for (int i = 0; i < size; i++)
                    {
                        for (int j = 0; j < size; j++)
                        {
                            matrix[i][j] = bigMatrix[i, j];
                        }
                    }
                }



                for (int k = size - 1; k > -1; k--)
                {
                    for (int i = 2 * size - 1; i > -1; i--)
                    {
                        bigMatrix[k, i] = bigMatrix[k, i] / matrix[k][k];
                    }

                    for (int i = k - 1; i > -1; i--)
                    {
                        K = bigMatrix[i, k] / bigMatrix[k, k];
                        for (int j = 2 * size - 1; j > -1; j--)
                        {
                            bigMatrix[i, j] = bigMatrix[i, j] - bigMatrix[k, j] * K;
                        }
                    }
                }



                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        result[i, j] = bigMatrix[i, j + size];
                    }
                }

                return result;
            }
        }
        #endregion

        #region functions
        /// <summary>
        /// Транспонирует матрицу.
        /// </summary>
        public void Transpose()
        {
            Copy(Transposed);
        }

        /// <summary>
        /// Копирует матрицу.
        /// </summary>
        /// 
        /// <param name="matrix"> Копируемая матрица</param>
        /// 
        /// <returns>
        /// this
        /// </returns>
        protected Matrix Copy(Matrix matrix)
        {
            this.matrix = new List<List<double>>(matrix.matrix);

            for (int i = 0; i < this.matrix.Count; i++)
            {
                this.matrix[i] = new List<double>(matrix.matrix[i]);
            }

            return this;
        }

        /// <summary>
        /// Печатает матрицу в консоль.
        /// </summary>
        public void Print()
        {
            for (int i = 0; i < M; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    Console.Write(matrix[i][j] + " ");
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Инвертирует матрицу.
        /// </summary>
        /// 
        /// <exception cref="Exception">
        /// Вызывается, если определитель равен 0 или матрица не квадратная.
        /// </exception>
        public void Inverse()
        {
            Copy(Inversed);
        }

        /// <summary>
        /// Матрица поворота вокруг оси X на angle радиан.
        /// </summary>
        /// 
        /// <param name="angle"> Угол поворота. Измеряется в радианах.</param>
        /// 
        /// <returns>
        /// Матрица поворота вокруг оси X на angle радиан.
        /// </returns>
        public static Matrix GetRx(double angle)
        {
            Matrix result = new Matrix();

            result[0, 0] = 1.0;
            result[0, 1] = 0.0;
            result[0, 2] = 0.0;
            result[1, 0] = 0.0;
            result[1, 1] = Cos(angle);
            result[1, 2] = Sin(angle);
            result[2, 0] = 0.0;
            result[2, 1] = -Sin(angle);
            result[2, 2] = Cos(angle);

            return result;
        }

        /// <summary>
        /// Матрица поворота вокруг оси Y на angle радиан.
        /// </summary>
        /// 
        /// <param name="angle"> Угол поворота. Измеряется в радианах.</param>
        /// 
        /// <returns>
        /// Матрица поворота вокруг оси Y на angle радиан.
        /// </returns>
        public static Matrix GetRy(double angle)
        {
            Matrix result = new Matrix();

            result[0, 0] = Cos(angle);
            result[0, 1] = 0.0;
            result[0, 2] = -Sin(angle);
            result[1, 0] = 0.0;
            result[1, 1] = 1.0;
            result[1, 2] = 0.0;
            result[2, 0] = Sin(angle);
            result[2, 1] = 0.0;
            result[2, 2] = Cos(angle);

            return result;
        }

        /// <summary>
        /// Матрица поворота вокруг оси Z на angle радиан.
        /// </summary>
        /// 
        /// <param name="angle"> Угол поворота. Измеряется в радианах.</param>
        /// 
        /// <returns>
        /// Матрица поворота вокруг оси Z на angle радиан.
        /// </returns>
        public static Matrix GetRz(double angle)
        {
            Matrix result = new Matrix();

            result[0, 0] = Cos(angle);
            result[0, 1] = Sin(angle);
            result[0, 2] = 0.0;
            result[1, 0] = -Sin(angle);
            result[1, 1] = Cos(angle);
            result[1, 2] = 0.0;
            result[2, 0] = 0.0;
            result[2, 1] = 0.0;
            result[2, 2] = 1.0;

            return result;
        }

        /// <summary>
        /// Матрица поворота по углам Эйлера.
        /// </summary>
        /// 
        /// <param name="precession"> Прецессия. Измеряется в радианах.</param>
        /// <param name="nutation"> Нутация. Измеряется в радианах.</param>
        /// <param name="rotation"> Поворот. Измеряется в радианах.</param>
        /// 
        /// <returns>
        /// Матрица поворота по углам Эйлера.
        /// </returns>
        public static Matrix GetEulerMatrix(double precession, double nutation, double rotation)
        {
            return GetRz(rotation) * GetRx(nutation) * GetRz(precession);
        }

        /// <summary>
        /// Матрица поворота вокруг оси axis на angle радиан.
        /// </summary>
        /// 
        /// <param name="angle"> Угол поворота. Измеряется в радианах.</param>
        /// <param name="axis"> Ось вращения. Не должна быть null.</param>
        /// 
        /// <returns>
        /// Матрица поворота вокруг оси axis на angle радиан.
        /// </returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Вызывается, если axis = null.
        /// </exception>
        public static Matrix GetAxisRotationMatrix(Vector axis, double angle)
        {
            if(axis == null)
            {
                throw new ArgumentNullException("axis mustn't be null");
            }

            axis /= axis.Length;

            Matrix result = new Matrix();

            double c = Cos(angle);
            double s = Sin(angle);

            result[0, 0] = c + (1 - c) * axis.X * axis.X;
            result[0, 1] = (1 - c) * axis.X * axis.Y - s * axis.Z;
            result[0, 2] = (1 - c) * axis.X * axis.Z + s * axis.Y;
            result[1, 0] = (1 - c) * axis.X * axis.Y + s * axis.Z;
            result[1, 1] = c + (1 - c) * axis.Y * axis.Y;
            result[1, 2] = (1 - c) * axis.Z * axis.Y - s * axis.X;
            result[2, 0] = (1 - c) * axis.Z * axis.X - s * axis.Y;
            result[2, 1] = (1 - c) * axis.Z * axis.Y + s * axis.X;
            result[2, 2] = c + (1 - c) * axis.Z * axis.Z;

            return result;
        }
        #endregion

        #region operators
        /// <summary>
        /// Элемент матрицы (i, j).
        /// </summary>
        /// 
        /// <param name="i"> Строка. Должна быть положительной.</param>
        /// <param name="j"> Столбец. Должен быть положительным.</param>
        /// 
        /// <returns>
        /// Элемент матрицы (i, j).
        /// </returns>
        /// 
        /// <exception cref="ArgumentOutOfRangeException"> </exception>
        public double this[int i, int j]
        {
            get
            {
                return matrix[i][j];
            }

            set
            {
                matrix[i][j] = value;
            }
        }

        /// <summary>
        /// Складывает матрицы. Они должны быть одного размера.
        /// </summary>
        /// 
        /// <param name="matrix1"> Матрица. Не должна быть null.</param>
        /// <param name="matrix2"> Матрица. Не должна быть null.</param>
        /// 
        /// <returns>
        /// Сумма матриц.
        /// </returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Вызывается, если хотя бы одна из матриц равна null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Вызывается, если матрицы разного размера.
        /// </exception>
        public static Matrix operator +(Matrix matrix1, Matrix matrix2)
        {
            if(matrix1 == null)
            {
                throw new ArgumentNullException("matrix1 mustn't be null");
            }
            if (matrix2 == null)
            {
                throw new ArgumentNullException("matrix2 mustn't be null");
            }
            if (matrix1.M != matrix2.M || matrix1.N != matrix2.N)
            {
                throw new ArgumentException("Matrices must be the same size");
            }

            var result = new Matrix(matrix1.M, matrix1.N);

            for(int i = 0; i < matrix1.M; i++)
            {
                for (int j = 0; j < matrix1.N; j++)
                {
                    result[i, j] = matrix1[i, j] + matrix2[i, j];
                }
            }

            return result;
        }

        /// <summary>
        /// Вычитает матрицы. Они должны быть одного размера.
        /// </summary>
        /// 
        /// <param name="matrix1"> Уменьшаемая матрица. Не должна быть null.</param>
        /// <param name="matrix2"> Вычитаемая матрица. Не должна быть null.</param>
        /// 
        /// <returns>
        /// Разность матриц.
        /// </returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Вызывается, если хотя бы одна из матриц равна null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Вызывается, если матрицы разного размера.
        /// </exception>
        public static Matrix operator -(Matrix matrix1, Matrix matrix2)
        {
            if (matrix1 == null)
            {
                throw new ArgumentNullException("matrix1 mustn't be null");
            }
            if (matrix2 == null)
            {
                throw new ArgumentNullException("matrix2 mustn't be null");
            }
            if (matrix1.M != matrix2.M || matrix1.N != matrix2.N)
            {
                throw new ArgumentException("Matrices must be the same size");
            }

            var result = new Matrix(matrix1.M, matrix1.N);

            for (int i = 0; i < matrix1.M; i++)
            {
                for (int j = 0; j < matrix1.N; j++)
                {
                    result[i, j] = matrix1[i, j] - matrix2[i, j];
                }
            }

            return result;
        }

        /// <summary>
        /// Умножает матрицу на число.
        /// </summary>
        /// 
        /// <param name="matrix"> Матрица. Не должна быть null.</param>
        /// <param name="number"> Число.</param>
        /// 
        /// <returns>
        /// Произведение матрицы на число.
        /// </returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Вызывается, если матрица равна null.
        /// </exception>
        public static Matrix operator *(Matrix matrix, double number)
        {
            if (matrix == null)
            {
                throw new ArgumentNullException("matrix mustn't be null");
            }

            var result = new Matrix(matrix);

            for (int i = 0; i < result.M; i++)
            {
                for (int j = 0; j < result.N; j++)
                {
                    result[i, j] *= number;
                }
            }

            return result;
        }

        /// <summary>
        /// Умножает матрицу на число.
        /// </summary>
        /// 
        /// <param name="matrix"> Матрица. Не должна быть null.</param>
        /// <param name="number"> Число.</param>
        /// 
        /// <returns>
        /// Произведение матрицы на число.
        /// </returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Вызывается, если матрица равна null.
        /// </exception>
        public static Matrix operator *(double number, Matrix matrix)
        {
            return matrix * number;
        }

        /// <summary>
        /// Делит матрицу на число.
        /// </summary>
        /// 
        /// <param name="matrix"> Матрица. Не должна быть null.</param>
        /// <param name="number"> Число.</param>
        /// 
        /// <returns>
        /// Матрица, деленная на число.
        /// </returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Вызывается, если матрица равна null.
        /// </exception>
        public static Matrix operator /(Matrix matrix, double number)
        {
            if (matrix == null)
            {
                throw new ArgumentNullException("matrix mustn't be null");
            }

            var result = new Matrix(matrix);

            for (int i = 0; i < result.M; i++)
            {
                for (int j = 0; j < result.N; j++)
                {
                    result[i, j] /= number;
                }
            }

            return result;
        }

        /// <summary>
        /// Умножает матрицы. Число столбцов первой матрицы должно быть равно числу строк второй.
        /// </summary>
        /// 
        /// <param name="matrix1"> Матрица. Не должна быть null.</param>
        /// <param name="matrix2"> Матрица. Не должна быть null.</param>
        /// 
        /// <returns>
        /// Произведение матриц.
        /// </returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Вызывается, если хотя бы одна из матриц равна null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Вызывается, если число столбцов первой матрицы не равно числу строк второй.
        /// </exception>
        public static Matrix operator *(Matrix matrix1, Matrix matrix2)
        {
            if (matrix1 == null)
            {
                throw new ArgumentNullException("matrix1 mustn't be null");
            }
            if (matrix2 == null)
            {
                throw new ArgumentNullException("matrix2 mustn't be null");
            }
            if (matrix1.N != matrix2.M)
            {
                throw new ArgumentException("Matrices aren't compatible");
            }

            var result = new Matrix(matrix1.M, matrix2.N);
            double sum;

            for (int i = 0; i < result.M; i++)
            {
                for (int j = 0; j < result.N; j++)
                {
                    sum = 0.0;
                    for (int r = 0; r < matrix1.N; r++)
                        sum += matrix1[i, r] * matrix2[r, j];
                    result[i, j] = sum;
                }
            }

            return result;
        }

        /// <summary>
        /// Умножает матрицу 3 * 3 на вектор.
        /// </summary>
        /// 
        /// <param name="matrix"> Матрица 3 * 3. Не должна быть null.</param>
        /// <param name="vector"> Вектор. Не должен быть null.</param>
        /// 
        /// <returns>
        /// Произведение матрицы на вектор.
        /// </returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Вызывается, если вектор или матрица равна null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Вызывается, если матрица не 3 * 3.
        /// </exception>
        public static Vector operator*(Matrix matrix, Vector vector)
        {
            if (matrix == null)
            {
                throw new ArgumentNullException("matrix mustn't be null");
            }
            if (vector == null)
            {
                throw new ArgumentNullException("vector mustn't be null");
            }
            if (matrix.N != 3 || matrix.M != 3)
            {
                throw new ArgumentException("Matrices aren't compatible");
            }

            var result = new Vector();
            result.X = matrix[0, 0] * vector.X + matrix[0, 1] * vector.Y + matrix[0, 2] * vector.Z;
            result.Y = matrix[1, 0] * vector.X + matrix[1, 1] * vector.Y + matrix[1, 2] * vector.Z;
            result.Z = matrix[2, 0] * vector.X + matrix[2, 1] * vector.Y + matrix[2, 2] * vector.Z;

            return result;
        }

        /// <summary>
        /// Умножает матрицу 3 * 3 на единичный вектор.
        /// </summary>
        /// 
        /// <param name="matrix"> Матрица 3 * 3. Не должна быть null.</param>
        /// <param name="vector"> Единичный ектор. Не должен быть null.</param>
        /// 
        /// <returns>
        /// Произведение матрицы на единичный вектор.
        /// </returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Вызывается, если вектор или матрица равна null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Вызывается, если матрица не 3 * 3.
        /// </exception>
        public static UnitVector operator *(Matrix matrix, UnitVector vector)
        {
            if (matrix == null)
            {
                throw new ArgumentNullException("matrix mustn't be null");
            }
            if (vector == null)
            {
                throw new ArgumentNullException("vector mustn't be null");
            }
            if (matrix.N != 3 || matrix.M != 3)
            {
                throw new ArgumentException("Matrices aren't compatible");
            }

            double x = matrix[0, 0] * vector.X + matrix[0, 1] * vector.Y + matrix[0, 2] * vector.Z;
            double y = matrix[1, 0] * vector.X + matrix[1, 1] * vector.Y + matrix[1, 2] * vector.Z;
            double z = matrix[2, 0] * vector.X + matrix[2, 1] * vector.Y + matrix[2, 2] * vector.Z;

            return new UnitVector(x, y, z);
        }

        /// <summary>
        /// Возводит квадратную матрицу в степень.
        /// </summary>
        /// 
        /// <param name="matrix"> Квадратная матрица. Не должна быть null.</param>
        /// <param name="number"> Показатель степени. Должен быть положителен.</param>
        /// 
        /// <returns>
        /// Матрица, возведенная в степень number.
        /// </returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Вызывается, если матрица равна null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Вызывается, если матрица не квадратная.
        /// </exception>
        public static Matrix operator ^(Matrix matrix, int number)
        {
            if(matrix == null)
            {
                throw new ArgumentNullException("matrix mustn't be null");
            }
            if(matrix.M != matrix.N)
            {
                throw new ArgumentException("Matrix isn't square");
            }

            Matrix result = null;
            if (number == 0)
            {
                return new Matrix(matrix.M);
            }
            else if(number < 0)
            {
                result = matrix.Inversed;
                number = -number;
            }
            else
            {
                result = new Matrix(matrix);
            }

            for(uint i = 1; i < number; i++)
            {
                result *= matrix;
            }

            return result;
        }
        #endregion
    }
}
