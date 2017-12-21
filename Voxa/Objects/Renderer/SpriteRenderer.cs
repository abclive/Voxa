using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Voxa.Rendering;
using Voxa.Rendering.Uniforms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Voxa.Objects.Renderer
{
    public class SpriteRenderer : Component, IRenderer
    {
        public Sprite               Sprite;
        public Size                 CustomSize = Size.Empty;
        public ShaderProgram        CustomShader;
        public OrthographicCamera   Camera;
        public int                  Priority;

        private VertexBuffer<SpriteVertex> vertexBuffer;
        private VertexArray<SpriteVertex>  vertexArray;

        public SpriteRenderer()
        {}

        public SpriteRenderer(Sprite sprite, OrthographicCamera camera)
        {
            this.Sprite = sprite;
            this.Camera = camera;
            this.Priority = 0;
        }

        public SpriteRenderer(Sprite sprite, OrthographicCamera camera, int priority)
        {
            this.Sprite = sprite;
            this.Camera = camera;
            this.Priority = priority;
        }

        public SpriteRenderer(Sprite sprite, OrthographicCamera camera, int priority, ShaderProgram customShader)
        {
            this.Sprite = sprite;
            this.Camera = camera;
            this.Priority = priority;
            this.CustomShader = customShader;
        }

        public SpriteRenderer(Sprite sprite, OrthographicCamera camera, ShaderProgram customShader)
        {
            this.Sprite = sprite;
            this.Camera = camera;
            this.CustomShader = customShader;
            this.Priority = 0;
        }

        public override void OnLoad()
        {
            base.OnLoad();
            this.Init();
        }

        public void Init()
        {
            Engine.UniformManager.AddUniform(new Matrix4Uniform("cameraProjection"));
            Engine.UniformManager.AddUniform(new Sampler2DUniform("spriteTexture"));

            this.Sprite.Texture.Load();

            this.vertexBuffer = new VertexBuffer<SpriteVertex>(SpriteVertex.Size, PrimitiveType.Quads);
            this.vertexArray = new VertexArray<SpriteVertex>(
                this.vertexBuffer, this.GetShader(),
                new VertexAttribute("vPosition", 3, VertexAttribPointerType.Float, SpriteVertex.Size, 0),
                new VertexAttribute("vColor", 4, VertexAttribPointerType.Float, SpriteVertex.Size, 12),
                new VertexAttribute("vTexCoord", 2, VertexAttribPointerType.Float, SpriteVertex.Size, 28)
            );
            this.UpdateVertexBuffer();
            Engine.RenderingPool.AddToPool(this, this.Priority);
        }

        public void Render()
        {
            Matrix4Uniform cameraProjection = Engine.UniformManager.GetUniform<Matrix4Uniform>("cameraProjection");
            cameraProjection.Matrix = (this.Camera != null) ? this.Camera.GetProjectionMatrix() : Matrix4.Identity;
            cameraProjection.Set(this.GetShader());

            this.Sprite.Texture.Bind();
            Sampler2DUniform spriteUniform = Engine.UniformManager.GetUniform<Sampler2DUniform>("spriteTexture");
            spriteUniform.SamplerId = 0;
            spriteUniform.Set(this.GetShader());

            this.vertexArray.Bind();
            this.vertexBuffer.Bind();

            this.vertexBuffer.BufferData();
            this.vertexBuffer.Draw();
        }

        public void UpdateVertexBuffer()
        {
            this.vertexBuffer.Clear();
            SpriteVertex[] vertices = new SpriteVertex[4];
            if (this.CustomSize == Size.Empty) {
                vertices[0] = new SpriteVertex(new Vector3(Sprite.Position.X, Sprite.Position.Y, -Sprite.Order), new Vector2(0, 1)); // bottom left
                vertices[1] = new SpriteVertex(new Vector3(Sprite.Position.X, Sprite.Position.Y + (Sprite.Texture.Height * Sprite.Size.Y), -Sprite.Order), new Vector2(0, 0)); // top left
                vertices[2] = new SpriteVertex(new Vector3(Sprite.Position.X + (Sprite.Texture.Width * Sprite.Size.X), Sprite.Position.Y + (Sprite.Texture.Height * Sprite.Size.Y), -Sprite.Order), new Vector2(1, 0)); // top right
                vertices[3] = new SpriteVertex(new Vector3(Sprite.Position.X + (Sprite.Texture.Width * Sprite.Size.X), Sprite.Position.Y, -Sprite.Order), new Vector2(1, 1)); // bottom right
            } else {
                vertices[0] = new SpriteVertex(new Vector3(Sprite.Position.X, Sprite.Position.Y, -Sprite.Order), new Vector2(0, 1)); // bottom left
                vertices[1] = new SpriteVertex(new Vector3(Sprite.Position.X, Sprite.Position.Y + this.CustomSize.Height, -Sprite.Order), new Vector2(0, 0)); // top left
                vertices[2] = new SpriteVertex(new Vector3(Sprite.Position.X + this.CustomSize.Width, Sprite.Position.Y + this.CustomSize.Height, -Sprite.Order), new Vector2(1, 0)); // top right
                vertices[3] = new SpriteVertex(new Vector3(Sprite.Position.X + this.CustomSize.Width, Sprite.Position.Y, -Sprite.Order), new Vector2(1, 1)); // bottom right
            }

            this.vertexBuffer.AddVertices(vertices);
        }

        public ShaderProgram GetShader()
        {
            if (this.CustomShader != null)
                return this.CustomShader;
            return Engine.RenderingPool.SpriteShaderProgram;
        }

        public int GetPriority()
        {
            return this.Priority;
        }

        public override void OnDestroy()
        {
            this.Sprite = null;
            this.CustomShader = null;
            Engine.RenderingPool.RemoveFromPool(this);
            base.OnDestroy();
        }
    }
}
