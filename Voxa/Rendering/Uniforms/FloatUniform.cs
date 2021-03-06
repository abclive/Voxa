﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace Voxa.Rendering.Uniforms
{
    public sealed class FloatUniform : IUniform
    {
        private readonly string name;
        public float Value;

        public FloatUniform(string name, float value)
        {
            this.name = name;
            this.Value = value;
        }

        public void Set(ShaderProgram program)
        {
            // Get uniform location
            var i = program.GetUniformLocation(this.name);

            // Set uniform value
            GL.Uniform1(i, this.Value);
        }

        public string GetName()
        {
            return this.name;
        }
    }
}
