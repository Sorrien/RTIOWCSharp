using System.Numerics;

namespace RTIOWSharp.Materials
{
    public class Metal : IMaterial
    {
        Vector3 Albedo { get; set; }
        public Metal(Vector3 albedo)
        {
            Albedo = albedo;
        }

        public bool Scatter(Ray rIn, HitRecord hitRecord, out Vector3 attenuation, out Ray scattered)
        {
            var reflected = Reflect(HelperFunctions.UnitVector(rIn.Direction()), hitRecord.Normal);
            scattered = new Ray(hitRecord.P, reflected);
            attenuation = Albedo;
            return Vector3.Dot(scattered.Direction(), hitRecord.Normal) > 0;
        }

        private Vector3 Reflect(Vector3 v, Vector3 n)
        {
            return v - 2 * Vector3.Dot(v, n) * n;
        }
    }
}
