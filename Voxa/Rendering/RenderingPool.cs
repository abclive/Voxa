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
    sealed class RenderingPool
    {
        private Matrix4         projectionMatrix;
        private Matrix4Uniform  projectionMatrixUniform;
        private ShaderProgram   shaderProgram;
        private List<IRenderer> rendererPool;
        private bool            loaded = false;
        private Camera          renderCamera;
        
        public ShaderProgram    ShaderProgam { get { return shaderProgram; } }
        public Sampler2DUniform ActiveTextureUniform;
        public LightUniform     CurrentLightUniform;
        public Matrix3Uniform   NormalMatrixUniform;
        public Vector3Uniform   CameraPositionUniform;

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
            this.projectionMatrixUniform = new Matrix4Uniform("projectionMatrix");
            this.projectionMatrixUniform.Matrix = Matrix4.Mult(modelview, this.projectionMatrix);

            this.ActiveTextureUniform = new Sampler2DUniform("tex");
            this.CurrentLightUniform = new LightUniform("light");
            this.NormalMatrixUniform = new Matrix3Uniform("normalMatrix");
            this.CameraPositionUniform = new Vector3Uniform("cameraPosition");

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
                this.projectionMatrixUniform.Matrix = Matrix4.Mult(modelview, this.projectionMatrix);
                this.CameraPositionUniform.Value = this.renderCamera.GetGameObject().Transform.Position;

                // Activate shader program and set uniforms
                this.shaderProgram.Use();
                this.projectionMatrixUniform.Set(this.shaderProgram);
                this.ActiveTextureUniform.Set(this.shaderProgram);
                this.CameraPositionUniform.Set(this.shaderProgram);
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
