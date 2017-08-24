using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;

namespace Voxa.Rendering
{
    struct ColouredVertex
    {
        public const int Size = (3 + 4 + 3) * 4; // Size of struct in bytes

        public Vector3 Position;
        public Color4  Color;
        public Vector3 Normal;

        public ColouredVertex(Vector3 position, Color4 color, Vector3 normal)
        {
            this.Position = position;
            this.Color = color;
            this.Normal = normal;
        }

        public ColouredVertex(Vector3 position, Color4 color)
        {
            this.Position = position;
            this.Color = color;
            this.Normal = Vector3.Zero;
        }
    }
}
