using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Voxa.Objects;
using Voxa.Rendering.Uniforms;
using Voxa.Utils;
using Voxa.Objects.Renderer;

namespace Voxa.Rendering
{
    public sealed class RenderingPool
    {
        private Matrix4         projectionMatrix;
        private ShaderProgram   phongShaderProgram;
        private ShaderProgram   spriteShaderProgram;
        private List<IRenderer> rendererPool;
        private bool            loaded = false;
        private Camera          renderCamera;
        
        public ShaderProgram    PhongShaderProgram { get { return this.phongShaderProgram; } }
        public ShaderProgram    SpriteShaderProgram { get { return this.spriteShaderProgram; } }

        public RenderingPool()
        {
        }

        public void Load()
        {
            string phongVertexShaderCode = ResourceManager.GetTextResource("Voxa.Assets.Shaders.BasicShader.vert");
            string phongFragmentShaderCode = ResourceManager.GetTextResource("Voxa.Assets.Shaders.BasicFragmentShader.frag");
            Shader phongVertexShader = new Shader(ShaderType.VertexShader, phongVertexShaderCode);
            Shader phongFragmentShader = new Shader(ShaderType.FragmentShader, phongFragmentShaderCode);
            this.phongShaderProgram = new ShaderProgram(phongVertexShader, phongFragmentShader);

            string spriteVertexShaderCode = ResourceManager.GetTextResource("Voxa.Assets.Shaders.SpriteShader.vert");
            string spriteFragmentShaderCode = ResourceManager.GetTextResource("Voxa.Assets.Shaders.SpriteFragmentShader.frag");
            Shader spriteVertexShader = new Shader(ShaderType.VertexShader, spriteVertexShaderCode);
            Shader spriteFragmentShader = new Shader(ShaderType.FragmentShader, spriteFragmentShaderCode);
            this.spriteShaderProgram = new ShaderProgram(spriteVertexShader, spriteFragmentShader);

            this.rendererPool = new List<IRenderer>();

            this.projectionMatrix = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, Engine.EngineWindow.Width / (float)Engine.EngineWindow.Height, 0.1f, Engine.Game.RENDER_DISTANCE);
            Matrix4 modelview = Matrix4.LookAt(Vector3.Zero, Vector3.UnitZ, Vector3.UnitY);

            Engine.UniformManager.AddUniform(new Matrix4Uniform("projectionMatrix", Matrix4.Mult(modelview, this.projectionMatrix)));
            Engine.UniformManager.AddUniform(new LightUniform("light"));
            Engine.UniformManager.AddUniform(new Matrix4Uniform("modelMatrix"));
            Engine.UniformManager.AddUniform(new Matrix3Uniform("normalMatrix"));
            Engine.UniformManager.AddUniform(new Vector3Uniform("cameraPosition"));
            Engine.UniformManager.AddUniform(new MaterialUniform("material")); 

            this.loaded = true;
        }

        public void AddToPool(IRenderer renderer)
        {
            this.rendererPool.Add(renderer);
            this.SortRendererList();
        }

        public void SortRendererList()
        {
            this.rendererPool.Sort((a, b) => {
                if (a.GetPriority() > b.GetPriority()) return 1;
                return -1;
            });
        }

        public void AddToPool(IRenderer renderer, int priority)
        {
            int index = 0;
            foreach (IRenderer lRenderer in this.rendererPool) {
                if (lRenderer.GetPriority() > priority) {
                    break;
                }
                index++;
            }
            this.rendererPool.Insert(index, renderer);
        }

        public void RemoveFromPool(IRenderer renderer)
        {
            this.rendererPool.Remove(renderer);
        }

        public void RenderPool()
        {
            if (this.loaded && this.renderCamera != null)
            {
                Matrix4 modelview = this.renderCamera.GetViewMatrix();
                Matrix4Uniform projectionMatrixUniform = Engine.UniformManager.GetUniform<Matrix4Uniform>("projectionMatrix");
                Vector3Uniform cameraPositionUniform = Engine.UniformManager.GetUniform<Vector3Uniform>("cameraPosition");

                projectionMatrixUniform.Matrix = Matrix4.Mult(modelview, this.projectionMatrix);
                cameraPositionUniform.Value = this.renderCamera.GetGameObject().Transform.Position;

                // Render the pool list
                foreach (var renderer in this.rendererPool) {
                    // Activate shader program and set uniforms
                    ShaderProgram rendererShader = renderer.GetShader();
                    rendererShader.Use();
                    projectionMatrixUniform.Set(rendererShader);
                    cameraPositionUniform.Set(rendererShader);
                    Engine.Game.CurrentScene.SetShadingUniforms(rendererShader);
                    renderer.Render();
                }
            }
        }

        public void SetRenderCamera(Camera camera)
        {
            this.renderCamera = camera;
        }
    }
}
