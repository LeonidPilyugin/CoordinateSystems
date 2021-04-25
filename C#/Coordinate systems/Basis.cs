using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoordinateSystems
{
    // класс представляет базис (3 единичных вектора)
    public class Basis
    {
        #region constructors
        public Basis()
        {
            I = new UnitVector(1.0, 0.0, 0.0);
            J = new UnitVector(0.0, 1.0, 0.0);
            K = new UnitVector(0.0, 0.0, 1.0);
        }

        public Basis(UnitVector i, UnitVector j, UnitVector k)
        {
            I = i;
            J = j;
            K = k;
        }

        public Basis(Matrix matrix)
        {
            for (int i = 0; i < 3; i++)
            {
                this[i] = new UnitVector(matrix[i, 0], matrix[i, 1], matrix[i, 2]);
            }
        }
        #endregion

        #region operators
        public UnitVector this[int i]
        {
            get
            {
                switch (i)
                {
                    case 0: return I;
                    case 1: return J;
                    case 2: return K;
                    default: throw new Exception("Index is out of range");
                }
            }
            set
            {
                switch (i)
                {
                    case 0: I = value; break;
                    case 1: J = value; break;
                    case 2: K = value; break;
                    default: throw new Exception("Index is out of range");
                }
            }
        }

        public static Basis operator *(Matrix matrix, Basis basis)
        {
            var basisMatrix = new Matrix(basis);

            basisMatrix = matrix * basisMatrix;

            return new Basis(basisMatrix);
        }
        #endregion

        #region properties
        public UnitVector I { get; set; }
        public UnitVector J { get; set; }
        public UnitVector K { get; set; }
        #endregion

        #region functions
        #region turn
        public void TurnX(double angle)
        {
            I.TurnX(angle);
            J.TurnX(angle);
            K.TurnX(angle);
        }

        public void TurnY(double angle)
        {
            I.TurnY(angle);
            J.TurnY(angle);
            K.TurnY(angle);
        }

        public void TurnZ(double angle)
        {
            I.TurnZ(angle);
            J.TurnZ(angle);
            K.TurnZ(angle);
        }

        public void TurnI(double angle)
        {
            J.TurnAxis(I, angle);
            K.TurnAxis(I, angle);
        }

        public void TurnJ(double angle)
        {
            I.TurnAxis(J, angle);
            K.TurnAxis(J, angle);
        }

        public void TurnK(double angle)
        {
            I.TurnAxis(K, angle);
            J.TurnAxis(K, angle);
        }

        public void TurnAxis(Vector axis, double angle)
        {
            I.TurnAxis(axis, angle);
            J.TurnAxis(axis, angle);
            K.TurnAxis(axis, angle);
        }

        public void TurnEuler(double precession, double nutation, double rotation)
        {
            I.TurnEuler(precession, nutation, rotation);
            J.TurnEuler(precession, nutation, rotation);
            K.TurnEuler(precession, nutation, rotation);
        }
        #endregion
        #endregion
    }
}
