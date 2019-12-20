using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace RTIOWSharp
{
    public interface IHitable
    {
        bool Hit(Ray r, float t_min, float t_max, out HitRecord hitRecord);
    }

    public class HitRecord
    {
        public float t { get; set; }
        public Vector3 p { get; set; }
        public Vector3 normal { get; set; }
    }
}
