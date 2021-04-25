using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoordinateSystems
{
    // класс представляет единичный вектор
    public class UnitVector : Vector
    {
        #region constructors
        public UnitVector() : base()
        {

        }

        public UnitVector(double x, double y, double z) : base(x, y, z)
        {
            base.Length = 1.0;
        }

        public UnitVector(double theta, double phi) : base(theta, phi)
        {

        }

        public UnitVector(UnitVector unitVector) : base(unitVector.Theta, unitVector.Phi)
        {

        }

        public UnitVector(Vector vector) : base(vector.Theta, vector.Phi)
        {

        }
        #endregion

        #region properties
        new public double Length
        {
            get
            {
                return 1.0;
            }
        }

        new public double X
        {
            get
            {
                return x;
            }
        }

        new public double Y
        {
            get
            {
                return y;
            }
        }

        new public double Z
        {
            get
            {
                return z;
            }
        }

        public static UnitVector UnitVectorX
        {
            get
            {
                return new UnitVector(1.0, 0.0, 0.0);
            }
        }

        public static UnitVector UnitVectorY
        {
            get
            {
                return new UnitVector(0.0, 1.0, 0.0);
            }
        }

        public static UnitVector UnitVectorZ
        {
            get
            {
                return new UnitVector(0.0, 0.0, 1.0);
            }
        }
        #endregion

        #region operators
        public static UnitVector operator +(UnitVector unitVector1, UnitVector unitVector2)
        {
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

        public static UnitVector operator -(UnitVector unitVector1, UnitVector unitVector2)
        {
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

        public static UnitVector operator -(UnitVector unitVector)
        {
            var result = new UnitVector();
            result.x = -unitVector.x;
            result.y = -unitVector.y;
            result.z = -unitVector.z;

            return result.UnitVector;
        }

        public static UnitVector operator *(UnitVector unitVector1, UnitVector unitVector2)
        {
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

        new public double this[int i]
        {
            get
            {
                switch (i)
                {
                    case 0: return x;
                    case 1: return y;
                    case 2: return z;
                    default: throw new Exception("Index is out of range");
                }
            }
        }
        #endregion
    }
}
