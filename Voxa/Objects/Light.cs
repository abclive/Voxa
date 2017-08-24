using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;

namespace Voxa.Objects
{
    struct AmbientLight
    {
        public Color4 Color;
        public float Strength;

        public AmbientLight(Color4 color, float strength)
        {
            this.Color = color;
            this.Strength = strength;
        }
    }

    class Light : Component
    {
        public Color4 Color;

        public Light(Color4 color)
        {
            this.Color = color;
        }

        public Light()
        {
            this.Color = Color4.White;
        }

        public void Bind()
        {
            Engine.RenderingPool.CurrentLightUniform.Color = new Vector3(this.Color.R, this.Color.G, this.Color.B);
            Engine.RenderingPool.CurrentLightUniform.Position = this.gameObject.Transform.Position;
            Engine.RenderingPool.CurrentLightUniform.Set(Engine.RenderingPool.ShaderProgam);
        }
    }
}
