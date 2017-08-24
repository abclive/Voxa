using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Voxa.Rendering
{
    sealed class VertexBuffer<TVertex> where TVertex : struct
    {
        private PrimitiveType primitiveType;
        private readonly int  handle;
        private readonly int  vertexSize;

        private TVertex[]     vertices;
        private ushort        count;
        private bool          disposed;

        public int            Handle { get { return this.handle; } }
        public int            VertexSize { get { return this.vertexSize; } }
        public ushort         Count { get { return this.count; } }
        public int            Capacity { get { return this.vertices.Length; } }

        public VertexBuffer(int vertexSize, PrimitiveType primitiveType = PrimitiveType.Triangles, int capacity = 0)
        {
            this.primitiveType = primitiveType;
            this.vertexSize = vertexSize;
            this.vertices = new TVertex[capacity > 0 ? capacity : 4];
            this.handle = GL.GenBuffer();
        }

        public void AddVertex(TVertex v)
        {
            if (this.count == this.vertices.Length)
                Array.Resize(ref this.vertices, this.count * 2);

            this.vertices[count] = v;
            this.count++;
        }

        public ushort AddVertices(TVertex vertex0, TVertex vertex1)
        {
            ushort oldCount = this.count;
            int newCount = oldCount + 2;
            this.ensureCapacity(newCount);
            this.count = (ushort)newCount;

            this.vertices[oldCount] = vertex0;
            this.vertices[oldCount + 1] = vertex1;

            return oldCount;
        }

        public ushort AddVertices(TVertex vertex0, TVertex vertex1, TVertex vertex2)
        {
            ushort oldCount = this.count;
            int newCount = oldCount + 3;
            this.ensureCapacity(newCount);
            this.count = (ushort)newCount;

            this.vertices[oldCount] = vertex0;
            this.vertices[oldCount + 1] = vertex1;
            this.vertices[oldCount + 2] = vertex2;

            return oldCount;
        }

        public ushort AddVertices(TVertex vertex0, TVertex vertex1, TVertex vertex2, TVertex vertex3)
        {
            ushort oldCount = this.count;
            int newCount = oldCount + 4;
            this.ensureCapacity(newCount);
            this.count = (ushort)newCount;

            this.vertices[oldCount] = vertex0;
            this.vertices[oldCount + 1] = vertex1;
            this.vertices[oldCount + 2] = vertex2;
            this.vertices[oldCount + 3] = vertex3;

            return oldCount;
        }

        public ushort AddVertices(params TVertex[] vertices)
        {
            ushort oldCount = this.count;
            int newCount = oldCount + vertices.Length;
            this.ensureCapacity(newCount);
            Array.Copy(vertices, 0, this.vertices, this.count, vertices.Length);
            this.count = (ushort)newCount;
            return oldCount;
        }

        public void Bind()
        {
            // Make this the active array buffer
            GL.BindBuffer(BufferTarget.ArrayBuffer, this.handle);
        }

        public void BufferData()
        {
            // Copy contained vertices to GPU memory
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(this.vertexSize * this.count), this.vertices, BufferUsageHint.StreamDraw);
        }

        public void Draw()
        {
            // Draw buffered vertices as triangles
            GL.DrawArrays(this.primitiveType, 0, this.count);
        }

        public void Clear()
        {
            this.count = 0;
        }

        public void RemoveVertices(int count)
        {
            this.count = count > this.count ? (ushort)0 : (ushort)(this.count - count);
        }

        private void ensureCapacity(int minCapacity)
        {
            if (this.vertices.Length <= minCapacity)
                Array.Resize(ref this.vertices, Math.Max(this.vertices.Length * 2, minCapacity));
        }

        public void Dispose()
        {
            this.dispose();
            GC.SuppressFinalize(this);
        }

        // Disposes the vertex buffer and deletes the underlying GL object.
        private void dispose()
        {
            if (this.disposed)
                return;

            if (GraphicsContext.CurrentContext == null || GraphicsContext.CurrentContext.IsDisposed)
                return;

            GL.DeleteBuffer(this.handle);

            this.disposed = true;
        }

        ~VertexBuffer()
        {
            this.dispose();
        }
    }
}
