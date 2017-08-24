using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Voxa.Rendering.Uniforms
{
    sealed class LightUniform
    {
        private readonly string name;
        public Vector3 Position;
        public Vector3 Color;

        public LightUniform(string name, Vector3 position, Color4 color)
        {
            this.name = name;
            this.Position = position;
            this.Color = new Vector3(color.R, color.G, color.B);
        }

        public LightUniform(string name, Vector3 position)
        {
            this.name = name;
            this.Position = position;
            this.Color = new Vector3(1, 1, 1);
        }

        public LightUniform(string name)
        {
            this.name = name;
        }

        public void Set(ShaderProgram program)
        {
            int i = program.GetUniformLocation(this.name + ".position");
            GL.Uniform3(i, this.Position);
            i = program.GetUniformLocation(this.name + ".color");
            GL.Uniform3(i, this.Color);
        }
    }
}
