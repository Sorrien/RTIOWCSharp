using RTIOWSharp.Materials;
using System.Numerics;

namespace RTIOWSharp
{
    public interface IHitable
    {
        bool Hit(Ray r, float t_min, float t_max, out HitRecord hitRecord);
    }

    public class HitRecord
    {
        public float T { get; set; }
        public Vector3 P { get; set; }
        public Vector3 Normal { get; set; }
        public IMaterial Material { get; set; }
    }
}
