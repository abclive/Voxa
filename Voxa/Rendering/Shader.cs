using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Voxa.Rendering
{
    public class Shader
    {
        private readonly int handle;
        private bool         disposed = false;

        public int           Handle { get { return this.handle; } }

        public Shader(ShaderType type, string code)
        {
            // Create shader object
            this.handle = GL.CreateShader(type);

            // Set source and compile shader
            GL.ShaderSource(this.handle, code);
            GL.CompileShader(this.handle);
        }

        public void Dispose()
        {
            this.dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void dispose(bool disposing)
        {
            if (this.disposed)
                return;

            if (GraphicsContext.CurrentContext == null || GraphicsContext.CurrentContext.IsDisposed)
                return;

            GL.DeleteShader(this.handle);

            this.disposed = true;
        }

        ~Shader()
        {
            this.dispose(false);
        }
    }
}
