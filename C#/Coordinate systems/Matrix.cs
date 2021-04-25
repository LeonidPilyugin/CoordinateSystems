using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

namespace CoordinateSystems
{
    // класс пердставляет матрицу
    public class Matrix
    {
        #region data
        protected List<List<double>> matrix;
        #endregion

        #region constructors
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

        public Matrix(Matrix matrix)
        {
            Copy(matrix);
        }

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

        public Matrix(int m) : this(m, m)
        {
            for(int i = 0; i < m; i++)
            {
                matrix[i][i] = 1.0;
            }
        }

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

        public Matrix() : this(3)
        {

        }
        #endregion

        #region properties
        public int M
        {
            get
            {
                return matrix.Count;
            }
        }

        public int N
        {
            get
            {
                return matrix[0].Count;
            }
        }

        public double Determinant
        {
            get
            {
                if (M != N)
                    throw new Exception("Can't get determinant");
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

        #region operators
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

        public static Matrix operator +(Matrix matrix1, Matrix matrix2)
        {
            if(matrix1.M != matrix2.M || matrix1.N != matrix2.N)
            {
                throw new Exception("Matrices must be the same size");
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

        public static Matrix operator -(Matrix matrix1, Matrix matrix2)
        {
            if (matrix1.M != matrix2.M || matrix1.N != matrix2.N)
            {
                throw new Exception("Matrices must be the same size");
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

        public static Matrix operator *(Matrix matrix, double number)
        {
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

        public static Matrix operator *(double number, Matrix matrix)
        {
            return matrix * number;
        }

        public static Matrix operator /(Matrix matrix, double number)
        {
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

        public static Matrix operator *(Matrix matrix1, Matrix matrix2)
        {
            if (matrix1.N != matrix2.M)
                throw new Exception("Matrices aren't compatible");

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

        public static Vector operator*(Matrix matrix, Vector vector)
        {
            if(matrix.N != 3 || matrix.M != 3)
                throw new Exception("Matrices aren't compatible");

            var result = new Vector();
            result.X = matrix[0, 0] * vector.X + matrix[0, 1] * vector.Y + matrix[0, 2] * vector.Z;
            result.Y = matrix[1, 0] * vector.X + matrix[1, 1] * vector.Y + matrix[1, 2] * vector.Z;
            result.Z = matrix[2, 0] * vector.X + matrix[2, 1] * vector.Y + matrix[2, 2] * vector.Z;

            return result;
        }

        public static UnitVector operator *(Matrix matrix, UnitVector vector)
        {
            if (matrix.N != 3 || matrix.M != 3)
                throw new Exception("Matrices aren't compatible");

            double x = matrix[0, 0] * vector.X + matrix[0, 1] * vector.Y + matrix[0, 2] * vector.Z;
            double y = matrix[1, 0] * vector.X + matrix[1, 1] * vector.Y + matrix[1, 2] * vector.Z;
            double z = matrix[2, 0] * vector.X + matrix[2, 1] * vector.Y + matrix[2, 2] * vector.Z;

            return new UnitVector(x, y, z);
        }

        public static Matrix operator ^(Matrix matrix, uint number)
        {
            Matrix result = new Matrix(matrix);

            for(uint i = 1; i < number; i++)
            {
                result *= matrix;
            }

            return result;
        }
        #endregion

        #region functions
        public void Transpose()
        {
            Copy(Transposed);
        }

        protected Matrix Copy(Matrix matrix)
        {
            this.matrix = new List<List<double>>(matrix.matrix);

            for (int i = 0; i < this.matrix.Count; i++)
            {
                this.matrix[i] = new List<double>(matrix.matrix[i]);
            }

            return this;
        }

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

        public void Inverse()
        {
            Copy(Inversed);
        }

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

        public static Matrix GetEulerMatrix(double precession, double nutation, double rotation)
        {
            return GetRz(rotation) * GetRx(nutation) * GetRz(precession);
        }

        public static Matrix GetAxisRotationMatrix(Vector axis, double angle)
        {
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
    }
}
