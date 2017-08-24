using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Voxa.Objects;

namespace Voxa.Assets.Scripts
{
    class LightTest : Component
    {
        public override void OnUpdate()
        {
            base.OnUpdate();
            Vector3 updatedPos = this.gameObject.Transform.Position;
            if (Engine.EngineWindow.Keyboard[OpenTK.Input.Key.I]) {
                updatedPos.Y += 0.1f;
            }
            if (Engine.EngineWindow.Keyboard[OpenTK.Input.Key.K]) {
                updatedPos.Y -= 0.1f;
            }
            if (Engine.EngineWindow.Keyboard[OpenTK.Input.Key.J]) {
                updatedPos.X -= 0.1f;
            }
            if (Engine.EngineWindow.Keyboard[OpenTK.Input.Key.L]) {
                updatedPos.X += 0.1f;
            }
            if (Engine.EngineWindow.Keyboard[OpenTK.Input.Key.F]) {
                updatedPos = Engine.Game.CurrentScene.GetActiveCamera().GetGameObject().Transform.Position;
            }
            this.gameObject.Transform.Position = updatedPos;
        }
    }
}
