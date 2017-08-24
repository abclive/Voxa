using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using Voxa.Rendering;

namespace Voxa.Objects.Renderer.Shape
{
    class BlockRenderer : ShapeRenderer
    {
        public override void OnLoad()
        {
            base.OnLoad();

            // Top
            this.modelVertices.Add(new ColouredVertex(new Vector3(1, 1, -1), Color4.Green)); // top right
            this.modelVertices.Add(new ColouredVertex(new Vector3(-1, 1, -1), Color4.Green)); // top left
            this.modelVertices.Add(new ColouredVertex(new Vector3(-1, 1, 1), Color4.Green)); // bottom left
            this.modelVertices.Add(new ColouredVertex(new Vector3(1, 1, 1), Color4.Green)); // bottom right

            // Bottom
            this.modelVertices.Add(new ColouredVertex(new Vector3(1, -1, -1), Color4.Orange)); // top right
            this.modelVertices.Add(new ColouredVertex(new Vector3(-1, -1, -1), Color4.Orange)); // top left
            this.modelVertices.Add(new ColouredVertex(new Vector3(-1, -1, 1), Color4.Orange)); // bottom left
            this.modelVertices.Add(new ColouredVertex(new Vector3(1, -1, 1), Color4.Orange)); // bottom right

            // Front
            this.modelVertices.Add(new ColouredVertex(new Vector3(1, 1, 1), Color4.Red)); // top right
            this.modelVertices.Add(new ColouredVertex(new Vector3(-1, 1, 1), Color4.Red)); // top left
            this.modelVertices.Add(new ColouredVertex(new Vector3(-1, -1, 1), Color4.Red)); // bottom left
            this.modelVertices.Add(new ColouredVertex(new Vector3(1, -1, 1), Color4.Red)); // bottom right
            
            // Back
            this.modelVertices.Add(new ColouredVertex(new Vector3(1, 1, -1), Color4.Yellow)); // top right
            this.modelVertices.Add(new ColouredVertex(new Vector3(-1, 1, -1), Color4.Yellow)); // top left
            this.modelVertices.Add(new ColouredVertex(new Vector3(-1, -1, -1), Color4.Yellow)); // bottom left
            this.modelVertices.Add(new ColouredVertex(new Vector3(1, -1, -1), Color4.Yellow)); // bottom right

            // Left
            this.modelVertices.Add(new ColouredVertex(new Vector3(-1, 1, 1), Color4.Blue)); // top right
            this.modelVertices.Add(new ColouredVertex(new Vector3(-1, 1, -1), Color4.Blue)); // top left
            this.modelVertices.Add(new ColouredVertex(new Vector3(-1, -1, -1), Color4.Blue)); // bottom left
            this.modelVertices.Add(new ColouredVertex(new Vector3(-1, -1, 1), Color4.Blue)); // bottom right

            // Left
            this.modelVertices.Add(new ColouredVertex(new Vector3(1, 1, 1), Color4.Magenta)); // top right
            this.modelVertices.Add(new ColouredVertex(new Vector3(1, 1, -1), Color4.Magenta)); // top left
            this.modelVertices.Add(new ColouredVertex(new Vector3(1, -1, -1), Color4.Magenta)); // bottom left
            this.modelVertices.Add(new ColouredVertex(new Vector3(1, -1, 1), Color4.Magenta)); // bottom right


            this.vertexBuffer = new VertexBuffer<ColouredVertex>(ColouredVertex.Size, PrimitiveType.Quads);
            this.UpdateVertexBuffer();
            this.vertexArray = new VertexArray<ColouredVertex>(
                this.vertexBuffer, Engine.RenderingPool.ShaderProgam,
                new VertexAttribute("vPosition", 3, VertexAttribPointerType.Float, ColouredVertex.Size, 0),
                new VertexAttribute("vColor", 4, VertexAttribPointerType.Float, ColouredVertex.Size, 12)
            );
            Engine.RenderingPool.AddToPool(this);
        }

        public override void Render()
        {
            base.Render();

            // Bind vertex buffer and array objects
            this.vertexBuffer.Bind();
            this.vertexArray.Bind();

            // Upload vertices to GPU and draw them
            this.vertexBuffer.BufferData();
            this.vertexBuffer.Draw();
        }

        ~BlockRenderer()
        {
            Engine.RenderingPool.RemoveFromPool(this);
        }
    }
}
