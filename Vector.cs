using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

namespace CoordinateSystems
{
    public class Vector
    {
        public Vector()
        {
            X = Y = Z = Sqrt(1.0 / 3.0);
        }
        public Vector(double value)
        {
            X = Y = Z = value;
        }
        public Vector(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }
        public Vector(double theta, double phi)
        {
            this.X = this.Y = this.Z = 1.0;
            this.Length = 1.0;
            this.Theta = theta;
            this.Phi = phi;
        }

        public Vector(Vector vector)
        {
            this.X = vector.X;
            this.Y = vector.Y;
            this.Z = vector.Z;
        }

        public Vector(Orientation orientation)
        {
            this.X = orientation.X;
            this.Y = orientation.Y;
            this.Z = orientation.Z;
        }

        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double Length
        {
            get { return Sqrt(X * X + Y * Y + Z * Z); }
            set
            {
                if (value <= 0)
                    throw new System.Exception("Value must be > 0");
                double theta = Theta;
                double phi = Phi;
                X = value * Sin(theta) * Cos(phi);
                Y = value * Sin(theta) * Sin(phi);
                Z = value * Cos(theta);
            }
        }
        public double Theta
        {
            get { return Acos(Z / Length); }
            set
            {
                double length = Length;
                double phi = Phi;
                X = length * Sin(value) * Cos(phi);
                Y = length * Sin(value) * Sin(phi);
                Z = length * Cos(value);
            }
        }
        public double Phi
        {
            get { return (X == 0.0) ? ((Y > 0.0) ? PI / 2.0 : ((Y == 0.0) ? 0.0 : -PI / 2.0)) : Atan(Y / X); }
            set
            {
                double length = Length;
                double theta = Theta;
                X = length * Sin(theta) * Cos(value);
                Y = length * Sin(theta) * Sin(value);
                Z = length * Cos(theta);
            }
        }

        public Orientation Orientation
        {
            get { return new Orientation(X, Y, Z); }
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
                return new Vector(X, Y, 0.0);
            }
        }

        public Vector XZprojection
        {
            get
            {
                return new Vector(X, 0.0, Z);
            }
        }

        public Vector YZprojection
        {
            get
            {
                return new Vector(0.0, Y, Z);
            }
        }

        public void TurnX(double angle)
        {
            double x = X, y = Y, z = Z;
            Coordinates.UnmanagedCoordinates.turnX(ref x, ref y, ref z, angle);
            X = x;
            Y = y;
            Z = z;
        }
        public void TurnY(double angle)
        {
            double x = X, y = Y, z = Z;
            Coordinates.UnmanagedCoordinates.turnY(ref x, ref y, ref z, angle);
            X = x;
            Y = y;
            Z = z;
        }
        public void TurnZ(double angle)
        {
            double x = X, y = Y, z = Z;
            Coordinates.UnmanagedCoordinates.turnZ(ref x, ref y, ref z, angle);
            X = x;
            Y = y;
            Z = z;
        }

        public void turnAxis(Vector axis, double angle)
        {
            double x = X, y = Y, z = Z;
            Coordinates.UnmanagedCoordinates.turnAxis(ref x, ref y, ref z, axis.X, axis.Y, axis.Z, angle);
            X = x;
            Y = y;
            Z = z;
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
            result.X = vector1.X + vector2.X;
            result.Y = vector1.Y + vector2.Y;
            result.Z = vector1.Z + vector2.Z;
            return result;
        }
        public static Vector operator -(Vector vector1, Vector vector2)
        {
            Vector result = new Vector();
            result.X = vector1.X - vector2.X;
            result.Y = vector1.Y - vector2.Y;
            result.Z = vector1.Z - vector2.Z;
            return result;
        }
        public static Vector operator -(Vector vector)
        {
            Vector result = new Vector();
            result.X = -vector.X;
            result.Y = -vector.Y;
            result.Z = -vector.Z;
            return result;
        }
        public static Vector operator *(Vector vector1, Vector vector2)
        {
            Vector result = new Vector();
            result.X = vector1.Y * vector2.Z - vector1.Z * vector2.Y;
            result.Y = vector1.Z * vector2.X - vector1.X * vector2.Z;
            result.Z = vector1.X * vector2.Y - vector1.Y * vector2.X;
            return result;
        }
        public static Vector operator *(Vector vector, double value)
        {
            Vector result = new Vector(vector);
            result.X *= value;
            result.Y *= value;
            result.Z *= value;
            return result;
        }
        public static Vector operator /(Vector vector, double value)
        {
            Vector result = new Vector(vector);
            result.X /= value;
            result.Y /= value;
            result.Z /= value;
            return result;
        }

        public static double MultiplyScalar(Vector vector1, Vector vector2)
        {
            return vector1.X * vector2.X + vector1.Y * vector2.Y + vector1.Z * vector2.Z;
        }

        public static double GetAngle(Vector vector1, Vector vector2)
        {
            return Acos(MultiplyScalar(vector1, vector2) / vector1.Length / vector2.Length);
        }
    }

    public class Orientation : Vector
    {
        public Orientation() : base()
        {

        }
        public Orientation(double x, double y, double z) : base(x, y, z)
        {
            base.Length = 1.0;
        }
        public Orientation(double theta, double phi) : base(theta, phi)
        {

        }
        public Orientation(Orientation orientation) : base(orientation.Theta, orientation.Phi)
        {

        }
        public Orientation(Vector vector) : base(vector.Theta, vector.Phi)
        {

        }
        new public double Length { get { return 1.0; } }

        public static Orientation operator +(Orientation vector1, Orientation vector2)
        {
            Vector result = new Vector();
            result.X = vector1.X + vector2.X;
            result.Y = vector1.Y + vector2.Y;
            result.Z = vector1.Z + vector2.Z;
            result /= result.Length;
            return result.Orientation;
        }
        public static Orientation operator -(Orientation vector1, Orientation vector2)
        {
            Vector result = new Vector();
            result.X = vector1.X - vector2.X;
            result.Y = vector1.Y - vector2.Y;
            result.Z = vector1.Z - vector2.Z;
            result /= result.Length;
            return result.Orientation;
        }
        public static Orientation operator -(Orientation vector)
        {
            Vector result = new Vector();
            result.X = -vector.X;
            result.Y = -vector.Y;
            result.Z = -vector.Z;
            return result.Orientation;
        }
        public static Orientation operator *(Orientation vector1, Orientation vector2)
        {
            Vector result = new Vector();
            result.X = vector1.Y * vector2.Z - vector1.Z * vector2.Y;
            result.Y = vector1.Z * vector2.X - vector1.X * vector2.Z;
            result.Z = vector1.X * vector2.Y - vector1.Y * vector2.X;
            result /= result.Length;
            return result.Orientation;
        }

        public static Orientation AxisX
        {
            get { return new Vector(1.0, 0.0, 0.0).Orientation; }
        }
        public static Orientation AxisY
        {
            get { return new Vector(0.0, 1.0, 0.0).Orientation; }
        }
        public static Orientation AxisZ
        {
            get { return new Vector(0.0, 0.0, 1.0).Orientation; }
        }
    }
}
