using System;
using CoordinateSystems;
using static System.Math;

namespace CelestialMechanics
{
    public struct KeplerianElements
    {
        private const double ROUNDING_CONSTANT = 1000000000000000.0;
        private const int ITERATION_CONSTANT = 100;
        private const double G = 6.6743e-11;

        public double JulianDate;
        public double Eccentricity;
        public double SemimajorAxis;
        public double Inclination;
        public double AscendingNodeLongitude;
        public double PeriapsisArgument;
        public double TrueAnomaly;
        public Body CentralBody;



        private double GetEccentricAnomaly(double trueAnomaly)
        {
            // Without this operator in case eccentricity == 0, 
            // function won't work correctly

            if (Eccentricity == 0.0)
            {
                return TrueAnomaly;
            }

            // Without ROUNDING_CONSTANT eccentricity may be nan, because
            // acos takes value between -1 and 1

            double eccentric_anomaly = Acos(Floor((1 - (1 - Eccentricity * Eccentricity) /
                (1 + Eccentricity * Cos(TrueAnomaly))) / Eccentricity * ROUNDING_CONSTANT) / ROUNDING_CONSTANT);

            // If sin(trueAnomaly) < 0, eccentric anomaly < 0,
            // but 0 <= acos() <= PI

            if (Sin(TrueAnomaly) < 0)
            {
                eccentric_anomaly = -eccentric_anomaly;
            }

            return eccentric_anomaly;
        }

        private double GetMeanAnomaly(double trueAnomaly)
        {
            double eccentric_anomaly = GetEccentricAnomaly(trueAnomaly);
            return eccentric_anomaly - Eccentricity * Sin(eccentric_anomaly);
        }

        private double GetTrueAnomaly2(double meanAnomaly)
        {
            // This function solves Kepler equation by fixed-point iteration method
            // More information:
            // https://en.wikipedia.org/wiki/Kepler%27s_equation#Fixed-point_iteration

            double eccentricAnomaly = meanAnomaly;

            for (int i = 0; i < ITERATION_CONSTANT; i++)
            {
                eccentricAnomaly = Eccentricity * Sin(eccentricAnomaly) + meanAnomaly;
            }

            double true_anomaly = Acos((Cos(eccentricAnomaly) - Eccentricity) /
                (1 - Eccentricity * Cos(eccentricAnomaly)));

            if (Sin(eccentricAnomaly) < 0)
            {
                true_anomaly = -true_anomaly;
            }

            return true_anomaly;
        }

        public double GetTrueAnomaly(double julianDate)
        {
            double meanAnomaly = MeanAnomaly + MeanAngularVelocity * (julianDate - JulianDate);
            return GetTrueAnomaly2(meanAnomaly);
        }

        public double GetTime(double trueAnomaly)
        {
            double TimeInterval = (GetMeanAnomaly(trueAnomaly) - MeanAnomaly) / MeanAngularVelocity;

            if (TimeInterval < 0)
            {
                TimeInterval += OrbitalPeriod;
            }

            return TimeInterval;
        }

        private double MeanAnomaly
        {
            get
            {
                return GetMeanAnomaly(TrueAnomaly);
            }
        }

        private double MeanAngularVelocity
        {
            get
            {
                return Sqrt(G * CentralBody.Mass / SemimajorAxis / SemimajorAxis / SemimajorAxis);
            }
        }

        private double OrbitalPeriod
        {
            get
            {
                return 2 * PI / MeanAngularVelocity;
            }
        }

        public double Distance
        {
            get
            {
                return SemimajorAxis * (1 - Eccentricity * Eccentricity) / (1 + Eccentricity * Cos(TrueAnomaly));
            }
        }
    }
}
