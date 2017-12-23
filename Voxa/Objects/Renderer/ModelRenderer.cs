using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Voxa.Rendering;
using Voxa.Rendering.Uniforms;
using Voxa.Utils;

namespace Voxa.Objects.Renderer
{
    public class ModelRenderer : Component, IRenderer
    {
        public StaticModel   Model;
        public ShaderProgram CustomShader;
        public int           Priority;

        private bool isInit = false;
        private Dictionary<Guid, int> glBufferIds = new Dictionary<Guid, int>();
        protected Dictionary<Guid, VertexBuffer<TexturedVertex>> vertexBuffers = new Dictionary<Guid, VertexBuffer<TexturedVertex>>();
        protected Dictionary<Guid, VertexArray<TexturedVertex>> vertexArrays = new Dictionary<Guid, VertexArray<TexturedVertex>>();

        public ModelRenderer()
        {}

        public ModelRenderer(StaticModel model)
        {
            this.Model = model;
            this.Priority = 0;
        }

        public ModelRenderer(StaticModel model, int priority)
        {
            this.Model = model;
            this.Priority = priority;
        }

        public ModelRenderer(StaticModel model, ShaderProgram customShader)
        {
            this.Model = model;
            this.CustomShader = customShader;
        }

        public ModelRenderer(StaticModel model, ShaderProgram customShader, int priority)
        {
            this.Model = model;
            this.CustomShader = customShader;
            this.Priority = priority;
        }

        public override void OnLoad()
        {
            base.OnLoad();
            this.Init();
        }

        public void Init()
        {
            if (!this.isInit) {
                this.isInit = true;
                foreach (Mesh mesh in this.Model.Meshes) {
                    foreach (Mesh.Primitive primitive in mesh.Primitives) {
                        this.glBufferIds.Add(primitive.GUID, GL.GenBuffer());
                        this.vertexBuffers.Add(primitive.GUID, new VertexBuffer<TexturedVertex>(TexturedVertex.Size, PrimitiveType.Triangles));
                        this.vertexArrays.Add(primitive.GUID, new VertexArray<TexturedVertex>(
                            this.vertexBuffers[primitive.GUID], this.GetShader(),
                            new VertexAttribute("vPosition", 3, VertexAttribPointerType.Float, TexturedVertex.Size, 0),
                            new VertexAttribute("vColor", 4, VertexAttribPointerType.Float, TexturedVertex.Size, 12),
                            new VertexAttribute("vTexCoord", 2, VertexAttribPointerType.Float, TexturedVertex.Size, 28),
                            new VertexAttribute("vNormal", 3, VertexAttribPointerType.Float, TexturedVertex.Size, 36)
                        ));
                        if (!Model.Materials[primitive.MaterialModelId].IsLoaded)
                            Model.Materials[primitive.MaterialModelId].Load();

                        foreach (TexturedVertex vertex in primitive.Vertices) {
                            this.vertexBuffers[primitive.GUID].AddVertex(vertex);
                        }
                    }
                }

                Engine.RenderingPool.AddToPool(this, this.Priority);
            }
        }

        public void Render()
        {
            Light light = Engine.Game.CurrentScene.GetClosestLight(this.gameObject);
            if (light != null) {
                light.Bind();
            }
            foreach (Mesh mesh in this.Model.Meshes) {
                Matrix4 transformMatrix = this.gameObject.Transform.GetLocalToWorldMatrix();
                if (mesh.LocalMatrix != Matrix4.Identity) {
                    transformMatrix = mesh.LocalMatrix * transformMatrix;
                }
                Matrix3Uniform normalMatrixUniform = Engine.UniformManager.GetUniform<Matrix3Uniform>("normalMatrix");
                normalMatrixUniform.Matrix = Matrix3.Transpose(new Matrix3(transformMatrix.Inverted()));
                normalMatrixUniform.Set(this.GetShader());

                foreach (Mesh.Primitive primitive in mesh.Primitives) {
                    // Bind vertex buffer and array objects
                    this.vertexBuffers[primitive.GUID].Bind();
                    this.vertexArrays[primitive.GUID].Bind();

                    // Upload vertices to GPU and draw them
                    this.vertexBuffers[primitive.GUID].BufferData();

                    // Bind primitive Material
                    Model.Materials[primitive.MaterialModelId].Bind(this.GetShader());

                    if (primitive.Indices.Count == 0) {
                        this.vertexBuffers[primitive.GUID].Draw();
                    } else {
                        GL.BindBuffer(BufferTarget.ElementArrayBuffer, this.glBufferIds[primitive.GUID]);
                        GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(primitive.Indices.Count * sizeof(ushort)), primitive.Indices.ToArray(), BufferUsageHint.DynamicDraw);
                        GL.DrawElements(PrimitiveType.Triangles, primitive.Indices.Count, DrawElementsType.UnsignedShort, (IntPtr)0);
                    }
                }
            }

            this.UpdateAllVertexBuffers();
        }

        public void UpdateAllVertexBuffers()
        {
            foreach (Mesh mesh in this.Model.Meshes) {
                this.UpdateVertexBuffer(mesh);
            }
        }

        public void UpdateVertexBuffer(Mesh mesh)
        {
            Matrix4 transformMatrix = this.gameObject.Transform.GetLocalToWorldMatrix();
            if (mesh.LocalMatrix != Matrix4.Identity) {
                transformMatrix = mesh.LocalMatrix * transformMatrix;
            }
            Matrix4Uniform modelMatrix = Engine.UniformManager.GetUniform<Matrix4Uniform>("modelMatrix");
            modelMatrix.Matrix = transformMatrix;
            modelMatrix.Set(this.GetShader());
        }

        public ShaderProgram GetShader()
        {
            if (this.CustomShader != null) {
                return CustomShader;
            }
            return Engine.RenderingPool.PhongShaderProgram;
        }

        public int GetPriority()
        {
            return this.Priority;
        }

        public override void OnDestroy()
        {
            this.Model = null;
            this.CustomShader = null;
            Engine.RenderingPool.RemoveFromPool(this);
            base.OnDestroy();
        }
    }
}
