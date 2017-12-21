using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxa.Rendering;
using Voxa.Rendering.Uniforms;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using Voxa.Utils;

namespace Voxa.Objects.Renderer
{
    public class TextRenderer : Component, IRenderer
    {
        public string             Text;
        public Font               Font;
        public Color4             Color = Color4.White;
        public Vector2            Position = Vector2.Zero;
        public double             Scale;
        public bool               Active = true;

        public ShaderProgram      CustomShader;
        public OrthographicCamera Camera;
        public int                Priority;

        private VertexBuffer<TextVertex> vertexBuffer;
        private VertexArray<TextVertex>  vertexArray;
        private TextVertex[]             textVertices;

        public TextRenderer()
        {}

        public TextRenderer(string text, Vector2 position, Color4 color, Font font)
        {
            this.Text = text;
            this.Color = color;
            this.Position = position;
            this.Font = font;
            this.Scale = 1;
            this.Camera = Engine.RenderingPool.UICamera;
            this.Priority = 100;
        }

        public TextRenderer(string text, Vector2 position, Color4 color, Font font, double scale)
        {
            this.Text = text;
            this.Color = color;
            this.Position = position;
            this.Font = font;
            this.Scale = scale;
            this.Camera = Engine.RenderingPool.UICamera;
            this.Priority = 100;
        }

        public TextRenderer(string text, Vector2 position, Color4 color, Font font, OrthographicCamera camera)
        {
            this.Text = text;
            this.Color = color;
            this.Position = position;
            this.Font = font;
            this.Camera = camera;
            this.Priority = 100;
            this.Scale = 1;
        }

        public override void OnLoad()
        {
            base.OnLoad();
            this.Init();
        }

        public void Init()
        {
            Engine.UniformManager.AddUniform(new Matrix4Uniform("textProjection"));
            Engine.UniformManager.AddUniform(new Sampler2DUniform("textTexture"));
            Engine.UniformManager.AddUniform(new Vector3Uniform("textColor"));

            this.vertexBuffer = new VertexBuffer<TextVertex>(TextVertex.Size, PrimitiveType.Triangles);
            this.vertexBuffer.BufferUsage = BufferUsageHint.DynamicDraw;
            this.vertexArray = new VertexArray<TextVertex>(
                this.vertexBuffer, this.GetShader(),
                new VertexAttribute("vPosition", 2, VertexAttribPointerType.Float, TextVertex.Size, 0),
                new VertexAttribute("vTexCoord", 2, VertexAttribPointerType.Float, TextVertex.Size, 8)
            );
            this.UpdateVertexBuffer();
            Engine.RenderingPool.AddToPool(this, this.Priority);
        }

        public void Render()
        {
            if (this.Active) {
                Matrix4Uniform textProjection = Engine.UniformManager.GetUniform<Matrix4Uniform>("textProjection");
                textProjection.Matrix = this.Camera.GetProjectionMatrix();
                textProjection.Set(this.GetShader());

                Vector3Uniform textColor = Engine.UniformManager.GetUniform<Vector3Uniform>("textColor");
                textColor.Value = new Vector3(this.Color.R, this.Color.G, this.Color.B);
                textColor.Set(this.GetShader());

                GL.ActiveTexture(TextureUnit.Texture0);
                this.vertexArray.Bind();
                this.vertexBuffer.Bind();
                GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * 6 * 4, (IntPtr)null, BufferUsageHint.DynamicDraw);

                for (int i = 0; i < this.Text.Length; i++) {
                    Font.Character character = this.Font.Characters[this.Text[i]];

                    GL.BindTexture(TextureTarget.Texture2D, character.TextureId);

                    TextVertex[] subVertices = textVertices.SubArray(i * 6, 6);

                    this.vertexBuffer.Bind();
                    this.vertexBuffer.BufferSubData((IntPtr)0, TextVertex.Size * subVertices.Length, subVertices);
                    GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
                    GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
                }
            }
        }

        public void UpdateVertexBuffer()
        {
            this.textVertices = new TextVertex[this.Text.Length * 6];
            int vIndex = 0;
            float x = this.Position.X;

            for (int i = 0; i < this.Text.Length; i++) {
                Font.Character character = this.Font.Characters[this.Text[i]];

                float xPos = x + character.Bearing.X * (float)this.Scale;
                float yPos = this.Position.Y - (character.Size.Height - character.Bearing.Y) * (float)this.Scale;

                float w = character.Size.Width * (float)this.Scale;
                float h = character.Size.Height * (float)this.Scale;

                this.textVertices[vIndex++] = new TextVertex(new Vector2(xPos, yPos + h),     new Vector2(0, 0));
                this.textVertices[vIndex++] = new TextVertex(new Vector2(xPos, yPos),         new Vector2(0, 1));
                this.textVertices[vIndex++] = new TextVertex(new Vector2(xPos + w, yPos),     new Vector2(1, 1));

                this.textVertices[vIndex++] = new TextVertex(new Vector2(xPos, yPos + h),     new Vector2(0, 0));
                this.textVertices[vIndex++] = new TextVertex(new Vector2(xPos + w, yPos),     new Vector2(1, 1));
                this.textVertices[vIndex++] = new TextVertex(new Vector2(xPos + w, yPos + h), new Vector2(1, 0));

                x += character.Advance * (float)this.Scale;
            }
        }

        public double GetPixelWidth()
        {
            List<double> linesSize = new List<double>();
            double currentSize = 0;
            for (int i = 0; i < this.Text.Length; i++) {
                Font.Character character = this.Font.Characters[this.Text[i]];
                if (this.Text[i] == '\n') {
                    linesSize.Add(currentSize);
                    currentSize = 0;
                    continue;
                }
                currentSize += (double)(character.Advance) * this.Scale;
                if (i + 1 == this.Text.Length) {
                    linesSize.Add(currentSize);
                }
            }
            linesSize.Sort((double a, double b) => {
                if (a > b) return 1;
                return -1;
            });
            return linesSize.First();
        }

        public int GetPixelHeight()
        {
            Font.Character character = this.Font.Characters[this.Text[0]];
            return character.Size.Height;
        }

        public int GetPriority()
        {
            return this.Priority;
        }

        public ShaderProgram GetShader()
        {
            return this.CustomShader ?? Engine.RenderingPool.TextShaderProgram;
        }
    }
}
