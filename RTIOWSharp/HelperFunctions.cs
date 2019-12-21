using System;
using System.Numerics;

namespace RTIOWSharp
{
    public static class HelperFunctions
    {
        public static Vector3 RandomInUnitSphere()
        {
            var random = new Random();
            return new Vector3((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());
        }

        public static Vector3 UnitVector(Vector3 vector)
        {
            return vector / vector.Length();
        }
    }
}
