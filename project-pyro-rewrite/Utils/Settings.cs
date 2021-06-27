using Newtonsoft.Json;
using System.IO;

namespace project_pyro_rewrite.Utils
{
    public struct Configuration
    {
        public int Height;
        public int Width;
        public bool Fullscreen;
        public bool HardwareModeSwitch;
    }

    public static class Settings
    {
        public static readonly string AppDataDirectory;
        public static string ConfigFilename => AppDataDirectory + "config.json";
        public static readonly Configuration DefaultConfig;
        public static Configuration Config;

        static Settings()
        {
            DefaultConfig = new Configuration
            {
                Width = 640,
                Height = 480,
                Fullscreen = false,
                HardwareModeSwitch = false,
            };

            AppDataDirectory = System.Environment.GetFolderPath(
                System.Environment.SpecialFolder.LocalApplicationData) +
                "/bitbrawl/";
        }

        public static void InitSettingsDirectory()
        {
            if (!Directory.Exists(AppDataDirectory))
                Directory.CreateDirectory(AppDataDirectory);

            if (!File.Exists(ConfigFilename))
                using (StreamWriter sw = new StreamWriter(ConfigFilename, true))
                    sw.Write("");
        }

        public static Configuration ReadFromFile()
        {
            InitSettingsDirectory();
            try
            {
                Config = JsonConvert.DeserializeObject<Configuration>(
                    File.ReadAllText(ConfigFilename));
            }
            catch (JsonSerializationException)
            {
                if (File.Exists(ConfigFilename + ".broken"))
                    File.Delete(ConfigFilename + ".broken");
                File.Move(ConfigFilename, ConfigFilename + ".broken");
                Config = DefaultConfig;
                WriteToFile();
            }
            return Config;
        }

        public static void WriteToFile()
        {
            InitSettingsDirectory();
            File.WriteAllText(ConfigFilename,
                JsonConvert.SerializeObject(Config));
        }

        /*
        [Nez.Console.Command("config-bool", "gets or sets a boolean configuration setting")]
        private static void ConfigCommand(string settingName, bool value)
        {
            if (settingName != default)
            {
                var info = typeof(Configuration).GetProperty(settingName));
                if (info != null)
                {
                }
            }
        }

        [Nez.Console.Command("config-bool", "gets or sets a boolean configuration setting")]
        private static void ConfigCommand(string settingName, int value)
        {
            if (settingName != default)
            {
                var info = typeof(Configuration).GetProperty(settingName));
                if (info != null)
                {
                }
            }
        }
        */
    }
}
