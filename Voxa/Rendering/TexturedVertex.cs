using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;

namespace Voxa.Rendering
{
    struct TexturedVertex
    {
        public const int Size = (3 + 2 + 4 + 3) * 4; // Size of struct in bytes

        public Vector3 Position;
        public Color4 Color;
        public Vector2 TextureCoord;
        public Vector3 Normal;

        public TexturedVertex(Vector3 position, float texU, float texV, Color4 color, Vector3 normal)
        {
            this.Position = position;
            this.TextureCoord = new Vector2(texU, texV);
            this.Color = color;
            this.Normal = normal;
        }

        public TexturedVertex(Vector3 position, Vector2 texCoord, Color4 color, Vector3 normal)
        {
            this.Position = position;
            this.TextureCoord = texCoord;
            this.Color = color;
            this.Normal = normal;
        }

        public TexturedVertex(Vector3 position, Color4 color, Vector3 normal)
        {
            this.Position = position;
            this.TextureCoord = new Vector2(0, 0);
            this.Color = color;
            this.Normal = normal;
        }

        public TexturedVertex(Vector3 position, Color4 color)
        {
            this.Position = position;
            this.TextureCoord = new Vector2(0, 0);
            this.Color = color;
            this.Normal = Vector3.Zero;
        }

        public override string ToString()
        {
            return $"Pos: {this.Position.ToString()} U: {this.TextureCoord.X} V: {this.TextureCoord.Y} Color: {this.Color.ToString()} Normal: {this.Normal.ToString()}";
        }
    }
}
