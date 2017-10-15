using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using Voxa.Rendering.Uniforms;

namespace Voxa.Objects
{
    public struct AmbientLight
    {
        public Color4 Color;
        public float Strength;

        public AmbientLight(Color4 color, float strength)
        {
            this.Color = color;
            this.Strength = strength;
        }
    }

    public class Light : Component
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
            LightUniform currentLightUniform = Engine.UniformManager.GetUniform<LightUniform>("light");
            currentLightUniform.Color = new Vector3(this.Color.R, this.Color.G, this.Color.B);
            currentLightUniform.Position = this.gameObject.Transform.Position;
            currentLightUniform.Set(Engine.RenderingPool.PhongShaderProgram);
        }
    }
}
