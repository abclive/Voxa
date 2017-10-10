using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxa.Rendering.Uniforms
{
    public interface IUniform
    {
        string GetName();
        void Set(ShaderProgram program);
    }
}
