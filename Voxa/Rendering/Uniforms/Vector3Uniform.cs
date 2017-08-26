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
    sealed class Vector3Uniform
    {
        private readonly string name;
        public Vector3 Value;

        public Vector3Uniform(string name, Vector3 value)
        {
            this.name = name;
            this.Value = value;
        }

        public Vector3Uniform(string name, Color4 value)
        {
            this.name = name;
            this.Value = new Vector3(value.R, value.G, value.B);
        }

        public Vector3Uniform(string name)
        {
            this.name = name;
        }

        public void Set(ShaderProgram program)
        {
            // Get uniform location
            var i = program.GetUniformLocation(this.name);

            // Set uniform value
            GL.Uniform3(i, this.Value);
        }
    }
}

