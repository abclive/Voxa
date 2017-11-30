using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;

namespace Voxa.Rendering
{
    public struct TextVertex
    {
        public const int Size = 4 * 4; // Size of struct in bytes

        public Vector2 Position;
        public Vector2 TexCoord;

        public TextVertex(Vector2 position, Vector2 texCoord)
        {
            this.Position = position;
            this.TexCoord = texCoord;
        }

        public override string ToString()
        {
            return $"Pos: {this.Position.ToString()} Texture Coord: {this.TexCoord.ToString()}";
        }
    }
}
