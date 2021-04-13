using System;
using System.Collections.Generic;
using System.Text;
using CoordinateSystems;
using static CoordinateSystems.Coordinates;

namespace SunSystem
{
    public class Earth : Body
    {
        public override bool IsInside(Vector point)
        {
            return vector.X * vector.X / EARTH_MAX_RADIUS / EARTH_MAX_RADIUS +
            vector.Y * vector.Y / EARTH_MAX_RADIUS / EARTH_MAX_RADIUS +
            vector.Z * vector.Z / EARTH_MIN_RADIUS / EARTH_MIN_RADIUS < 1.0;
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
		    return vector1 - direction* Vector.MultiplyScalar((vector1 - point), direction)
                / direction.Length / direction.Length;
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
    }
}
