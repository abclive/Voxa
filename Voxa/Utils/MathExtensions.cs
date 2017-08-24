using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Voxa.Utils
{
    static class MathExtensions
    {
        public static float Distance(this Vector3 vec, Vector3 target)
        {
            float deltaX = target.X - vec.X;
            float deltaY = target.Y - vec.Y;
            float deltaZ = target.Z - vec.Z;

            return (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ);
        }
    }
}
