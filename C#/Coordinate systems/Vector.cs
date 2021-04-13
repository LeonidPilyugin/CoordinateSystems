using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

namespace CoordinateSystems
{
    // класс представляет вектор
    public class Vector
    {
        protected double x;
        protected double y;
        protected double z;



        public Vector()
        {
            x = y = z = Sqrt(1.0 / 3.0);
        }

        public Vector(double value)
        {
            x = y = z = value;
        }

        public Vector(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector(double theta, double phi)
        {
            this.x = this.y = this.z = 1.0;
            this.Length = 1.0;
            this.Theta = theta;
            this.Phi = phi;
        }

        public Vector(Vector vector)
        {
            this.x = vector.x;
            this.y = vector.y;
            this.z = vector.z;
        }

        public Vector(UnitVector unitVector)
        {
            this.x = unitVector.x;
            this.y = unitVector.y;
            this.z = unitVector.z;
        }



        public double X 
        {
            get { return x; }
            set { x = value; }
        }

        public double Y
        {
            get { return y; }
            set { y = value; }
        }

        public double Z
        {
            get { return z; }
            set { z = value; }
        }

        public double Length
        {
            get { return Sqrt(x * x + y * y + z * z); }
            set
            {
                if (value <= 0)
                    throw new Exception("Value must be > 0");
                double theta = Theta;
                double phi = Phi;
                x = value * Sin(theta) * Cos(phi);
                y = value * Sin(theta) * Sin(phi);
                z = value * Cos(theta);
            }
        }

        public double Theta
        {
            get { return Acos(z / Length); }
            set
            {
                double length = Length;
                double phi = Phi;
                x = length * Sin(value) * Cos(phi);
                y = length * Sin(value) * Sin(phi);
                z = length * Cos(value);
            }
        }

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
                        // Return PI to differ this case
                        return -PI;
                    }
                }
            }
            set
            {
                double length = Length;
                double theta = Theta;
                x = length * Sin(theta) * Cos(value);
                y = length * Sin(theta) * Sin(value);
                z = length * Cos(theta);
            }
        }



        public UnitVector UnitVector
        {
            get { return new UnitVector(x, y, z); }
            set
            {
                this.Phi = value.Phi;
                this.Theta = value.Theta;
            }
        }

        public Vector XYprojection
        {
            get
            {
                return new Vector(x, y, 0.0);
            }
        }

        public Vector XZprojection
        {
            get
            {
                return new Vector(x, 0.0, z);
            }
        }

        public Vector YZprojection
        {
            get
            {
                return new Vector(0.0, y, z);
            }
        }



        public void TurnX(double angle)
        {
            Vector result = Matrix.GetRx(angle) * this;
            this.X = result.X;
            this.Y = result.Y;
            this.Z = result.Z;
        }

        public void TurnY(double angle)
        {
            Vector result = Matrix.GetRy(angle) * this;
            this.X = result.X;
            this.Y = result.Y;
            this.Z = result.Z;
        }

        public void TurnZ(double angle)
        {
            Vector result = Matrix.GetRz(angle) * this;
            this.X = result.X;
            this.Y = result.Y;
            this.Z = result.Z;
        }

        public void TurnAxis(Vector axis, double angle)
        {
            Vector result = Matrix.GetAxisRotationMatrix(axis, angle) * this;
            this.X = result.X;
            this.Y = result.Y;
            this.Z = result.Z;
        }

        public void TurnEuler(double precession, double nutation, double rotation)
        {
            TurnZ(precession);
            TurnX(nutation);
            TurnZ(rotation);
        }



        public static Vector operator +(Vector vector1, Vector vector2)
        {
            Vector result = new Vector();
            result.x = vector1.x + vector2.x;
            result.y = vector1.y + vector2.y;
            result.z = vector1.z + vector2.z;
            return result;
        }

        public static Vector operator -(Vector vector1, Vector vector2)
        {
            Vector result = new Vector();
            result.x = vector1.x - vector2.x;
            result.y = vector1.y - vector2.y;
            result.z = vector1.z - vector2.z;
            return result;
        }

        public static Vector operator -(Vector vector)
        {
            Vector result = new Vector();
            result.x = -vector.x;
            result.y = -vector.y;
            result.z = -vector.z;
            return result;
        }

        public static Vector operator *(Vector vector1, Vector vector2)
        {
            Vector result = new Vector();
            result.x = vector1.y * vector2.z - vector1.z * vector2.y;
            result.y = vector1.z * vector2.x - vector1.x * vector2.z;
            result.z = vector1.x * vector2.y - vector1.y * vector2.x;
            return result;
        }

        public static Vector operator *(Vector vector, double value)
        {
            Vector result = new Vector(vector);
            result.x *= value;
            result.y *= value;
            result.z *= value;
            return result;
        }

        public static Vector operator /(Vector vector, double value)
        {
            Vector result = new Vector(vector);
            result.x /= value;
            result.y /= value;
            result.z /= value;
            return result;
        }

        public double this[int i]
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
            set
            {
                switch (i)
                {
                    case 0: x = value; break;
                    case 1: y = value; break;
                    case 2: z = value; break;
                    default: throw new Exception("Index is out of range");
                }
            }
        }

        public static bool operator ==(Vector vector1, Vector vector2)
        {
            if(Equals(vector1, null) || Equals(vector2, null))
            {
                return Equals(vector1, vector2);
            }

            return vector1.X == vector2.X && vector1.Y == vector2.Y && vector1.Z == vector2.Z;
        }

        public static bool operator !=(Vector vector1, Vector vector2)
        {
            return !(vector1 == vector2);
        }



        public static double MultiplyScalar(Vector vector1, Vector vector2)
        {
            return vector1.x * vector2.x + vector1.y * vector2.y + vector1.z * vector2.z;
        }

        public static double GetAngle(Vector vector1, Vector vector2)
        {
            return Acos(MultiplyScalar(vector1, vector2) / vector1.Length / vector2.Length);
        }
    }
}
