using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;

namespace RTIOWSharp
{
    public class RTIOWApplication
    {
        public RTIOWApplication()
        {

        }

        public void Run()
        {
            var nx = 200;
            var ny = 100;
            var ns = 100;
            var bmp = new Bitmap(nx, ny);

            var colors = Render(nx, ny, ns);

            var colorArray = MapListToMultiArray(nx, ny, colors);

            MultiArrayToBitmap(colorArray, ref bmp);

            bmp.Save("testimage.png");
        }

        public List<Color> Render(int width, int height, int ns)
        {
            var colors = new List<Color>();
            var hitables = new List<IHitable>();
            hitables.Add(new Sphere(new Vector3(0.0f, 0.0f, -1.0f), 0.5f));
            hitables.Add(new Sphere(new Vector3(0.0f, -100.5f, -1.0f), 100));
            var camera = new Camera();
            var world = new World(hitables);
            var rand = new Random();
            for (int j = height - 1; j >= 0; j--)
            {
                for (int i = 0; i < width; i++)
                {
                    var col = new Vector3();
                    for (int s = 0; s < ns; s++)
                    {
                        //random float greater than 0 and less than 1
                        var u = (float)(i + rand.NextDouble()) / width;
                        var v = (float)(j + rand.NextDouble()) / height;
                        var ray = camera.GetRay(u, v);
                        col += GetColor(ray, world);
                    }
                    col /= ns;
                    var ir = (int)(255.99 * col.X);
                    var ig = (int)(255.99 * col.Y);
                    var ib = (int)(255.99 * col.Z);
                    var color = Color.FromArgb(ir, ig, ib);
                    colors.Add(color);
                }
            }

            return colors;
        }

        public Vector3 GetColor(Ray r, World world)
        {
            HitRecord hitRecord;
            if (world.Hit(r, 0.0f, float.MaxValue, out hitRecord))
            {
                return 0.5f * new Vector3(hitRecord.normal.X + 1.0f, hitRecord.normal.Y + 1.0f, hitRecord.normal.Z + 1.0f);
            }
            else
            {
                var unitDirection = UnitVector(r.Direction());
                float t = 0.5f * (unitDirection.Y + 1.0f);
                return (1.0f - t) * new Vector3(1.0f, 1.0f, 1.0f) + t * new Vector3(0.5f, 0.7f, 1.0f);
            }
        }

        public Vector3 UnitVector(Vector3 vector)
        {
            return vector / vector.Length();
        }

        public Color[,] MapListToMultiArray(int width, int height, List<Color> colors)
        {
            var colorArray = new Color[height, width];

            var colorIndex = 0;
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    var color = colors[colorIndex];
                    colorArray[j, i] = color;
                    colorIndex++;
                }
            }

            return colorArray;
        }

        public void MultiArrayToBitmap(Color[,] colors, ref Bitmap bmp)
        {
            var height = colors.GetLength(0);
            var width = colors.GetLength(1);

            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    var color = colors[j, i];
                    bmp.SetPixel(i, j, color);
                }
            }
        }
    }
}
