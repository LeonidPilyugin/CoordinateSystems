using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

namespace CoordinateSystems
{
    // класс представляет тело
    // физику надо считать для этого класса
    public class Body : CoordinateSystem
    {
        #region data
        // остальные тела
        protected static List<Body> bodies;
        #endregion

        #region constructors
        public Body(string ID, Vector vector, Basis basis, Velocity velocity, CoordinateSystem referenceSystem = null)
            : base(vector, basis, velocity, referenceSystem)
        {
            this.ID = ID;
        }

        public Body() : base()
        {
            this.ID = "New body";
        }

        public Body(Body body) : base(body.vector, body.basis, body.velocity, body.referenceSystem)
        {
            this.ID = body.ID;
        }

        public Body(string ID, CoordinateSystem coordinateSystem) :
            this(ID, coordinateSystem.Vector, coordinateSystem.Basis,
                coordinateSystem.Velocity, coordinateSystem.ReferenceSystem)
        {

        }

        static Body()
        {
            bodies = new List<Body>();
        }
        #endregion

        #region properties
        public static List<Body> Bodies
        {
            get
            {
                return bodies;
            }
        }

        public string ID { get; set; }
        #endregion

        #region functions
        public virtual bool IsInside(Vector point)
        {
            return false;
        }

        // возвращает true, если отрезок, соединяющий vector1 и vector2,
        // заданные в СК этого тела пересекает это тело
        public virtual bool IsCrossing(Vector vector1, Vector vector2)
        {
            return false;
        }
        #endregion
    }
}
