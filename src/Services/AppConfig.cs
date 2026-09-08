using System.Text.Json;
using WallpaperButler.Utils;

namespace WallpaperButler.Services;

public class AppConfig
{
    // %APPDATA% -> C:/Users/<user>/AppData/Roaming/
    private static readonly string ConfigDir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "WallpaperButler"
        );
    public static readonly string ConfigFilePath = Path.Combine(ConfigDir, "config.json");
    private static readonly JsonSerializerOptions SerializerOptions = new() { WriteIndented = true };

    private string _baseFolder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
            "wallpapers").Replace('\\', '/'); // C:/Users/<Username>/Pictures/wallpapers

    public string BaseFolder
    {
        get => _baseFolder;
        set => _baseFolder = value?.Replace('\\', '/') ?? string.Empty;
    }

    public static AppConfig LoadOrCreate()
    {
        if (File.Exists(ConfigFilePath))
        {
            try
            {
                string loadedJsonStr = File.ReadAllText(ConfigFilePath);
                AppConfig? loadedConfig = JsonSerializer.Deserialize<AppConfig>(loadedJsonStr);
                if (loadedConfig != null)
                    return loadedConfig;
            }
            catch (Exception)
            {
                CliUtils.WriteWarning("config.json was missing or corrupted. Resetting to defaults.");
            }
        }
        AppConfig defaultConfig = new AppConfig();
        defaultConfig.Save();
        return defaultConfig;
    }

    public void Save()
    {
        if (!Directory.Exists(ConfigDir))
            Directory.CreateDirectory(ConfigDir);
        string jsonString = JsonSerializer.Serialize(this, SerializerOptions);
        File.WriteAllText(ConfigFilePath, jsonString);
    }
}
