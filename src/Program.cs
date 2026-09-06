using WallpaperManager.Services;
using WallpaperManager.Utils;

namespace WallpaperManager;

class Program
{
    static int Main()
    {
        AppConfig config = AppConfig.LoadOrCreate();
        if (!Directory.Exists(config.BaseFolder))
        {
            CliUtils.ShowWarning($"Base wallpapers folder does not exist: {config.BaseFolder}");
            FolderSelector.PromptForBaseFolder(config);
        }

        string[] categoryFolders = Directory.GetDirectories(config.BaseFolder);
        if (categoryFolders.Length == 0)
            return CliUtils.ExitWithError("No category/topic subfolders inside the directory!");

        string categoryFolder = FolderSelector.PromptForCategoryFolder(categoryFolders);

        List<string> allImages = Directory.EnumerateFiles(categoryFolder, "*.*")
                                       .Where(FileUtils.IsSupportedImageFormat)
                                       .ToList();
        if (allImages.Count == 0)
            return CliUtils.ExitWithError("Category folder does not have valid images.");

        List<string> horizontalImages = [];
        List<string> verticalImages = [];

        foreach (var imagePath in allImages)
        {
            if (IsHorizontal(imagePath))
                horizontalImages.Add(imagePath);
            else
                verticalImages.Add(imagePath);
        }

        // Sort monitors left-to-right by physical X position
        var screens = Screen.AllScreens.OrderBy(s => s.Bounds.X).ToArray();

        // Dynamically pick horizontal or vertical images per screen orientation
        var imagePaths = screens.Select(s =>
            s.Bounds.Width >= s.Bounds.Height
                ? PickAndRemoveRandom(horizontalImages)
                : PickAndRemoveRandom(verticalImages)
        ).ToArray();

        string newWallpaperPath = WallpaperCreator.MergeImages(imagePaths, screens);
        WallpaperSwapper.ApplyNewWallpaper(newWallpaperPath);
        return 0;
    }

    // Helper to pick a random image and remove it from the pool to avoid duplicates
    private static string PickAndRemoveRandom(List<string> list)
    {
        if (list.Count == 0) return string.Empty;
        int index = Random.Shared.Next(list.Count);
        string chosen = list[index];
        if (list.Count > 1) list.RemoveAt(index);
        return chosen;
    }

    private static bool IsHorizontal(string filePath)
    {
        try
        {
            using var stream = File.OpenRead(filePath);
            using var img = Image.FromStream(stream, useEmbeddedColorManagement: false, validateImageData: false);
            return img.Width >= img.Height;
        }
        catch
        {
            return true; // Default fallback on corrupt file
        }
    }
}
