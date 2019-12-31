using System;
using System.Numerics;

namespace RTIOWSharp
{
    public class Camera
    {
        public Vector3 LowerLeftCorner { get; set; }
        public Vector3 Horizontal { get; set; }
        public Vector3 Vertical { get; set; }
        public Vector3 Origin { get; set; }
        public float LensRadius { get; set; }
        private Vector3 u, v, w;

        public Camera(Vector3 lookFrom, Vector3 lookAt, Vector3 vUp, float verticalFov, float aspect, float aperture, float focusDistance)
        {
            LensRadius = aperture / 2;
            var theta = verticalFov * MathF.PI / 180;
            var halfHeight = MathF.Tan(theta / 2);
            var halfWidth = aspect * halfHeight;
            Origin = lookFrom;
            w = HelperFunctions.UnitVector(lookFrom - lookAt);
            u = HelperFunctions.UnitVector(Vector3.Cross(vUp, w));
            v = Vector3.Cross(w, u);
            LowerLeftCorner = Origin - halfWidth * focusDistance * u - halfHeight * focusDistance * v - focusDistance * w;
            Horizontal = 2 * halfWidth * focusDistance * u;
            Vertical = 2 * halfHeight * focusDistance * v;
        }

        public Ray GetRay(float s, float t)
        {
            var rd = LensRadius * RandomUnitInDisk();
            var offset = u * rd.X + v * rd.Y;
            return new Ray(Origin + offset, LowerLeftCorner + s * Horizontal + t * Vertical - Origin - offset);
        }

        private Vector3 RandomUnitInDisk()
        {
            var rand = new Random();
            var p = 2.0f * new Vector3((float)rand.NextDouble(), (float)rand.NextDouble(), 0f) - new Vector3(1f, 1f, 0);           
            while (Vector3.Dot(p, p) >= 1.0f)
            {
                p = 2.0f * new Vector3((float)rand.NextDouble(), (float)rand.NextDouble(), 0f) - new Vector3(1f, 1f, 0);
            }
            return p;
        }
    }
}
