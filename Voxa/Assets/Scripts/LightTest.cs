using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Voxa.Objects;
using Voxa.Utils;

namespace Voxa.Assets.Scripts
{
    class LightTest : Component
    {
        public override void OnLoad()
        {
            base.OnLoad();
            Logger.AddStickyInfo("lightPos");
            Engine.EngineWindow.KeyPress += OnKeyPress;
        }

        private void OnKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 'f') {
                if (this.gameObject.Transform.GetParent() != null)
                    this.gameObject.Transform.DetachFromParent();
                else
                    this.gameObject.Transform.SetParent(Engine.Game.CurrentScene.GetActiveCamera().GetGameObject().Transform);
            }
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            Logger.UpdateStickyInfo("lightPos", this.gameObject.Transform.Position.ToString());
        }
    }
}
