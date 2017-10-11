using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Voxa.Utils
{
    public static class MathExtensions
    {
        public static float Distance(this Vector3 vec, Vector3 target)
        {
            float deltaX = target.X - vec.X;
            float deltaY = target.Y - vec.Y;
            float deltaZ = target.Z - vec.Z;

            return (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ);
        }

        public static Vector2 GetSphereCoord(this Vector3 vec)
        {
            var len = vec.Length;
            Vector2 uv;
            uv.Y = (float)(Math.Acos(vec.Y / len) / Math.PI);
            uv.X = -(float)((Math.Atan2(vec.Z, vec.X) / Math.PI + 1.0f) * 0.5f);
            return uv;
        }

        public static float Range(this Random rand, float min, float max)
        {
            return (float)(min + rand.NextDouble() * (max - min));
        }
    }
}
