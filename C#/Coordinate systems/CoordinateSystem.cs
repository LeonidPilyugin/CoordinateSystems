using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoordinateSystems
{
    // класс нужен для создания системы координат (СК)
    public class CoordinateSystem
    {
        // вектор от базовой СК
        protected Vector vector;

        // базис относительно базовой СК
        protected Basis basis;

        // базовая СК, если она null, то базовая СК — гелиоцентрическая
        protected CoordinateSystem referenceSystem;



        public CoordinateSystem(Vector vector, Basis basis, CoordinateSystem referenceSystem = null)
        {
            this.vector = vector;
            this.basis = basis;
            this.referenceSystem = referenceSystem;
        }

        public CoordinateSystem()
        {
            this.vector = new Vector();
            this.basis = new Basis();
            this.referenceSystem = null;
        }

        public CoordinateSystem(CoordinateSystem coordinateSystem)
        {
            this.vector = coordinateSystem.vector;
            this.basis = coordinateSystem.basis;
            this.referenceSystem = coordinateSystem.referenceSystem;
        }



        public Vector Vector
        {
            get { return vector; }
            set
            {
                if (value == null)
                    throw new Exception("Vector can't be null");
                vector = value;
            }
        }

        public Basis Basis
        {
            get { return basis; }
            set
            {
                if (value == null)
                    throw new Exception("Basis can't be null");
                basis = value;
            }
        }

        public CoordinateSystem ReferenceSystem
        {
            get { return referenceSystem; }
            set { referenceSystem = value; }
        }

        // матрица переноса относительно базовой СК
        protected Matrix TransitionMatrix
        {
            get { return new Matrix(basis).Transposed; }
        }

        // матрица переноса относительно гелиоцентрической СК
        protected Matrix TransitionMatrixRelativelySun
        {
            get
            {
                Matrix result = new Matrix();
                var temp = this;
                var matrices = new Stack<Matrix>();

                do
                {
                    matrices.Push(temp.TransitionMatrix);
                    temp = temp.ReferenceSystem;
                } while (temp != null);

                do
                {
                    result *= matrices.Pop();
                } while (matrices.Count > 0);

                return result;
            }
        }

        // базис относительно базовой СК
        protected Basis BasisRelativelyReferenceSystem
        {
            get
            {
                return new Basis(TransitionMatrix);
            }
        }

        // базис относительно гелиоцентрической СК
        protected Basis BasisRelativelySun
        {
            get
            {
                return new Basis(TransitionMatrixRelativelySun);
            }
        }

        // базис гелиоцентрической СК относительно этой СК
        protected Basis SunBasis
        {
            get
            {
                return new Basis(TransitionMatrixRelativelySun.Inversed);
            }
        }

        // базис базовой СК относительно этой СК
        protected Basis ReferenceSystemBasis
        {
            get
            {
                return new Basis(TransitionMatrix.Inversed);
            }
        }

        // вектор от Солнца до этой СК
        public Vector VectorFromSun
        {
            get { return GetVectorFromSun(); }
        }



        // переводит вектор point из этой СК в СК system
        public Vector ConvertTo(CoordinateSystem system, Vector point = null)
        {
            if (point == null)
                point = new Vector(0.0, 0.0, 0.0);

            return system.TransitionMatrixRelativelySun.Inversed * (GetVectorFromSun(point) - system.GetVectorFromSun());
        }

        // поворачивает СК осью I к точке target
        // точки из списка points, заданные относительно этой СК,
        // поворачиваются вместе с этой СК
        public void TurnTo(Vector target, List<Vector> points = null) 
        {
            UnitVector direction = new UnitVector(target);
            UnitVector axis = direction * basis.I;
            double angle = -Vector.GetAngle(direction, basis.I);

            if(points != null)
            {
                foreach (Vector point in points)
                {
                    point.TurnAxis(axis, angle);
                }
            }

            basis.TurnAxis(axis, angle);
        }

        // возвращает вектор от Солнца до точки, заданной в этой СК
        public Vector GetVectorFromSun(Vector point = null)
        {
            var result = point;
            if(result == null)
            {
                result = new Vector(0.0, 0.0, 0.0);
            }
            var temp = this;

            do
            {
                result = temp.GetVectorRelativelyReferenceSystem(result);
                temp = temp.ReferenceSystem;
            } while (temp != null);
            return result;
        }

        // возвращает вектор от базовой СК до точки, заданной в этой СК
        protected Vector GetVectorRelativelyReferenceSystem(Vector point = null)
        {
            var result = new Vector(0.0, 0.0, 0.0);

            if (point != null)
            {
                result = TransitionMatrix * point;
            }

            return result + vector;
        }
    }
}
