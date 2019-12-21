using System.Collections.Generic;

namespace RTIOWSharp
{
    public class World : IHitable
    {
        public List<IHitable> Hitables { get; set; }
        public World(List<IHitable> hitables)
        {
            Hitables = hitables;
        }

        public bool Hit(Ray r, float t_min, float t_max, out HitRecord hitRecord)
        {
            HitRecord temp;
            var hitAnything = false;
            var closestSoFar = t_max;
            hitRecord = null;
            for(var i=0;i< Hitables.Count;i++)
            {
                if(Hitables[i].Hit(r, t_min, closestSoFar, out temp))
                {
                    hitAnything = true;
                    closestSoFar = temp.T;
                    hitRecord = temp;                   
                }
            }
            return hitAnything;
        }
    }
}
