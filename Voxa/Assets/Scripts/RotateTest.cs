using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Voxa.Objects;
using Voxa.Objects.Renderer;

namespace Voxa.Assets.Scripts
{
    class RotateTest : Component
    {
        public override void OnUpdate()
        {
            base.OnUpdate();
            if (Engine.EngineWindow.Keyboard[OpenTK.Input.Key.R])
            {
                Vector3 updatedRot = this.gameObject.Transform.EulerRotation;
                updatedRot.Y -= 0.05f;
                this.gameObject.Transform.EulerRotation = updatedRot;
                this.gameObject.GetComponent<ModelRenderer>().UpdateAllVertexBuffers();
            }
        }
    }
}
