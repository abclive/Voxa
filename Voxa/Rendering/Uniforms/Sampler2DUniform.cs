using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace Voxa.Rendering.Uniforms
{
    sealed class Sampler2DUniform : IUniform
    {
        private readonly string name;
        public int SamplerId;

        public Sampler2DUniform(string name, int SamplerId = 0)
        {
            this.name = name;
            this.SamplerId = SamplerId;
        }

        public void Set(ShaderProgram program)
        {
            // Get uniform location
            var i = program.GetUniformLocation(this.name);

            // Set uniform value
            GL.Uniform1(i, this.SamplerId);
        }

        public string GetName()
        {
            return this.name;
        }
    }
}
