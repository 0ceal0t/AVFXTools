using Newtonsoft.Json;
using System;
using System.IO;

namespace AVFXTools.Configuration
{
    public class Paths
    {
        public static string RoamingPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AVFXTools");

        public static string GetConfigPath(string prefix) => Path.Combine(RoamingPath, $"{prefix}ConfigV1.json");

        public static void WriteSettings(string path, object value)
        {
            var folderPath = Path.GetDirectoryName(RoamingPath);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            File.WriteAllText(path, JsonConvert.SerializeObject(value, Formatting.Indented));
        }
    }
}