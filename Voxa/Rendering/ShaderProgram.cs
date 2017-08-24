using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Voxa.Rendering
{
    sealed class ShaderProgram
    {
        private readonly int                     handle;
        private bool                             disposed;
        private readonly Dictionary<string, int> attributeLocations = new Dictionary<string, int>();
        private readonly Dictionary<string, int> uniformLocations = new Dictionary<string, int>();

        public ShaderProgram(params Shader[] shaders)
        {
            // Create program object
            this.handle = GL.CreateProgram();

            // Assign all shaders
            foreach (var shader in shaders) {
                GL.AttachShader(this.handle, shader.Handle);
            }

            // Link program (effectively compiles it)
            GL.LinkProgram(this.handle);

            // Detach shaders
            foreach (var shader in shaders)
                GL.DetachShader(this.handle, shader.Handle);
        }

        public void Use()
        {
            // Activate this program to be used
            GL.UseProgram(this.handle);
        }

        public int GetAttributeLocation(string name)
        {
            int i;
            if (!this.attributeLocations.TryGetValue(name, out i))
            {
                i = GL.GetAttribLocation(this.handle, name);
                this.attributeLocations.Add(name, i);
            }
            return i;
        }

        public int GetUniformLocation(string name)
        {
            int i;
            if (!this.uniformLocations.TryGetValue(name, out i))
            {
                i = GL.GetUniformLocation(this.handle, name);
                this.uniformLocations.Add(name, i);
            }
            return i;
        }

        public void Dispose()
        {
            this.dispose(true);
            GC.SuppressFinalize(this);
        }

        private void dispose(bool disposing)
        {
            if (this.disposed)
                return;

            if (GraphicsContext.CurrentContext == null || GraphicsContext.CurrentContext.IsDisposed)
                return;

            GL.DeleteProgram(this.handle);

            this.disposed = true;
        }

        ~ShaderProgram()
        {
            this.dispose(false);
        }
    }
}
