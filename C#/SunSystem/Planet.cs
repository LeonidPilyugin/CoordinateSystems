using System;
using System.Collections.Generic;
using System.Text;
using CoordinateSystems;

namespace SunSystem
{
    public class Planet : Body
    {
        #region data
        protected double mass;
        protected double minRadius;
        protected double maxRadius;
        #endregion

        #region constructors
        public Planet(double mass, double minRadius, double maxRadius,
            string id, Vector vector, Basis basis, Velocity velocity, CoordinateSystem referenceSystem) :
            base(id, vector, basis, velocity, referenceSystem)
        {
            this.mass = mass;
            this.minRadius = minRadius;
            this.maxRadius = maxRadius;
        }
        #endregion

        #region methods
        public delegate (Vector Vector, Velocity Velocity, Basis basis) GetParams(double julianDate);

        public GetParams GetParamsMethod;

        public override bool IsInside(Vector point)
        {
            return point.X * point.X / maxRadius / maxRadius +
            point.Y * point.Y / maxRadius / maxRadius +
            point.Z * point.Z / minRadius / minRadius < 1.0;
        }

        public override bool IsCrossing(Vector vector1, Vector vector2)
        {
            Vector projection = GetProjection(vector1, vector2, new Vector(0.0, 0.0, 0.0));
            if (!IsInside(projection))
                return false;

            if (vector1.X != vector2.X)
                return (projection.X < vector1.X && projection.X > vector2.X) ||
                    (projection.X < vector2.X && projection.X > vector1.X);

            if (vector1.Y != vector2.Y)
                return (projection.Y < vector1.Y && projection.Y > vector2.Y) ||
                (projection.Y < vector2.Y && projection.Y > vector1.Y);

            return (projection.Z < vector1.Z && projection.Z > vector2.Z) ||
                (projection.Z < vector2.Z && projection.Z > vector1.Z);
        }

        protected Vector GetDirectionVector(Vector point1, Vector point2)
        {
            Vector result = point2 - point1;
            result /= result.Length;
            return result;
        }

        protected Vector GetProjection(Vector vector1, Vector vector2, Vector point)
        {
            Vector direction = GetDirectionVector(vector1, vector2);
            return vector1 - direction * Vector.MultiplyScalar((vector1 - point), direction)
                / direction.Length / direction.Length;
        }

        public void UpdateParams(double julianDate)
        {
            (vector, velocity, basis) = GetParamsMethod(julianDate);
        }
        #endregion

        #region properties
        public double Mass
        {
            get
            {
                return mass;
            }
        }
        #endregion
    }

    public static class Planets
    {
        #region data
        private static Planet sun;
        private static Planet mercury;
        private static Planet venus;
        private static Planet earth;
        private static Planet fixedEarth;
        private static Planet moon;
        private static Planet mars;
        private static Planet jupiter;
        private static Planet saturn;
        private static Planet uranus;
        private static Planet neptune;
        #endregion

        #region constructor
        static Planets()
        {
            sun = new Planet(1.9885e30, 6.9551e8, 6.955e8, "Sun",
                new Vector(0.0, 0.0, 0.0), new Basis(), new Velocity(0.0, 0.0, 0.0), null);
            sun.GetParamsMethod += SunParams;

            mercury = new Planet(3.33022e23, 2.4397e6, 2.4397e6, "Mercury",
                new Vector(0.0, 0.0, 0.0), new Basis(), new Velocity(0.0, 0.0, 0.0), sun);
            mercury.GetParamsMethod += MercuryParams;

            venus = new Planet(4.8675e24, 6.0518e6, 6.0518e6, "Venus",
                new Vector(0.0, 0.0, 0.0), new Basis(), new Velocity(0.0, 0.0, 0.0), sun);
            venus.GetParamsMethod += VenusParams;

            earth = new Planet(5.9722e24, 6.3781e6, 6.3568e6, "Earth",
                new Vector(0.0, 0.0, 0.0), new Basis(), new Velocity(0.0, 0.0, 0.0), sun);
            earth.GetParamsMethod += EarthParams;

            fixedEarth = new Planet(0.0, 6.3781e6, 6.3568e6, "Fixed Earth",
                new Vector(0.0, 0.0, 0.0), new Basis(), new Velocity(0.0, 0.0, 0.0), earth);
            fixedEarth.GetParamsMethod += FixedEarthParams;

            moon = new Planet(7.342e22, 1.73814e6, 1.7371e6, "Moon",
                new Vector(0.0, 0.0, 0.0), new Basis(), new Velocity(0.0, 0.0, 0.0), earth);
            moon.GetParamsMethod += MoonParams;

            mars = new Planet(6.4171e23, 3.3962e6, 3.3762e6, "Mars",
                new Vector(0.0, 0.0, 0.0), new Basis(), new Velocity(0.0, 0.0, 0.0), sun);
            mars.GetParamsMethod += MarsParams;

            jupiter = new Planet(1.8986e27, 71.492e6, 66.854e6, "Jupiter",
                new Vector(0.0, 0.0, 0.0), new Basis(), new Velocity(0.0, 0.0, 0.0), sun);
            jupiter.GetParamsMethod += JupiterParams;

            saturn = new Planet(5.6846e26, 60.268e6, 54.364e6, "Saturn", 
                new Vector(0.0, 0.0, 0.0), new Basis(), new Velocity(0.0, 0.0, 0.0), sun);
            saturn.GetParamsMethod += SaturnParams;

            uranus = new Planet(8.6813e25, 25.559e6, 24.973e6, "Uranus",
                new Vector(0.0, 0.0, 0.0), new Basis(), new Velocity(0.0, 0.0, 0.0), sun);
            uranus.GetParamsMethod += UranusParams;

            neptune = new Planet(1.0243e26, 24.764e6, 24.341e6, "Neptune",
                new Vector(0.0, 0.0, 0.0), new Basis(), new Velocity(0.0, 0.0, 0.0), sun);
            neptune.GetParamsMethod += NeptuneParams;
        }
        #endregion

        #region params functions
        private static (Vector Vector, Velocity Velocity, Basis Basis) SunParams(double julianDate)
        {
            return (new Vector(0.0, 0.0, 0.0), new Velocity(0.0, 0.0, 0.0), new Basis());
        }

        private static (Vector Vector, Velocity Velocity, Basis basis) MercuryParams(double julianDate)
        {
            var pv = new double[2, 3];
            IAUSOFA.iauPlan94(Date.J2000, julianDate - Date.J2000, 1, pv);
            var vector = new Vector(pv[0, 0], pv[0, 1], pv[0, 2]);
            var velocity = new Velocity(pv[1, 0], pv[1, 1], pv[1, 2]);
            return (vector * Coordinates.DAU, velocity * Coordinates.DAU / Date.JDtoSecond, new Basis());
        }

        private static (Vector Vector, Velocity Velocity, Basis basis) VenusParams(double julianDate)
        {
            var pv = new double[2, 3];
            IAUSOFA.iauPlan94(Date.J2000, julianDate - Date.J2000, 2, pv);
            var vector = new Vector(pv[0, 0], pv[0, 1], pv[0, 2]);
            var velocity = new Velocity(pv[1, 0], pv[1, 1], pv[1, 2]);
            return (vector * Coordinates.DAU, velocity * Coordinates.DAU / Date.JDtoSecond, new Basis());
        }

        private static (Vector Vector, Velocity Velocity, Basis basis) EarthParams(double julianDate)
        {
            var pvh = new double[2, 3];
            var pvb = new double[2, 3];
            IAUSOFA.iauEpv00(Date.J2000, julianDate - Date.J2000, pvh, pvb);
            var vector = new Vector(pvh[0, 0], pvh[0, 1], pvh[0, 2]);
            var velocity = new Velocity(pvh[1, 0], pvh[1, 1], pvh[1, 2]);
            return (vector * Coordinates.DAU, velocity * Coordinates.DAU / Date.JDtoSecond, new Basis());
        }

        private static (Vector Vector, Velocity Velocity, Basis basis) FixedEarthParams(double julianDate)
        {
            var basis = new Basis();
            var matrix = Coordinates.GetICStoGCSmatrix(julianDate);
            basis.I = matrix * basis.I;
            basis.J = matrix * basis.J;
            basis.K = matrix * basis.K;
            return (new Vector(0.0, 0.0, 0.0), new Velocity(0.0, 0.0, 0.0), basis);
        }

        private static (Vector Vector, Velocity Velocity, Basis basis) MoonParams(double julianDate)
        {
            (Vector EarthVector, Velocity EarthVelocity, Basis basis) = EarthParams(julianDate);
            var pv = new double[2, 3];
            IAUSOFA.iauPlan94(Date.J2000, julianDate - Date.J2000, 3, pv);
            Vector EMBVector = new Vector(pv[0, 0], pv[0, 1], pv[0, 2]) * Coordinates.DAU;
            Velocity EMBVelocity = new Velocity(pv[1, 0], pv[1, 1], pv[1, 2]) * Coordinates.DAU / Date.JDtoSecond;

            return (new Vector((EMBVector * (earth.Mass + moon.Mass) - EarthVector * earth.Mass) / moon.Mass),
                new Velocity((EMBVelocity * (earth.Mass + moon.Mass) - EarthVelocity * earth.Mass) / moon.Mass),
                basis);
        }

        private static (Vector Vector, Velocity Velocity, Basis basis) MarsParams(double julianDate)
        {
            var pv = new double[2, 3];
            IAUSOFA.iauPlan94(Date.J2000, julianDate - Date.J2000, 4, pv);
            var vector = new Vector(pv[0, 0], pv[0, 1], pv[0, 2]);
            var velocity = new Velocity(pv[1, 0], pv[1, 1], pv[1, 2]);
            return (vector * Coordinates.DAU, velocity * Coordinates.DAU / Date.JDtoSecond, new Basis());
        }

        private static (Vector Vector, Velocity Velocity, Basis basis) JupiterParams(double julianDate)
        {
            var pv = new double[2, 3];
            IAUSOFA.iauPlan94(Date.J2000, julianDate - Date.J2000, 5, pv);
            var vector = new Vector(pv[0, 0], pv[0, 1], pv[0, 2]);
            var velocity = new Velocity(pv[1, 0], pv[1, 1], pv[1, 2]);
            return (vector * Coordinates.DAU, velocity * Coordinates.DAU / Date.JDtoSecond, new Basis());
        }

        private static (Vector Vector, Velocity Velocity, Basis basis) SaturnParams(double julianDate)
        {
            var pv = new double[2, 3];
            IAUSOFA.iauPlan94(Date.J2000, julianDate - Date.J2000, 6, pv);
            var vector = new Vector(pv[0, 0], pv[0, 1], pv[0, 2]);
            var velocity = new Velocity(pv[1, 0], pv[1, 1], pv[1, 2]);
            return (vector * Coordinates.DAU, velocity * Coordinates.DAU / Date.JDtoSecond, new Basis());
        }

        private static (Vector Vector, Velocity Velocity, Basis basis) UranusParams(double julianDate)
        {
            var pv = new double[2, 3];
            IAUSOFA.iauPlan94(Date.J2000, julianDate - Date.J2000, 7, pv);
            var vector = new Vector(pv[0, 0], pv[0, 1], pv[0, 2]);
            var velocity = new Velocity(pv[1, 0], pv[1, 1], pv[1, 2]);
            return (vector * Coordinates.DAU, velocity * Coordinates.DAU / Date.JDtoSecond, new Basis());
        }

        private static (Vector Vector, Velocity Velocity, Basis basis) NeptuneParams(double julianDate)
        {
            var pv = new double[2, 3];
            IAUSOFA.iauPlan94(Date.J2000, julianDate - Date.J2000, 8, pv);
            var vector = new Vector(pv[0, 0], pv[0, 1], pv[0, 2]);
            var velocity = new Velocity(pv[1, 0], pv[1, 1], pv[1, 2]);
            return (vector * Coordinates.DAU, velocity * Coordinates.DAU / Date.JDtoSecond, new Basis());
        }
        #endregion

        #region properties
        public static Planet Sun
        {
            get
            {
                return sun;
            }
        }

        public static Planet Mercury
        {
            get
            {
                return mercury;
            }
        }

        public static Planet Venus
        {
            get
            {
                return venus;
            }
        }

        public static Planet Earth
        {
            get
            {
                return earth;
            }
        }

        public static Planet FixedEarth
        {
            get
            {
                return fixedEarth;
            }
        }

        public static Planet Moon
        {
            get
            {
                return moon;
            }
        }

        public static Planet Mars
        {
            get
            {
                return mars;
            }
        }

        public static Planet Jupiter
        {
            get
            {
                return jupiter;
            }
        }

        public static Planet Saturn
        {
            get
            {
                return saturn;
            }
        }

        public static Planet Uranus
        {
            get
            {
                return uranus;
            }
        }

        public static Planet Neptune
        {
            get
            {
                return neptune;
            }
        }
        #endregion

        #region functions
        public static void UpdateParams(double julianDate)
        {
            sun.UpdateParams(julianDate);
            mercury.UpdateParams(julianDate);
            venus.UpdateParams(julianDate);
            earth.UpdateParams(julianDate);
            fixedEarth.UpdateParams(julianDate);
            mars.UpdateParams(julianDate);
            jupiter.UpdateParams(julianDate);
            saturn.UpdateParams(julianDate);
            uranus.UpdateParams(julianDate);
            neptune.UpdateParams(julianDate);
        }
        #endregion
    }
}
