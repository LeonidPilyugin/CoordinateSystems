using System;
using CoordinateSystems;
using static System.Math;

namespace CelestialMechanics
{
    public abstract class Keplerian
    {
        #region consts
        protected const int ITERATION_CONSTANT = 100;
        protected const double G = 6.6743e-11;
        #endregion

        #region data
        public double julianDate;
        public double eccentricity;
        public double perifocusDistance;
        public double inclination;
        public double ascendingNodeLongitude;
        public double periapsisArgument;
        public double trueAnomaly;
        public Body centralBody;
        #endregion

        #region constructors
        public Keplerian(double julianDate, double eccentricity, double perifocusDistance,
            double inclination, double ascendingNodeLongitude, double periapsisArgument,
            double trueAnomaly, Body centralBody)
        {
            this.julianDate = julianDate;
            this.eccentricity = eccentricity;
            this.perifocusDistance = perifocusDistance;
            this.inclination = inclination;
            this.ascendingNodeLongitude = ascendingNodeLongitude;
            this.periapsisArgument = periapsisArgument;
            this.trueAnomaly = trueAnomaly;
            this.centralBody = centralBody;
        }

        public Keplerian(Keplerian keplerian) : this(keplerian.julianDate, keplerian.eccentricity,
            keplerian.perifocusDistance, keplerian.inclination, keplerian.ascendingNodeLongitude,
            keplerian.periapsisArgument, keplerian.trueAnomaly, keplerian.centralBody)
        {

        }
        #endregion

        #region properties
        public abstract Vector Vector { get; }
        protected abstract double MeanAngularVelocity { get; }
        public abstract double Eccentricity { get; set; }
        public abstract double TrueAnomaly { get; set; }

        public double JulianDate 
        {
            get 
            {
                return julianDate;
            }

            set
            {
                julianDate = value;
            }
        }

        public Body CentralBody
        {
            get
            {
                return centralBody;
            }

            set
            {
                centralBody = value;
            }
        }

        public double PerifocusDistance 
        {
            get
            {
                return perifocusDistance;
            }

            set
            {
                if(value < 0.0)
                {
                    throw new Exception("PerifocusDistance must be > 0");
                }

                perifocusDistance = value;
            }
        }

        public double Inclination
        {
            get
            {
                return inclination;
            }

            set
            {
                if(value < 0.0 || value > PI)
                {
                    throw new Exception("Inclination must be <= PI and >= 0");
                }

                inclination = value;
            }
        }

        public double AscendingNodeLongitude
        {
            get
            {
                return ascendingNodeLongitude;
            }

            set
            {
                if (value < 0.0 || value > 2.0 * PI)
                {
                    throw new Exception("AscendingNodeLongitude must be <= 2PI and >= 0");
                }

                ascendingNodeLongitude = value;
            }
        }

        public double PeriapsisArgument
        {
            get
            {
                return periapsisArgument;
            }

            set
            {
                if (value < 0.0 || value > 2.0 * PI)
                {
                    throw new Exception("PeriapsisArgument must be <= 2PI and >= 0");
                }

                periapsisArgument = value;
            }
        }
        #endregion

        #region methods
        // возвращает истинную аномалию в данный Юлианский день
        public abstract double GetTrueAnomaly(double julianDate);

        // возвращает Юлианский день, в который истинная аномалия равна данной
        public abstract double GetJD(double trueAnomaly);

        // возвращает вектор в ICS в данный Юлианский день
        public abstract Vector GetVector(double julianDate);
        #endregion
    }

    public abstract class NotParabolicKeplerian : Keplerian
    {
        #region constructors
        public NotParabolicKeplerian(double julianDate, double eccentricity, double semimajorAxis,
            double inclination, double ascendingNodeLongitude, double periapsisArgument,
            double trueAnomaly, Body centralBody) : base(julianDate, eccentricity,
                semimajorAxis * (1.0 - eccentricity), inclination, ascendingNodeLongitude,
                periapsisArgument, trueAnomaly, centralBody)
        {

        }

        public NotParabolicKeplerian(NotParabolicKeplerian keplerian) : base(keplerian)
        {

        }
        #endregion

        #region properties
        public double SemimajorAxis
        {
            get
            {
                return perifocusDistance / (1.0 - eccentricity);
            }

            set
            {
                if (value * (1.0 - eccentricity) <= 0.0)
                {
                    throw new Exception("SemimajorAxis must be > 0 for ellipse and < 0 for hyperbola");
                }

                perifocusDistance = value * (1.0 - eccentricity);
            }
        }

        public override Vector Vector
        {
            get
            {
                return GetVector(julianDate);
            }
        }
        #endregion

        #region methods
        protected abstract double GetEccentricAnomaly(double trueAnomaly);
        protected abstract double GetMeanAnomaly(double trueAnomaly);
        protected abstract double GetTrueAnomaly2(double meanAnomaly);

        // возвращает вектор в ICS в данный Юлианский день
        public override Vector GetVector(double julianDate)
        {
            double trueAnomaly = GetTrueAnomaly(julianDate);
            Vector result = new Vector(PI / 2.0, -trueAnomaly);
            result.Length = Abs(SemimajorAxis * (1.0 - eccentricity * eccentricity) /
                (1.0 + eccentricity * Cos(trueAnomaly)));

            result.TurnZ(-periapsisArgument);
            result.TurnX(-inclination);
            result.TurnZ(ascendingNodeLongitude);

            return result;
        }
        #endregion

    }

    public class ParabolicKeplerian : Keplerian
    {
        #region constructors
        public ParabolicKeplerian(double julianDate, double perifocusDistance,
            double inclination, double ascendingNodeLongitude, double periapsisArgument,
            double trueAnomaly, Body centralBody) : base(julianDate, 1.0,
                perifocusDistance, inclination, ascendingNodeLongitude,
                periapsisArgument, trueAnomaly, centralBody)
        {

        }

        public ParabolicKeplerian(ParabolicKeplerian keplerian) : base(keplerian)
        {

        }
        #endregion

        #region properties
        public override Vector Vector
        {
            get
            {
                return GetVector(julianDate);
            }
        }

        protected override double MeanAngularVelocity
        {
            get
            {
                return Sqrt(centralBody.Mass * G / 2.0 / perifocusDistance / perifocusDistance / perifocusDistance);
            }
        }

        public override double Eccentricity
        {
            get
            {
                return 1.0;
            }
            set => throw new NotImplementedException();
        }

        public override double TrueAnomaly
        {
            get
            {
                return trueAnomaly;
            }

            set
            {
                if (value >= PI || value <= -PI)
                {
                    throw new Exception("TrueAnomaly must be < PI and > -PI");
                }
            }
        }
        #endregion

        #region functions
        public override double GetJD(double trueAnomaly)
        {
            double tan1 = Tan(trueAnomaly / 2.0);
            double tan0 = Tan(this.trueAnomaly / 2.0);
            return (tan1 - tan0 + tan1 * tan1 * tan1 / 3.0 - tan0 * tan0 * tan0 / 3.0) /
                MeanAngularVelocity / Date.JDtoSecond + julianDate;
        }

        // возвращает истинную аномалию в данный Юлианский день
        public override double GetTrueAnomaly(double julianDate)
        {
            double jd0 = GetJD(0.0);
            double m = MeanAngularVelocity * (julianDate - jd0) * Date.JDtoSecond; // mean anomaly
            double x = 0.5 * Pow(12.0 * m + 4.0 * Sqrt(9.0 * m * m + 4.0), 1 / 3.0);
            return 2.0 * Atan(x - 1.0 / x);
        }

        // возвращает Юлианский день, в который истинная аномалия равна данной
        public override Vector GetVector(double julianDate)
        {
            double trueAnomaly = GetTrueAnomaly(julianDate);
            Vector result = new Vector(PI / 2.0, -trueAnomaly);
            result.Length = perifocusDistance * 2.0 / (1.0 + Cos(trueAnomaly));

            result.TurnZ(-periapsisArgument);
            result.TurnX(-inclination);
            result.TurnZ(ascendingNodeLongitude);

            return result;
        }
        #endregion
    }

    public class EllipticKeplerian : NotParabolicKeplerian
    {
        #region constructors

        public EllipticKeplerian(double julianDate, double eccentricity, double semimajorAxis,
            double inclination, double ascendingNodeLongitude, double periapsisArgument,
            double trueAnomaly, Body centralBody) :
            base(julianDate, eccentricity, semimajorAxis, inclination, ascendingNodeLongitude, periapsisArgument,
            trueAnomaly, centralBody)
        {

        }

        public EllipticKeplerian(NotParabolicKeplerian ke) : base(ke)
        {

        }

        #endregion

        #region properties
        protected override double MeanAngularVelocity
        {
            get
            {
                return Sqrt(G * centralBody.Mass / SemimajorAxis / SemimajorAxis / SemimajorAxis);
            }
        }

        protected double OrbitalPeriod
        {
            get
            {
                return 2 * PI / MeanAngularVelocity;
            }
        }

        public override double Eccentricity
        {
            get
            {
                return eccentricity;
            }

            set
            {
                if (value < 0.0 || value >= 1.0)
                {
                    throw new Exception("Eccentricity must be >=0 and < 1");
                }

                eccentricity = value;
            }
        }

        public override double TrueAnomaly
        {
            get
            {
                return trueAnomaly;
            }

            set
            {
                if (value < -PI || value > PI)
                {
                    throw new Exception("TrueAnomaly must be <= PI and >= -PI");
                }

                trueAnomaly = value;
            }
        }
        #endregion

        #region functions
        protected override double GetEccentricAnomaly(double trueAnomaly)
        {
            if (eccentricity == 0.0)
            {
                return trueAnomaly;
            }

            double eccentricAnomaly = 2.0 * Atan(Tan(trueAnomaly / 2.0) *
                Sqrt((1.0 - eccentricity) / (1.0 + eccentricity)));

            if (Sin(trueAnomaly) < 0)
            {
                eccentricAnomaly = -eccentricAnomaly;
            }

            return eccentricAnomaly;
        }

        protected override double GetMeanAnomaly(double trueAnomaly)
        {
            double eccentricAnomaly = GetEccentricAnomaly(trueAnomaly);
            return eccentricAnomaly - eccentricity * Sin(eccentricAnomaly);
        }

        protected override double GetTrueAnomaly2(double meanAnomaly)
        {
            double eccentricAnomaly = meanAnomaly;
            for (int i = 0; i < ITERATION_CONSTANT; i++)
            {
                eccentricAnomaly = eccentricity * Sin(eccentricAnomaly) + meanAnomaly;
            }

            double trueAnomaly = Acos((Cos(eccentricAnomaly) - eccentricity) /
                (1 - eccentricity * Cos(eccentricAnomaly)));

            if (Sin(eccentricAnomaly) < 0)
            {
                trueAnomaly = -trueAnomaly;
            }

            return trueAnomaly;
        }

        // возвращает истинную аномалию в данный Юлианский день
        public override double GetTrueAnomaly(double julianDate)
        {
            double meanAnomaly = GetMeanAnomaly(this.trueAnomaly) +
                MeanAngularVelocity * (julianDate - this.julianDate) * Date.JDtoSecond;
            return GetTrueAnomaly2(meanAnomaly);
        }

        // возвращает Юлианский день, в который истинная аномалия равна данной
        public override double GetJD(double trueAnomaly)
        {
            double timeInterval = (GetMeanAnomaly(trueAnomaly) -
                GetMeanAnomaly(this.trueAnomaly)) / MeanAngularVelocity;

            if (timeInterval < 0)
            {
                timeInterval += OrbitalPeriod;
            }

            return timeInterval / Date.JDtoSecond + this.julianDate;
        }
        #endregion
    }

    public class HyperbolicKeplerian : NotParabolicKeplerian
    {
        #region constructors
        public HyperbolicKeplerian(double julianDate, double eccentricity, double semimajorAxis,
            double inclination, double ascendingNodeLongitude, double periapsisArgument,
            double trueAnomaly, Body centralBody) :
            base(julianDate, eccentricity, semimajorAxis, inclination, ascendingNodeLongitude, periapsisArgument,
            trueAnomaly, centralBody)
        {

        }

        public HyperbolicKeplerian(NotParabolicKeplerian keplerian) : base(keplerian)
        {

        }
        #endregion

        #region properties
        protected override double MeanAngularVelocity
        {
            get
            {
                return Sqrt(-G * centralBody.Mass / SemimajorAxis / SemimajorAxis / SemimajorAxis);
            }
        }

        public override double Eccentricity
        {
            get
            {
                return eccentricity;
            }

            set
            {
                if (value <= 1.0)
                {
                    throw new Exception("Eccentricity must be > 1.0");
                }

                eccentricity = value;
            }
        }

        public override double TrueAnomaly
        {
            get
            {
                return trueAnomaly;
            }

            set
            {
                if (eccentricity * Cos(value) <= -1.0)
                {
                    throw new Exception("TrueAnomaly isn't correct");
                }

                trueAnomaly = value;
            }
        }
        #endregion

        #region functions
        protected override double GetEccentricAnomaly(double trueAnomaly)
        {
            double Arth(double z)
            {
                return Log((1 + z) / (1 - z)) / 2.0;
            }

            double eccentricAnomaly = 2.0 * Arth(Tan(trueAnomaly / 2.0) *
                Sqrt((eccentricity - 1.0) / (eccentricity + 1.0)));
            return eccentricAnomaly;
        }

        protected override double GetMeanAnomaly(double trueAnomaly)
        {
            double eccentricAnomaly = GetEccentricAnomaly(trueAnomaly);
            return eccentricity * Sinh(eccentricAnomaly) - eccentricAnomaly;
        }

        protected override double GetTrueAnomaly2(double meanAnomaly)
        {
            double Arsh(double n)
            {
                return Log(n + Sqrt(n * n + 1.0));
            }

            double eccentricAnomaly = meanAnomaly;

            for (int i = 0; i < ITERATION_CONSTANT; i++)
            {
                eccentricAnomaly = Arsh((eccentricAnomaly + meanAnomaly) / eccentricity);
            }

            return 2.0 * Atan(Tanh(eccentricAnomaly / 2.0) *
                Sqrt((eccentricity + 1.0) / (eccentricity - 1.0)));
        }

        // возвращает истинную аномалию в данный Юлианский день
        public override double GetTrueAnomaly(double julianDate)
        {
            double meanAnomaly = GetMeanAnomaly(this.trueAnomaly) + MeanAngularVelocity *
                    (julianDate - this.julianDate) * Date.JDtoSecond;

            return GetTrueAnomaly2(meanAnomaly);
        }

        // возвращает Юлианский день, в который истинная аномалия равна данной
        public override double GetJD(double trueAnomaly)
        {
            if (eccentricity * Cos(trueAnomaly) <= -1.0)
            {
                throw new Exception("TrueAnomaly isn't correct");
            }

            return ((GetMeanAnomaly(trueAnomaly) - GetMeanAnomaly(this.trueAnomaly)) /
                MeanAngularVelocity / Date.JDtoSecond) + this.julianDate;
        }
        #endregion
    }
}
