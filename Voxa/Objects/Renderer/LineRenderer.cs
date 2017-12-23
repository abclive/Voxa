using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxa.Rendering;
using Voxa.Rendering.Uniforms;
using OpenTK.Graphics.OpenGL;

namespace Voxa.Objects.Renderer
{
    public class LineRenderer : Component, IRenderer
    {
        public bool Active = true;

        public List<DebugLine> Lines;

        private bool isInit = false;
        private VertexBuffer<UnlitColouredVertex> vertexBuffer;
        private VertexArray<UnlitColouredVertex>  vertexArray;

        public LineRenderer(List<DebugLine> lines)
        {
            this.Lines = lines;
        }

        public LineRenderer(DebugLine line)
        {
            this.Lines = new List<DebugLine>();
            this.Lines.Add(line);
        }

        public override void OnLoad()
        {
            base.OnLoad();
            this.Init();
        }

        public void Init()
        {
            if (!this.isInit) {
                Engine.UniformManager.AddUniform(new Matrix4Uniform("cameraProjection"));

                this.vertexBuffer = new VertexBuffer<UnlitColouredVertex>(UnlitColouredVertex.Size, PrimitiveType.Lines);
                this.vertexArray = new VertexArray<UnlitColouredVertex>(this.vertexBuffer, this.GetShader(),
                    new VertexAttribute("vPosition", 3, VertexAttribPointerType.Float, UnlitColouredVertex.Size, 0),
                    new VertexAttribute("vColor", 4, VertexAttribPointerType.Float, UnlitColouredVertex.Size, 12)
                );

                Engine.RenderingPool.AddToPool(this);
                this.UpdateVertexBuffer();
            }
        }

        public void Render()
        {
            Matrix4Uniform cameraProjection = Engine.UniformManager.GetUniform<Matrix4Uniform>("cameraProjection");
            cameraProjection.Matrix = Engine.RenderingPool.UICamera.GetProjectionMatrix();
            cameraProjection.Set(this.GetShader());

            this.vertexBuffer.Bind();
            this.vertexArray.Bind();
            this.vertexBuffer.BufferData();
            this.vertexBuffer.Draw();
        }

        public void UpdateVertexBuffer()
        {
            this.vertexBuffer.Clear();
            foreach (DebugLine line in this.Lines) {
                this.vertexBuffer.AddVertices(new UnlitColouredVertex(line.Start, line.Color), new UnlitColouredVertex(line.End, line.Color));
            }
        }

        public int GetPriority()
        {
            return 1000;
        }

        public ShaderProgram GetShader()
        {
            return Engine.RenderingPool.LineShaderProgram;
        }
    }
}
