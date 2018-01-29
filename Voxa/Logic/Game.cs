using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Voxa.Objects;
using Voxa.Utils;
using System.Windows;

namespace Voxa.Logic
{
    public class Game
    {
        public class Configuration
        {
            public int     WindowWidth = 640;
            public int     WindowHeight = 400;
            public int     RenderDistance = 100;
            public double  TargetUpdateRate = 60;
            public bool    LockMouse = false;
            public string  WindowTitle = "Voxa";
            public Vector4 ClearColor = new Vector4(0.12f, 0.48f, 0.69f, 1.0f);
            public bool    Fullscreen = false;
        }

        public double UpdateElapsedTime { get; private set; }
        public double RenderElapsedTime { get; private set; }

        public Configuration Config { get; protected set; }
        public Scene  CurrentScene;

        public Game()
        {
            this.Config = new Configuration();
            Logger.AddStickyInfo("engineStats", new LoggerMessage("", ConsoleColor.Blue));
        }

        public Game(Configuration config)
        {
            this.Config = config;
        }

        protected void TryLoadConfig()
        {
            Configuration config = GameConfigurationLoader.Load();
            if (config != null) this.Config = config;
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
