using System;
using System.Numerics;

namespace RTIOWSharp
{
    public class Sphere : IHitable
    {
        public Vector3 Center { get; set; }
        public float Radius { get; set; }
        public Sphere(Vector3 center, float radius)
        {
            Center = center;
            Radius = radius;
        }
        public bool Hit(Ray r, float t_min, float t_max, out HitRecord hitRecord)
        {
            var oc = r.Origin() - Center;
            var a = Vector3.Dot(r.Direction(), r.Direction());
            var b = Vector3.Dot(oc, r.Direction());
            var c = Vector3.Dot(oc, oc) - Radius * Radius;
            var discriminant = b * b - a * c;
            var didHit = false;
            hitRecord = new HitRecord();
            if (discriminant > 0)
            {
                var temp = (-b - MathF.Sqrt(b * b - a * c)) / a;
                if (temp < t_max && temp > t_min)
                {
                    hitRecord.t = temp;
                    hitRecord.p = r.PointAtParameter(hitRecord.t);
                    hitRecord.normal = (hitRecord.p - Center) / Radius;
                    didHit = true;
                }
                else
                {
                    temp = (-b + MathF.Sqrt(b * b - a * c)) / a;
                    if (temp < t_max && temp > t_min)
                    {
                        hitRecord.t = temp;
                        hitRecord.p = r.PointAtParameter(hitRecord.t);
                        hitRecord.normal = (hitRecord.p - Center) / Radius;
                        didHit = true;
                    }
                }
            }
            return didHit;
        }
    }
}
