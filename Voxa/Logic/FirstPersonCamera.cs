using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Input;
using Voxa.Objects;
using Voxa.Utils;

namespace Voxa.Logic
{
    public class FirstPersonCamera : Component
    {
        public override void OnLoad()
        {
            base.OnLoad();
            Logger.AddStickyInfo("cameraInfo", new LoggerMessage("", ConsoleColor.DarkGray));
        }

        public override void OnUpdate()
        {
            Logger.UpdateStickyInfo("cameraInfo", "Camera Position: " + this.gameObject.Transform.Position);
            if (Engine.EngineWindow.Focused) {
                Vector2 delta = Engine.EngineWindow.LastMousePos - new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
                Camera camera = this.gameObject.GetComponent<Camera>();
                camera.AddRotation(delta.X, delta.Y);

                if (Engine.EngineWindow.Keyboard[Key.W])
                    camera.Move(0f, 0.1f, 0f);
                if (Engine.EngineWindow.Keyboard[Key.S])
                    camera.Move(0f, -0.1f, 0f);
                if (Engine.EngineWindow.Keyboard[Key.A])
                    camera.Move(-0.1f, 0f, 0f);
                if (Engine.EngineWindow.Keyboard[Key.D])
                    camera.Move(0.1f, 0f, 0f);
                if (Engine.EngineWindow.Keyboard[Key.Q])
                    camera.Move(0f, 0f, -0.1f);
                if (Engine.EngineWindow.Keyboard[Key.E])
                    camera.Move(0f, 0f, 0.1f);
            }
        }
    }
}
