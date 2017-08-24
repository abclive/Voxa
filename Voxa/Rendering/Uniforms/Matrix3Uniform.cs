using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Voxa.Rendering.Uniforms
{
    sealed class Matrix3Uniform
    {
        private readonly string name;

        public Matrix3 Matrix;

        public Matrix3Uniform(string name)
        {
            this.name = name;
        }

        public Matrix3Uniform(string name, Matrix3 matrix)
        {
            this.name = name;
            this.Matrix = matrix;
        }

        public void Set(ShaderProgram program)
        {
            // Get uniform location
            var i = program.GetUniformLocation(this.name);

            // Set uniform value
            GL.UniformMatrix3(i, false, ref this.Matrix);
        }
    }
}
