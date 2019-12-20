using System.Numerics;

namespace RTIOWSharp
{
    public class Ray
    {
        public Vector3 A { get; set; }
        public Vector3 B { get; set; }

        public Ray()
        {

        }

        public Ray(Vector3 a, Vector3 b) { A = a; B = b; }

        public Vector3 Origin()
        {
            return A;
        }

        public Vector3 Direction()
        {
            return B;
        }

        public Vector3 PointAtParameter(float t)
        {
            return A + t * B;
        }
    }
}
