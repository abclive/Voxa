using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Voxa.Objects;
using Voxa.Objects.Renderer.Shape;
using Voxa.Utils;

namespace Voxa.Primitives.Shape
{
    class Block : GameObject
    {
        public Block()
        {
            this.AttachComponent(new BlockRenderer());
        }

        public Block(Vector3 initialPosition)
        {
            this.AttachComponent(new BlockRenderer());
            this.Transform.Position = initialPosition;
        }
    }
}
