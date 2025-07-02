using System.IO;
using System.Text.Json;

namespace Cerm.Config
{
    public class ConfigService : IConfigService
    {
        private readonly string configPath = "config.json";
        private AppConfig config = null;

        public AppConfig Get() => config;

        public void Load()
        {
            if (!File.Exists(configPath))
            {
                config = new AppConfig();
            }

            try
            {
                string json = File.ReadAllText(configPath);
                config = JsonSerializer.Deserialize<AppConfig>(json) ?? new AppConfig();
            }
            catch
            {
                config = new AppConfig();
            }
        }

        public void Save()
        {
            try
            {
                string json = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(configPath, json);
            }
            catch
            {
                
            }
        }
    }
}