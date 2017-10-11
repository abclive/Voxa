using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Voxa.Rendering;
using Voxa.Objects;

namespace Voxa.Primitives.Shape
{
    public interface IShapeDecorator
    {
        Vector3 Apply(Vector3 vertex);
        void ApplyFace(ref Vector3 v1, ref Vector3 v2, ref Vector3 v3);
    }
}
