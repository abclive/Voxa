using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxa.Rendering;
using OpenTK;

namespace Voxa.Objects.Renderer.Shape
{
    class ShapeRenderer : Component, IRenderer
    {
        protected VertexBuffer<ColouredVertex> vertexBuffer;
        protected VertexArray<ColouredVertex>  vertexArray;
        protected List<ColouredVertex>         modelVertices = new List<ColouredVertex>();

        public virtual void Render()
        {}

        public void UpdateVertexBuffer()
        {
            Matrix4 transformMatrix = this.gameObject.Transform.GetLocalToWorldMatrix();
            this.vertexBuffer.Clear();
            foreach (ColouredVertex vertex in this.modelVertices)
            {
                Vector3 vertexPos = vertex.Position;
                Vector4 updatedPos = new Vector4(vertexPos.X, vertexPos.Y, vertexPos.Z, 1) * transformMatrix;
                this.vertexBuffer.AddVertex(new ColouredVertex(new Vector3(updatedPos.X, updatedPos.Y, updatedPos.Z), vertex.Color));
            }
        }
    }
}
