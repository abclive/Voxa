﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using OpenTK;

namespace Voxa.Utils
{
    public static class Extensions
    {
        public static T[] SubArray<T>(this T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }

        public static float Distance(this Vector3 vec, Vector3 target)
        {
            float deltaX = target.X - vec.X;
            float deltaY = target.Y - vec.Y;
            float deltaZ = target.Z - vec.Z;

            return (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ);
        }

        public static float Distance(this Vector2 vec, Vector2 target)
        {
            float deltaX = target.X - vec.X;
            float deltaY = target.Y - vec.Y;

            return (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
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

        public static Color Blend(this Color origin, Color mixColor, double amount = 0.5)
        {
            byte r = (byte)((origin.R * amount) + mixColor.R * (1 - amount));
            byte g = (byte)((origin.G * amount) + mixColor.G * (1 - amount));
            byte b = (byte)((origin.B * amount) + mixColor.B * (1 - amount));
            return Color.FromArgb(r, g, b);
        }

        public static Vector3 Lerp(this Vector3 origin, Vector3 target, float fraction)
        {
            origin.X = origin.X.Lerp(target.X, fraction);
            origin.Y = origin.Y.Lerp(target.Y, fraction);
            origin.Z = origin.Z.Lerp(target.Z, fraction);

            return origin;
        }

        public static float Lerp(this float origin, float target, float fraction)
        {
            return (origin * (1f - fraction)) + (target * fraction);
        }
    }
}
