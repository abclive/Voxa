using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;

namespace Voxa.Rendering
{
    public struct UnlitColouredVertex
    {
        public const int Size = (3 + 4) * 4; // Size of struct in bytes

        public Vector3 Position;
        public Color4 Color;

        public UnlitColouredVertex(Vector3 position, Color4 color)
        {
            this.Position = position;
            this.Color = color;
        }
    }
}
