using System;
using CoordinateSystems;
using static System.Math;
using System.Numerics;
using static System.Numerics.Complex;

namespace CelestialMechanics
{
    public struct NotParabolicKeplerianElements
    {
        public double JulianDate;
        public double Eccentricity;
        public double SemimajorAxis;
        public double Inclination;
        public double AscendingNodeLongitude;
        public double PeriapsisArgument;
        public double TrueAnomaly;
        public Body CentralBody;

        public NotParabolicKeplerianElements(double julianDate, double eccentricity, double semimajorAxis,
            double inclination, double ascendingNodeLongitude, double periapsisArgument,
            double trueAnomaly, Body centralBody)
        {
            JulianDate = julianDate;
            Eccentricity = eccentricity;
            SemimajorAxis = semimajorAxis;
            Inclination = inclination;
            AscendingNodeLongitude = ascendingNodeLongitude;
            PeriapsisArgument = periapsisArgument;
            TrueAnomaly = trueAnomaly;
            CentralBody = centralBody;
        }

        public NotParabolicKeplerianElements(NotParabolicKeplerianElements elements)
        {
            this.JulianDate = elements.JulianDate;
            this.Eccentricity = elements.Eccentricity;
            this.SemimajorAxis = elements.SemimajorAxis;
            this.Inclination = elements.Inclination;
            this.AscendingNodeLongitude = elements.AscendingNodeLongitude;
            this.PeriapsisArgument = elements.PeriapsisArgument;
            this.TrueAnomaly = elements.TrueAnomaly;
            this.CentralBody = elements.CentralBody;
        }

        public NotParabolicKeplerianElements(ParabolicKeplerianElements elements, double eccentricity = 0.0)
        {
            this.JulianDate = elements.JulianDate;
            this.Eccentricity = eccentricity;
            this.SemimajorAxis = elements.PerifocusDistance / Abs(1.0 - eccentricity);
            this.Inclination = elements.Inclination;
            this.AscendingNodeLongitude = elements.AscendingNodeLongitude;
            this.PeriapsisArgument = elements.PeriapsisArgument;
            this.TrueAnomaly = elements.TrueAnomaly;
            this.CentralBody = elements.CentralBody;
        }
    }
    public struct ParabolicKeplerianElements
    {
        public double JulianDate;
        public double PerifocusDistance;
        public double Inclination;
        public double AscendingNodeLongitude;
        public double PeriapsisArgument;
        public double TrueAnomaly;
        public Body CentralBody;

        public ParabolicKeplerianElements(double julianDate, double perifocusDistance, double inclination,
            double ascendingNodeLongitude, double periapsisArgument, double trueAnomaly, Body centralBody)
        {
            JulianDate = julianDate;
            PerifocusDistance = perifocusDistance;
            Inclination = inclination;
            AscendingNodeLongitude = ascendingNodeLongitude;
            PeriapsisArgument = periapsisArgument;
            TrueAnomaly = trueAnomaly;
            CentralBody = centralBody;
        }

        public ParabolicKeplerianElements(ParabolicKeplerianElements elements)
        {
            JulianDate = elements.JulianDate;
            PerifocusDistance = elements.PerifocusDistance;
            Inclination = elements.Inclination;
            AscendingNodeLongitude = elements.AscendingNodeLongitude;
            PeriapsisArgument = elements.PeriapsisArgument;
            TrueAnomaly = elements.TrueAnomaly;
            CentralBody = elements.CentralBody;
        }

        public ParabolicKeplerianElements(NotParabolicKeplerianElements elements)
        {
            JulianDate = elements.JulianDate;
            PerifocusDistance = elements.SemimajorAxis * Abs(1.0 - elements.Eccentricity);
            Inclination = elements.Inclination;
            AscendingNodeLongitude = elements.AscendingNodeLongitude;
            PeriapsisArgument = elements.PeriapsisArgument;
            TrueAnomaly = elements.TrueAnomaly;
            CentralBody = elements.CentralBody;
        }
    }


    public abstract class Keplerian
    {
        protected const int ITERATION_CONSTANT = 100;
        protected const double G = 6.6743e-11;

        protected object elements;

        public abstract double GetTrueAnomaly(double julianDate);
        public abstract double GetJD(double trueAnomaly);
    }

    public abstract class NotParabolicKeplerian : Keplerian
    {
        public NotParabolicKeplerian(double julianDate, double eccentricity, double semimajorAxis,
            double inclination, double ascendingNodeLongitude, double periapsisArgument,
            double trueAnomaly, Body centralBody)
        {
            elements = new NotParabolicKeplerianElements(julianDate, eccentricity, semimajorAxis,
                inclination, ascendingNodeLongitude, periapsisArgument, trueAnomaly, centralBody);
        }

        public NotParabolicKeplerian(NotParabolicKeplerianElements ke)
        {
            elements = new NotParabolicKeplerianElements(ke);
        }

        public NotParabolicKeplerian(ParabolicKeplerianElements ke)
        {
            elements = new NotParabolicKeplerianElements(ke);
        }

        public NotParabolicKeplerian(NotParabolicKeplerian k) : this(k.Elements)
        {

        }

        public NotParabolicKeplerian(ParabolicKeplerian k) : this(k.Elements)
        {

        }

        protected abstract Complex GetEccentricAnomaly(double trueAnomaly);
        protected abstract Complex GetMeanAnomaly(double trueAnomaly);
        protected abstract double GetTrueAnomaly(Complex meanAnomaly);

        protected abstract Complex MeanAngularVelocity { get; }

        public NotParabolicKeplerianElements Elements
        {
            get { return (NotParabolicKeplerianElements)elements; }
        }
    }

    public class ParabolicKeplerian : Keplerian
    {
        public ParabolicKeplerian(double julianDate, double perifocusDistance,
            double inclination, double ascendingNodeLongitude, double periapsisArgument,
            double trueAnomaly, Body centralBody)
        {
            elements = new ParabolicKeplerianElements(julianDate, perifocusDistance,
                inclination, ascendingNodeLongitude, periapsisArgument, trueAnomaly, centralBody);
        }

        public ParabolicKeplerian(ParabolicKeplerianElements ke)
        {
            elements = new ParabolicKeplerianElements(ke);
        }

        public ParabolicKeplerian(NotParabolicKeplerianElements ke)
        {
            elements = new ParabolicKeplerianElements(ke);
        }

        public ParabolicKeplerian(ParabolicKeplerian keplerian) : this(keplerian.Elements)
        {

        }

        public ParabolicKeplerian(NotParabolicKeplerian keplerian) : this(keplerian.Elements)
        {

        }



        protected double MeanAngularVelocity
        {
            get
            {
                return Sqrt(G * Elements.CentralBody.Mass / 2.0 / Elements.PerifocusDistance /
                    Elements.PerifocusDistance / Elements.PerifocusDistance);
            }
        }

        protected double GetTrueAnomaly2(double meanAnomaly)
        {
            double m = meanAnomaly;
            double x = Pow(12.0 * m + 4.0 * Sqrt(9.0 * m * m + 4.0), 1 / 3.0) / 2.0;
            return 2.0 * Atan(x - 1.0 / x);
        }

        public override double GetJD(double trueAnomaly)
        {
            double tan1 = Tan(trueAnomaly / 2.0);
            double tan0 = Tan(Elements.TrueAnomaly / 2.0);

            return (tan1 - tan0 + tan1 * tan1 * tan1 * 3.0 - tan0 * tan0 * tan0 / 3.0) / MeanAngularVelocity /
                Date.JDtoSecond + Elements.JulianDate;
        }

        public override double GetTrueAnomaly(double julianDate)
        {
            double jd0 = GetJD(PI);
            double m = MeanAngularVelocity * (Elements.JulianDate - jd0) * Date.JDtoSecond; // mean anomaly
            double x = 0.5 * Pow(12.0 * m + 4.0 * Sqrt(9.0 * m * m + 4.0), 1 / 3.0);
            return 2.0 * Atan(x - 1.0 / x);
        }

        public ParabolicKeplerianElements Elements
        {
            get { return (ParabolicKeplerianElements)elements; }
        }
    }



    public class EllipticKeplerian : NotParabolicKeplerian
    {
        public EllipticKeplerian(double julianDate, double eccentricity, double semimajorAxis,
            double inclination, double ascendingNodeLongitude, double periapsisArgument,
            double trueAnomaly, Body centralBody) :
            base(julianDate, eccentricity, semimajorAxis, inclination, ascendingNodeLongitude, periapsisArgument,
            trueAnomaly, centralBody)
        {

        }

        public EllipticKeplerian(NotParabolicKeplerianElements elements) : base(elements)
        {

        }

        public EllipticKeplerian(ParabolicKeplerianElements elements) : base(elements)
        {

        }

        public EllipticKeplerian(NotParabolicKeplerian ke) : base(ke)
        {

        }

        public EllipticKeplerian(ParabolicKeplerian ke) : base(ke)
        {

        }



        protected override Complex MeanAngularVelocity
        {
            get
            {
                return Sqrt(G * Elements.CentralBody.Mass / Elements.SemimajorAxis /
                    Elements.SemimajorAxis / Elements.SemimajorAxis);
            }
        }

        protected override Complex GetEccentricAnomaly(double trueAnomaly)
        {
            if (Elements.Eccentricity == 0.0)
            {
                return Elements.TrueAnomaly;
            }

            double eccentricAnomaly = 2.0 * Atan(Tan(Elements.TrueAnomaly / 2.0) *
                Sqrt((1.0 - Elements.Eccentricity) / (1.0 + Elements.Eccentricity)));

            if (Sin(Elements.TrueAnomaly) < 0)
            {
                eccentricAnomaly = -eccentricAnomaly;
            }

            return eccentricAnomaly;
        }

        protected override Complex GetMeanAnomaly(double trueAnomaly)
        {
            double eccentricAnomaly = GetEccentricAnomaly(trueAnomaly).Real;
            return eccentricAnomaly - Elements.Eccentricity * Sin(eccentricAnomaly);
        }

        protected override double GetTrueAnomaly(Complex meanAnomaly)
        {
            double eccentricAnomaly = meanAnomaly.Real;

            for (int i = 0; i < ITERATION_CONSTANT; i++)
            {
                eccentricAnomaly = Elements.Eccentricity * Sin(eccentricAnomaly) + meanAnomaly.Real;
            }

            double trueAnomaly = Acos((Cos(eccentricAnomaly) - Elements.Eccentricity) /
                (1 - Elements.Eccentricity * Cos(eccentricAnomaly)));

            if (eccentricAnomaly < 0)
            {
                trueAnomaly = -trueAnomaly;
            }

            return trueAnomaly;
        }

        public override double GetTrueAnomaly(double julianDate)
        {
            double meanAnomaly = GetMeanAnomaly(Elements.TrueAnomaly).Real +
                MeanAngularVelocity.Real * (julianDate - Elements.JulianDate) * Date.JDtoSecond;
            return GetTrueAnomaly(new Complex(meanAnomaly, 0.0));
        }

        public override double GetJD(double trueAnomaly)
        {
            double timeInterval = (GetMeanAnomaly(trueAnomaly).Real -
                GetMeanAnomaly(Elements.TrueAnomaly).Real) / MeanAngularVelocity.Real;

            if (timeInterval < 0)
            {
                timeInterval += OrbitalPeriod;
            }

            return timeInterval / Date.JDtoSecond + Elements.JulianDate;
        }

        protected double OrbitalPeriod
        {
            get
            {
                return 2 * PI / MeanAngularVelocity.Real;
            }
        }
    }

    public class HyperbolicKeplerian : NotParabolicKeplerian
    {
        public HyperbolicKeplerian(double julianDate, double eccentricity, double semimajorAxis,
            double inclination, double ascendingNodeLongitude, double periapsisArgument,
            double trueAnomaly, Body centralBody) :
            base(julianDate, eccentricity, semimajorAxis, inclination, ascendingNodeLongitude, periapsisArgument,
            trueAnomaly, centralBody)
        {

        }

        public HyperbolicKeplerian(NotParabolicKeplerianElements ke) : base(ke)
        {

        }

        public HyperbolicKeplerian(ParabolicKeplerianElements ke) : base(ke)
        {

        }

        public HyperbolicKeplerian(ParabolicKeplerian keplerian) : base(keplerian)
        {

        }

        public HyperbolicKeplerian(NotParabolicKeplerian keplerian) : base(keplerian)
        {

        }



        protected override Complex MeanAngularVelocity
        {
            get
            {
                return Sqrt(-G * Elements.CentralBody.Mass / Elements.SemimajorAxis /
                    Elements.SemimajorAxis / Elements.SemimajorAxis);
            }
        }

        protected override Complex GetEccentricAnomaly(double trueAnomaly)
        {
            Complex Arth(Complex z)
            {
                return Log((1 + z) / (1 - z)) / 2.0;
            }

            Complex eccentricAnomaly = 2.0 * Arth(Tan(Elements.TrueAnomaly / 2.0) *
                Sqrt((Elements.Eccentricity + 1.0) / (Elements.Eccentricity - 1.0)));

            if (Sin(Elements.TrueAnomaly) < 0)
            {
                eccentricAnomaly = -eccentricAnomaly;
            }

            return eccentricAnomaly;
        }

        protected override Complex GetMeanAnomaly(double trueAnomaly)
        {
            Complex eccentricAnomaly = GetEccentricAnomaly(trueAnomaly);
            return Elements.Eccentricity * Sinh(eccentricAnomaly) - eccentricAnomaly;
        }

        protected override double GetTrueAnomaly(Complex meanAnomaly)
        {
            Complex eccentricAnomaly = meanAnomaly;

            for (int i = 0; i < ITERATION_CONSTANT; i++)
            {
                eccentricAnomaly = Elements.Eccentricity * Sinh(eccentricAnomaly) - meanAnomaly;
            }

            double trueAnomaly = 2.0 * Atan(Tanh(eccentricAnomaly / 2.0) *
                Sqrt((Elements.Eccentricity + 1.0)/(Elements.Eccentricity - 1.0))).Real;

            if (eccentricAnomaly.Real < 0.0)
            {
                trueAnomaly = -trueAnomaly;
            }

            return trueAnomaly;
        }

        public override double GetTrueAnomaly(double julianDate)
        {
            Complex meanAnomaly = GetMeanAnomaly(Elements.TrueAnomaly) + MeanAngularVelocity *
                (julianDate - Elements.JulianDate) * Date.JDtoSecond;
            return GetTrueAnomaly(meanAnomaly);
        }

        public override double GetJD(double trueAnomaly)
        {
            if(Cos(trueAnomaly) * Elements.Eccentricity > 1.0)
            {
                throw new Exception("True anomaly isn't correct");
            }

            return ((GetMeanAnomaly(trueAnomaly) - GetMeanAnomaly(Elements.TrueAnomaly)) /
                MeanAngularVelocity).Real / Date.JDtoSecond + Elements.JulianDate;
        }
    }
}
