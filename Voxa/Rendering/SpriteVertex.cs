using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;

namespace Voxa.Rendering
{
    public struct SpriteVertex
    {
        public const int Size = (3 + 2 + 4) * 4; // Size of struct in bytes

        public Vector3 Position;
        public Color4 Color;
        public Vector2 TexCoord;

        public SpriteVertex(Vector3 position, Color4 color, Vector2 texCoord)
        {
            this.Position = position;
            this.Color = color;
            this.TexCoord = texCoord;
        }

        public SpriteVertex(Vector3 position, Vector2 texCoord)
        {
            this.Position = position;
            this.Color = Color4.White;
            this.TexCoord = texCoord;
        }

        public override string ToString()
        {
            return $"Pos: {this.Position.ToString()} Color: {this.Color.ToString()} Texture Coord: {this.TexCoord.ToString()}";
        }
    }
}
