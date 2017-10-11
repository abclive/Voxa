using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxa.Rendering;

namespace Voxa.Objects.Renderer
{
    public interface IRenderer
    {
        void Render();
        ShaderProgram GetShader();
    }
}
