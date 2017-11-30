using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using System.Threading;
using Voxa.Rendering;
using Voxa.Logic;
using Voxa.Utils;
using SharpFont;

namespace Voxa
{
    public class Engine
    {
        public const int              VERSION_MAJOR = 0;
        public const int              VERSION_MINOR = 1;

        public static Texture         WhitePixelTexture;

        public static EngineWindow    EngineWindow { get; private set; }

        public static RenderingPool   RenderingPool { get; private set; }

        public static UniformManager  UniformManager { get; private set; }

        public static Game            Game { get; private set; }

        public static TaskQueue       TaskQueue { get; private set; }

        public static Library         FontLibrary { get; private set; }

        public Engine()
        {
        }

        public static void Start(Game gameInstance)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Logger.AddStickyInfo("voxaInfo", new LoggerMessage("V O X A - Lite 3D Engine v" + VERSION_MAJOR + "." + VERSION_MINOR, ConsoleColor.Cyan));
            Logger.Info("Starting Voxa Engine v" + VERSION_MAJOR + "." + VERSION_MINOR);
            Game = gameInstance;
            EngineWindow = new EngineWindow(gameInstance.WINDOW_WIDTH, gameInstance.WINDOW_HEIGHT);
            RenderingPool = new RenderingPool();
            UniformManager = new UniformManager();
            TaskQueue = new TaskQueue();
            FontLibrary = new Library();

            EngineWindow.Run(gameInstance.TARGET_UPDATE_RATE, 0);
        }
    }
}
