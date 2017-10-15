using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Voxa.Objects;
using Voxa.Rendering.Uniforms;

namespace Voxa.Rendering
{
    public class UniformManager
    {
        private Dictionary<string, IUniform> uniformList = new Dictionary<string, IUniform>();

        public void AddUniform(IUniform uniform)
        {
            if (!this.uniformList.TryGetValue(uniform.GetName(), out IUniform testUniform)) {
                this.uniformList.Add(uniform.GetName(), uniform);
            }
        }

        public T GetUniform<T>(string name) where T : IUniform
        {
            IUniform uniform;
            if (this.uniformList.TryGetValue(name, out uniform)) {
                return (T)uniform;
            }
            return default(T);
        }

        public void SetUniform(IUniform uniform)
        {
            if (this.uniformList.ContainsKey(uniform.GetName())) {
                this.uniformList[uniform.GetName()] = uniform;
            }
        }

        public void BindAllUniforms(ShaderProgram program)
        {
            foreach (IUniform uniform in this.uniformList.Values) {
                uniform.Set(program);
            }
        }
    }
}
