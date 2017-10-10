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
        private ShaderProgram   shaderProgram;
        private List<IRenderer> rendererPool;
        private bool            loaded = false;
        private Camera          renderCamera;
        
        public ShaderProgram    ShaderProgam { get { return shaderProgram; } }

        public RenderingPool()
        {
        }

        public void Load()
        {
            string vertexShaderCode = ResourceManager.GetTextResource("Voxa.Assets.Shaders.BasicShader.vert");
            string fragmentShaderCode = ResourceManager.GetTextResource("Voxa.Assets.Shaders.BasicFragmentShader.frag");
            Shader vertexShader = new Shader(ShaderType.VertexShader, vertexShaderCode);
            Shader fragmentShader = new Shader(ShaderType.FragmentShader, fragmentShaderCode);

            this.shaderProgram = new ShaderProgram(vertexShader, fragmentShader);
            this.rendererPool = new List<IRenderer>();

            this.projectionMatrix = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, Engine.EngineWindow.Width / (float)Engine.EngineWindow.Height, 0.1f, 100.0f);
            Matrix4 modelview = Matrix4.LookAt(Vector3.Zero, Vector3.UnitZ, Vector3.UnitY);

            Engine.UniformManager.AddUniform(new Matrix4Uniform("projectionMatrix", Matrix4.Mult(modelview, this.projectionMatrix)));
            Engine.UniformManager.AddUniform(new LightUniform("light"));
            Engine.UniformManager.AddUniform(new Matrix3Uniform("normalMatrix"));
            Engine.UniformManager.AddUniform(new Vector3Uniform("cameraPosition"));
            Engine.UniformManager.AddUniform(new MaterialUniform("material")); 

            this.loaded = true;
        }

        public void AddToPool(IRenderer renderer)
        {
            this.rendererPool.Add(renderer);
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

                // Activate shader program and set uniforms
                this.shaderProgram.Use();
                projectionMatrixUniform.Set(this.shaderProgram);
                cameraPositionUniform.Set(this.shaderProgram);
                Engine.Game.CurrentScene.SetShadingUniforms();

                // Render the pool list
                foreach (var renderer in this.rendererPool) {
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
