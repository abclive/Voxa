using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Voxa.Rendering.Uniforms
{
    sealed class Matrix4Uniform : IUniform
    {
        private readonly string name;
        private Matrix4         matrix;

        public Matrix4          Matrix { get { return this.matrix; } set { this.matrix = value; } }

        public Matrix4Uniform(string name, Matrix4 matrix)
        {
            this.name = name;
            this.matrix = matrix;
        }

        public Matrix4Uniform(string name)
        {
            this.name = name;
        }

        public void Set(ShaderProgram program)
        {
            // Get uniform location
            var i = program.GetUniformLocation(this.name);

            // Set uniform value
            GL.UniformMatrix4(i, false, ref this.matrix);
        }

        public string GetName()
        {
            return this.name;
        }
    }
}
