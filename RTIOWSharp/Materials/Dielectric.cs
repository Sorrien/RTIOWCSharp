using System;
using System.Numerics;

namespace RTIOWSharp.Materials
{
    public class Dielectric : IMaterial
    {
        float RefIdx { get; set; }
        public Dielectric(float ri)
        {
            RefIdx = ri;
        }

        public bool Scatter(Ray rIn, HitRecord hitRecord, out Vector3 attenuation, out Ray scattered)
        {
            Vector3 outwardNormal;
            var reflected = HelperFunctions.Reflect(rIn.Direction(), hitRecord.Normal);
            float niOverNt;
            attenuation = new Vector3(1.0f, 1.0f, 1.0f);
            float cosine;
            if(Vector3.Dot(rIn.Direction(), hitRecord.Normal) > 0)
            {
                outwardNormal = -hitRecord.Normal;
                niOverNt = RefIdx;
                cosine = RefIdx * Vector3.Dot(rIn.Direction(), hitRecord.Normal) / rIn.Direction().Length();
            }
            else
            {
                outwardNormal = hitRecord.Normal;
                niOverNt = 1.0f / RefIdx;
                cosine = -Vector3.Dot(rIn.Direction(), hitRecord.Normal) / rIn.Direction().Length();
            }
            float reflectProb;
            if(HelperFunctions.Refract(rIn.Direction(), outwardNormal, niOverNt, out Vector3 refracted))
            {
                reflectProb = HelperFunctions.Schlick(cosine, RefIdx);
            }
            else
            {
                reflectProb = 1.0f;
                //scattered = new Ray(hitRecord.P, reflected);
            }
            var random = new Random();
            if(random.NextDouble() < reflectProb)
            {
                scattered = new Ray(hitRecord.P, reflected);
            }
            else
            {
                scattered = new Ray(hitRecord.P, refracted);
            }
            return true;
        }
    }
}
