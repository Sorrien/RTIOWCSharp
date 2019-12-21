using System.Numerics;

namespace RTIOWSharp.Materials
{
    public interface IMaterial
    {
        bool Scatter(Ray rIn, HitRecord hitRecord, out Vector3 attenuation, out Ray scattered);
    }
}