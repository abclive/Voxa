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
    sealed class Vector4Uniform
    {
        private readonly string name;
        public Vector4 Value;

        public Vector4Uniform(string name, Vector4 value)
        {
            this.name = name;
            this.Value = value;
        }

        public Vector4Uniform(string name, Color4 value)
        {
            this.name = name;
            this.Value = new Vector4(value.R, value.G, value.B, value.A);
        }

        public void Set(ShaderProgram program)
        {
            // Get uniform location
            var i = program.GetUniformLocation(this.name);

            // Set uniform value
            GL.Uniform4(i, this.Value);
        }
    }
}
