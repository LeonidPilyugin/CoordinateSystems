using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoordinateSystems;
using static System.Math;

namespace Connection
{
    public abstract class View
    {
        #region data
        protected double length;
        #endregion

        #region constructors
        public View(double length)
        {
            this.length = length;
        }
        #endregion

        #region properties
        public Vector Axis
        {
            get
            {
                return new Vector(length, 0.0, 0.0);
            }
        }

        public double Length
        {
            get
            {
                return length;
            }

            set
            {
                if (value <= 0.0)
                {
                    throw new Exception("Length must be > 0");
                }

                length = value;
            }
        }
        #endregion

        #region functions
        public abstract bool IsInside(Vector point);
        #endregion
    }

    public class ConicView : View
    {
        #region data
        protected double angle;
        #endregion

        #region constructors
        public ConicView(double angle, double length) : base(length)
        {
            this.angle = angle;
        }

        public ConicView(ConicView view) : this(view.angle, view.length)
        {

        }
        #endregion

        #region properties
        public double Angle
        {
            get
            {
                return angle;
            }

            set
            {
                if(value <= 0.0 || value > PI / 2.0)
                {
                    throw new Exception("Angle must be <= PI / 2.0 and > 0.0");
                }

                angle = value;
            }
        }
        #endregion

        #region functions
        public override bool IsInside(Vector point)
        {
            return (Abs(Cos(Vector.GetAngle(point, Axis))) >= Cos(angle)) && (point.Length <= length);
        }
        #endregion
    }

    public class PyramidView : View
    {
        #region data
        protected double angle1;
        protected double angle2;
        #endregion

        #region constructors
        public PyramidView(double angle1, double angle2, double length) : base(length)
        {
            this.angle1 = angle1;
            this.angle2 = angle2;
        }

        public PyramidView(PyramidView view) : this(view.angle1, view.angle2, view.length)
        {

        }
        #endregion

        #region properties
        public double Angle1
        {
            get
            {
                return angle1;
            }

            set
            {
                if (value <= 0.0 || value > PI / 2.0)
                {
                    throw new Exception("Angle1 must be <= PI / 2.0 and > 0.0");
                }

                angle1 = value;
            }
        }
        public double Angle2
        {
            get
            {
                return angle2;
            }

            set
            {
                if (value <= 0.0 || value > PI / 2.0)
                {
                    throw new Exception("Angle1 must be <= PI / 2.0 and > 0.0");
                }

                angle2 = value;
            }
        }
        #endregion

        #region functions
        public override bool IsInside(Vector point)
        {
            return (point.X * Tan(angle1) >= Abs(point.Y)) &&
                (point.X * Tan(angle2) >= Abs(point.Z)) && (point.Length <= length);
        }
        #endregion
    }
}
