using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxa.Rendering.Uniforms
{
    interface IUniform
    {
        string GetName();
        void Set(ShaderProgram program);
    }
}
