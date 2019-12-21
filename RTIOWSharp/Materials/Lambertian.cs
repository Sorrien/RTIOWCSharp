using System.Numerics;

namespace RTIOWSharp.Materials
{
    public class Lambertian : IMaterial
    {
        public Vector3 Albedo { get; set; }
        public Lambertian(Vector3 albedo)
        {
            Albedo = albedo;
        }

        public bool Scatter(Ray rIn, HitRecord hitRecord, out Vector3 attenuation, out Ray scattered)
        {
            var target = hitRecord.P + hitRecord.Normal + HelperFunctions.RandomInUnitSphere();
            scattered = new Ray(hitRecord.P, target - hitRecord.P);
            attenuation = Albedo;
            return true;
        }
    }
}
