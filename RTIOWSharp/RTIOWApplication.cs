using RTIOWSharp.Materials;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Threading.Tasks;

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

            var world = CreateBookWorld();

            var lookFrom = new Vector3(9.5f, 2f, 2.5f);
            var lookAt = new Vector3(3f, 0.5f, 0.65f);
            var distanceToFocus = (lookFrom - lookAt).Length();
            var aperture = 0.01f;
            var camera = new Camera(lookFrom, lookAt, new Vector3(0f, 1f, 0f), 25f, (float)nx / (float)ny, aperture, distanceToFocus);

            var colors = RenderParallel(nx, ny, ns, camera, world);

            var colorArray = MapListToMultiArray(nx, ny, colors);

            var bmp = new Bitmap(nx, ny);
            MultiArrayToBitmap(colorArray, ref bmp);
            bmp.Save("testimage.png");
        }

        public World CreateWorld()
        {
            var hitables = new List<IHitable>();
            hitables.Add(new Sphere(new Vector3(0.0f, 0.0f, -1.0f), 0.5f, new Lambertian(new Vector3(0.8f, 0.3f, 0.3f))));
            hitables.Add(new Sphere(new Vector3(0.0f, -100.5f, -1.0f), 100.0f, new Lambertian(new Vector3(0.8f, 0.8f, 0.0f))));
            hitables.Add(new Sphere(new Vector3(1.0f, 0.0f, -1.0f), 0.5f, new Metal(new Vector3(0.8f, 0.6f, 0.2f), 0.3f)));
            hitables.Add(new Sphere(new Vector3(-1.0f, 0.0f, -1.0f), 0.5f, new Dielectric(1.5f)));
            hitables.Add(new Sphere(new Vector3(-1.0f, 0.0f, -1.0f), -0.45f, new Dielectric(1.5f)));
            var world = new World(hitables);
            return world;
        }

        public World CreateBookWorld()
        {
            var hitables = new List<IHitable>();
            hitables.Add(new Sphere(new Vector3(0f, -1000f, 0f), 1000f, new Lambertian(new Vector3(0.5f, 0.5f, 0.5f))));
            var rand = new Random();
            for (var a = -11; a < 11; a++)
            {
                for (var b = -11; b < 11; b++)
                {
                    var chooseMat = (float)rand.NextDouble();
                    var center = new Vector3(a + 0.9f * (float)rand.NextDouble(), 0.2f, b + 0.9f * (float)rand.NextDouble());
                    if((center-new Vector3(4f,0.2f,0f)).Length() > 0.9f)
                    {
                        IMaterial material;
                        if(chooseMat < 0.8f) //diffuse
                        {
                            material = new Lambertian(new Vector3((float)(rand.NextDouble() * rand.NextDouble()), (float)(rand.NextDouble() * rand.NextDouble()), (float)(rand.NextDouble() * rand.NextDouble())));
                        }
                        else if(chooseMat < 0.95) //metal
                        {
                            material = new Metal(new Vector3(0.5f * (float)(1f + rand.NextDouble()), 0.5f * (float)(1 + rand.NextDouble()), 0.5f * (float)(1 + rand.NextDouble())), 0.5f * (float)(1 + rand.NextDouble()));
                        }
                        else //glass
                        {
                            material = new Dielectric(1.5f);
                        }

                        hitables.Add(new Sphere(center, 0.2f, material));
                    }
                }
            }
            hitables.Add(new Sphere(new Vector3(0f, 1f, 0f), 1.0f, new Dielectric(1.5f)));
            hitables.Add(new Sphere(new Vector3(-4.0f, 1, 0), 1.0f, new Lambertian(new Vector3(0.4f, 0.2f, 0.1f))));
            hitables.Add(new Sphere(new Vector3(4f, 1f, 0f), 1.0f, new Metal(new Vector3(0.7f, 0.6f, 0.5f), 0.0f)));
            var world = new World(hitables);
            return world;
        }

        public List<Color> RenderParallel(int width, int height, int ns, Camera camera, World world)
        {
            var colorMulti = new Color[height, width];

            Parallel.ForEach(CreateIterator(height - 1, 0), j =>
               {
                   var rand = new Random();
                   for (var i = 0; i < width; i++)
                   {
                       var col = new Vector3();
                       for (var s = 0; s < ns; s++)
                       {
                           //random float greater than 0 and less than 1
                           var u = (float)(i + rand.NextDouble()) / width;
                           var v = (float)(j + rand.NextDouble()) / height;
                           var ray = camera.GetRay(u, v);
                           col += GetColor(ray, world, 0);
                       }
                       col /= ns;
                       col = new Vector3(MathF.Sqrt(col.X), MathF.Sqrt(col.Y), MathF.Sqrt(col.Z));
                       var ir = (int)(255.99 * col.X);
                       var ig = (int)(255.99 * col.Y);
                       var ib = (int)(255.99 * col.Z);
                       var color = Color.FromArgb(ir, ig, ib);
                       colorMulti[j, i] = color;
                   }
               });

            var colors = new List<Color>();
            for (var j = height - 1; j >= 0; j--)
            {
                for (var i = 0; i < width; i++)
                {
                    colors.Add(colorMulti[j, i]);
                }
            }

            return colors;
        }

        private int[] CreateIterator(int start, int end)
        {
            var result = new int[Math.Abs(start - end)];

            var index = 0;
            var value = start;
            if (start < end)
            {
                while (index <= result.Length && value < end)
                {
                    result[index] = value;
                    value++;
                    index++;
                }
            }
            else
            {
                while (index <= result.Length && value > end)
                {
                    result[index] = value;
                    value--;
                    index++;
                }
            }

            return result;
        }

        public List<Color> Render(int width, int height, int ns, Camera camera, World world)
        {
            var colors = new List<Color>();
            var rand = new Random();
            for (var j = height - 1; j >= 0; j--)
            {
                for (var i = 0; i < width; i++)
                {
                    var col = new Vector3();
                    for (var s = 0; s < ns; s++)
                    {
                        //random float greater than 0 and less than 1
                        var u = (float)(i + rand.NextDouble()) / width;
                        var v = (float)(j + rand.NextDouble()) / height;
                        var ray = camera.GetRay(u, v);
                        col += GetColor(ray, world, 0);
                    }
                    col /= ns;
                    col = new Vector3(MathF.Sqrt(col.X), MathF.Sqrt(col.Y), MathF.Sqrt(col.Z));
                    var ir = (int)(255.99 * col.X);
                    var ig = (int)(255.99 * col.Y);
                    var ib = (int)(255.99 * col.Z);
                    var color = Color.FromArgb(ir, ig, ib);
                    colors.Add(color);
                }
            }

            return colors;
        }

        public Vector3 GetColor(Ray r, World world, int depth)
        {
            HitRecord hitRecord;
            if (world.Hit(r, 0.001f, float.MaxValue, out hitRecord))
            {
                Ray scattered;
                Vector3 attenuation;
                var maxDepth = 50; //50
                if (depth < maxDepth && hitRecord.Material.Scatter(r, hitRecord, out attenuation, out scattered))
                {
                    return attenuation * GetColor(scattered, world, depth + 1);
                }
                else
                {
                    return new Vector3(0.0f, 0.0f, 0.0f);
                }
            }
            else
            {
                var unitDirection = HelperFunctions.UnitVector(r.Direction());
                var t = 0.5f * (unitDirection.Y + 1.0f);
                return (1.0f - t) * new Vector3(1.0f, 1.0f, 1.0f) + t * new Vector3(0.5f, 0.7f, 1.0f);
            }
        }

        public Color[,] MapListToMultiArray(int width, int height, List<Color> colors)
        {
            var colorArray = new Color[height, width];

            var colorIndex = 0;
            for (var j = 0; j < height; j++)
            {
                for (var i = 0; i < width; i++)
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

            for (var j = 0; j < height; j++)
            {
                for (var i = 0; i < width; i++)
                {
                    var color = colors[j, i];
                    bmp.SetPixel(i, j, color);
                }
            }
        }
    }
}
