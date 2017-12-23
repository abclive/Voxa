using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Voxa.Objects;
using Voxa.Utils;

namespace Voxa.Logic
{
    public class Game
    {
        public virtual int WINDOW_WIDTH { get { return 640; } }
        public virtual int WINDOW_HEIGHT { get { return 400; } }
        public virtual int RENDER_DISTANCE { get { return 1; } }
        public virtual double TARGET_UPDATE_RATE { get { return 60; } }
        public virtual bool LOCK_MOUSE { get { return true; } }
        public virtual string WINDOW_TITLE { get { return "Voxa"; } }

        public virtual Vector4 ClearColor { get { return new Vector4(0.12f, 0.48f, 0.69f, 1.0f); } }

        public double UpdateElapsedTime { get; private set; }
        public double RenderElapsedTime { get; private set; }

        public Scene  CurrentScene;

        public Game()
        {
            Logger.AddStickyInfo("engineStats", new LoggerMessage("", ConsoleColor.Blue));
        }

        public virtual void Start()
        {
            Engine.RenderingPool.Load();
        }

        public void LoadScene(Scene sceneToload)
        {
            if (CurrentScene != null) {
                CurrentScene.Unload();
            }
            CurrentScene = sceneToload;
            sceneToload.Load();
        }

        public virtual void Update(FrameEventArgs e)
        {
            this.UpdateElapsedTime = e.Time;
            if (this.CurrentScene != null) {
                this.CurrentScene.UpdateAll();
                this.debugGameInfo();
            }
        }

        public virtual void Render(FrameEventArgs e)
        {
            this.RenderElapsedTime = e.Time;
            Engine.RenderingPool.RenderPool();
        }

        private void debugGameInfo()
        {
            int count = this.CurrentScene.GetGameObjectCount();
            int fps = (int)Engine.EngineWindow.RenderFrequency;
            Logger.UpdateStickyInfo("engineStats", "GameObj Count: " + count.ToString() + " FPS: " + fps + " Update time: " + UpdateElapsedTime.ToString("0.000") + "ms Render time: " + RenderElapsedTime.ToString("0.00") + "ms");
        }
    }
}
