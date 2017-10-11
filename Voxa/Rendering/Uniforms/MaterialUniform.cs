using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Voxa.Rendering.Uniforms
{
    public sealed class MaterialUniform : IUniform
    {
        private readonly string name;
        public Texture DiffuseMap;
        public Texture SpecularMap;
        public Vector3 DiffuseColor;
        public Vector3 SpecularColor;
        public Vector3 AmbientColor;
        public float Shininess;

        public MaterialUniform(string name, Texture diffuseMap, Texture specularMap, float shininess, Color4 ambientColor)
        {
            this.name = name;
            this.DiffuseMap = diffuseMap;
            this.SpecularMap = specularMap;
            this.Shininess = shininess;
            this.AmbientColor = new Vector3(ambientColor.R, ambientColor.G, ambientColor.B);
        }

        public MaterialUniform(string name, Color4 diffuseColor, Texture specularMap, float shininess, Color4 ambientColor)
        {
            this.name = name;
            this.DiffuseColor = new Vector3(diffuseColor.R, diffuseColor.G, diffuseColor.B);
            this.SpecularMap = specularMap;
            this.Shininess = shininess;
            this.AmbientColor = new Vector3(ambientColor.R, ambientColor.G, ambientColor.B);
        }

        public MaterialUniform(string name, Texture diffuseMap, Color4 specularColor, float shininess, Color4 ambientColor)
        {
            this.name = name;
            this.DiffuseMap = diffuseMap;
            this.SpecularColor = new Vector3(specularColor.R, specularColor.G, specularColor.B);
            this.Shininess = shininess;
            this.AmbientColor = new Vector3(ambientColor.R, ambientColor.G, ambientColor.B);
        }

        public MaterialUniform(string name, Color4 diffuseColor, Color4 specularColor, float shininess, Color4 ambientColor)
        {
            this.name = name;
            this.DiffuseColor = new Vector3(diffuseColor.R, diffuseColor.G, diffuseColor.B);
            this.SpecularColor = new Vector3(specularColor.R, specularColor.G, specularColor.B);
            this.Shininess = shininess;
            this.AmbientColor = new Vector3(ambientColor.R, ambientColor.G, ambientColor.B);
        }

        public MaterialUniform(string name)
        {
            this.name = name;
        }

        public void Set(ShaderProgram program)
        {
            int uHandle;
            if (this.DiffuseMap != null) {
                uHandle = program.GetUniformLocation(this.name + ".diffuseMap");
                this.DiffuseMap.Bind();
                GL.Uniform1(uHandle, 0);
                this.DiffuseColor = new Vector3(1, 1, 1);
            } else {
                Engine.WhitePixelTexture.Unit = TextureUnit.Texture0;
                Engine.WhitePixelTexture.Bind();
            }
            if (this.SpecularMap != null) {
                uHandle = program.GetUniformLocation(this.name + ".specularMap");
                this.SpecularMap.Unit = TextureUnit.Texture1;
                this.SpecularMap.Bind();
                GL.Uniform1(uHandle, 1);
                this.SpecularColor = new Vector3(1, 1, 1);
            }
            uHandle = program.GetUniformLocation(this.name + ".ambientColor");
            GL.Uniform3(uHandle, this.AmbientColor);
            uHandle = program.GetUniformLocation(this.name + ".diffuseColor");
            GL.Uniform3(uHandle, this.DiffuseColor);
            uHandle = program.GetUniformLocation(this.name + ".specularColor");
            GL.Uniform3(uHandle, this.SpecularColor);
            uHandle = program.GetUniformLocation(this.name + ".shininess");
            GL.Uniform1(uHandle, this.Shininess);
        }

        public string GetName()
        {
            return this.name;
        }
    }
}
