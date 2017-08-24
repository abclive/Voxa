using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Voxa.Rendering
{
    sealed class VertexArray<TVertex> where TVertex : struct
    {
        private readonly int handle;
        private bool         vertexArrayGenerated;
        private bool         disposed = false;

        public VertexArray(VertexBuffer<TVertex> vertexBuffer, ShaderProgram program, params VertexAttribute[] attributes)
        {
            if (this.vertexArrayGenerated)
                GL.DeleteVertexArrays(1, ref this.handle);
            // Create new vertex array object
            GL.GenVertexArrays(1, out this.handle);
            this.vertexArrayGenerated = true;

            // Bind the object so we can modify it
            this.Bind();

            // Bind the vertex buffer object
            vertexBuffer.Bind();

            // Set all attributes
            foreach (var attribute in attributes)
                attribute.Set(program);

            // Unbind objects to reset state
            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        public void Bind()
        {
            // Bind for usage (modification or rendering)
            GL.BindVertexArray(this.handle);
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

            if (this.vertexArrayGenerated)
                GL.DeleteVertexArray(this.handle);

            this.disposed = true;
        }

        ~VertexArray()
        {
            this.dispose(false);
        }
    }
}
