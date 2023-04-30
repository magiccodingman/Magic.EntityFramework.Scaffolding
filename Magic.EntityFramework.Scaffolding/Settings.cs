using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Magic.EntityFramework.Scaffolding
{
    public static class SettingsHelper
    {
        public static Settings GetSettings()
        {
            string filepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Settings.Json");

            if (!File.Exists(filepath))
            {
                using (FileStream stream = new FileStream(filepath, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        string json = JsonConvert.SerializeObject(new Settings());
                        writer.Write(json);
                    }
                }
            }

            using (FileStream stream = new FileStream(filepath, FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    string JsonSettings = reader.ReadToEnd();
                    Settings settings = Newtonsoft.Json.JsonConvert.DeserializeObject<Settings>(JsonSettings);

                    if (settings == null)
                        settings = new Settings();

                    return settings;
                }
            }
        }

        public static void SaveSettings(Settings settings)
        {
            string filepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Settings.Json");

            using (FileStream stream = new FileStream(filepath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    string json = JsonConvert.SerializeObject(settings);
                    writer.Write(json);
                }
            }
        }
    }

    public class Settings
    {
        public string NameOfDevEnvDb { get; set; }
        public string MainDbContextPath { get; set; } = "default";
        public string WorkingDirectory { get; set; } = "default";
        public string ScaffoldModelsPath { get; set; } = "default";
        public string PathToPlaceExtensionsAndMetaData { get; set; }
        public string DbContextName { get; set; } = "dbContext";
    }
}
