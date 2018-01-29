using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Voxa.Logic;

namespace Voxa.Utils
{
    public class GameConfigurationLoader
    {
        public static Game.Configuration Load()
        {
            JObject config = JObject.Parse(ResourceManager.GetTextAssetContent("Assets/Config/game.json"));
            if (config != null)
                return parseConfig(config);
            return null;
        }

        private static Game.Configuration parseConfig(JObject configJson)
        {
            Game.Configuration config = new Game.Configuration();
            if (configJson["fullscreen"] != null) {
                config.Fullscreen = (bool)configJson["fullscreen"];
                if (config.Fullscreen) {
                    if (configJson["autoDetectResolution"] != null && (bool)configJson["autoDetectResolution"] == true) {
                        config.WindowHeight = (int)SystemParameters.PrimaryScreenHeight;
                        config.WindowWidth = (int)SystemParameters.PrimaryScreenWidth;
                    }
                }
            }
            if (configJson["resolution"] != null && (!config.Fullscreen || (configJson["autoDetectResolution"] != null && !(bool)configJson["autoDetectResolution"]))) {
                config.WindowWidth = (int)configJson["resolution"]["W"];
                config.WindowHeight = (int)configJson["resolution"]["H"];
            }
            return config;
        }
    }
}
