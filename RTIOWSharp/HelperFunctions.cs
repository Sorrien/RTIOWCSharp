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

        public static Vector3 Reflect(Vector3 v, Vector3 n)
        {
            return v - 2 * Vector3.Dot(v, n) * n;
        }

        public static bool Refract(Vector3 v, Vector3 n, float niOverNt, out Vector3 refracted)
        {
            var uv = UnitVector(v);
            var dt = Vector3.Dot(uv, n);
            var discriminant = 1.0f - niOverNt * niOverNt * (1 - dt * dt);
            if(discriminant > 0)
            {
                refracted = niOverNt * (uv - n * dt) - n * MathF.Sqrt(discriminant);
                return true;
            }
            else
            {
                refracted = Vector3.Zero;
                return false;
            }
        }

        public static float Schlick(float cosine, float refIdx)
        {
            var r0 = (1 - refIdx) / (1 + refIdx);
            r0 = r0 * r0;
            return r0 + (1 - r0) * MathF.Pow((1 - cosine), 5);
        }
    }
}
