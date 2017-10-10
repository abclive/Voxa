using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using Voxa.Rendering;
using Voxa.Rendering.Uniforms;

namespace Voxa.Objects
{
    public class Material
    {
        public int     ModelId;
        public string  Name;
        public Texture DiffuseMap;
        public Color4  AmbientColor;
        public Color4  DiffuseColor;
        public Texture SpecularMap;
        public Color4  SpecularColor;
        public float   Shininess;
        public bool IsLoaded { get { return isLoaded; } }

        private bool isLoaded = false;
        private MaterialUniform uniform;

        public Material(int modelId, Texture diffuseMap, Texture specularMap, float shininess)
        {
            this.ModelId = modelId;
            this.DiffuseMap = diffuseMap;
            this.SpecularMap = specularMap;
            this.SpecularMap.Unit = TextureUnit.Texture1;
            this.Shininess = shininess;
            this.AmbientColor = Color4.White;
        }

        public Material(int modelId, Texture diffuseMap, Color4 specularColor, float shininess)
        {
            this.ModelId = modelId;
            this.DiffuseMap = diffuseMap;
            this.SpecularColor = specularColor;
            this.Shininess = shininess;
            this.AmbientColor = Color4.White;
        }

        public Material(int modelId, Color4 diffuseColor, Texture specularMap, float shininess)
        {
            this.ModelId = modelId;
            this.DiffuseColor = diffuseColor;
            this.SpecularMap = specularMap;
            this.SpecularMap.Unit = TextureUnit.Texture1;
            this.Shininess = shininess;
            this.AmbientColor = Color4.White;
        }

        public Material(int modelId, Color4 diffuseColor, Color4 specularColor, float shininess)
        {
            this.ModelId = modelId;
            this.DiffuseColor = diffuseColor;
            this.SpecularColor = specularColor;
            this.Shininess = shininess;
            this.AmbientColor = Color4.White;
        }

        public Material(int modelId)
        {
            this.ModelId = modelId;
        }

        public void Load()
        {
            if (this.DiffuseMap != null) {
                this.DiffuseMap.Load();
            }
            if (this.SpecularMap != null) {
                this.SpecularMap.Load();
            }
            this.isLoaded = true;
        }

        public void Bind(ShaderProgram program)
        {
            if (this.DiffuseMap != null) {
                if (this.SpecularMap != null) {
                    this.uniform = new MaterialUniform("material", this.DiffuseMap, this.SpecularMap, this.Shininess, this.AmbientColor);
                } else {
                    this.uniform = new MaterialUniform("material", this.DiffuseMap, this.SpecularColor, this.Shininess, this.AmbientColor);
                }
            } else {
                if (this.SpecularMap != null) {
                    this.uniform = new MaterialUniform("material", this.DiffuseColor, this.SpecularMap, this.Shininess, this.AmbientColor);
                } else {
                    this.uniform = new MaterialUniform("material", this.DiffuseColor, this.SpecularColor, this.Shininess, this.AmbientColor);
                }
            }
            this.uniform.Set(program);
        }
    }
}
