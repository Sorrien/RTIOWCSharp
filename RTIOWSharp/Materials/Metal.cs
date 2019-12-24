using System.Numerics;

namespace RTIOWSharp.Materials
{
    public class Metal : IMaterial
    {
        Vector3 Albedo { get; set; }
        float Fuzz { get; set; }
        public Metal(Vector3 albedo, float f)
        {
            Albedo = albedo;
            if(f < 1)
            {
                Fuzz = f;
            }
            else
            {
                Fuzz = 1;
            }
        }

        public bool Scatter(Ray rIn, HitRecord hitRecord, out Vector3 attenuation, out Ray scattered)
        {
            var reflected = HelperFunctions.Reflect(HelperFunctions.UnitVector(rIn.Direction()), hitRecord.Normal);
            scattered = new Ray(hitRecord.P, reflected + Fuzz * HelperFunctions.RandomInUnitSphere());
            attenuation = Albedo;
            return Vector3.Dot(scattered.Direction(), hitRecord.Normal) > 0;
        }
    }
}
