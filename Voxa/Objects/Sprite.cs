using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxa.Rendering;
using OpenTK;

namespace Voxa.Objects
{
    public class Sprite
    {
        public Texture Texture;
        public Vector2 Position;
        public int     Order;
        public float   Size;

        public Sprite()
        {}

        public Sprite(Texture texture)
        {
            this.Texture = texture;
            this.Position = Vector2.Zero;
            this.Size = 1;
            this.Order = 1;
        }

        public Sprite(Texture texture, Vector2 position)
        {
            this.Texture = texture;
            this.Position = position;
            this.Size = 1;
            this.Order = 1;
        }

        public Sprite(Texture texture, Vector2 position, float size)
        {
            this.Texture = texture;
            this.Position = position;
            this.Size = size;
            this.Order = 1;
        }

        public Sprite(Texture texture, Vector2 position, float size, int order)
        {
            this.Texture = texture;
            this.Position = position;
            this.Size = size;
            this.Order = order;
        }
    }
}
