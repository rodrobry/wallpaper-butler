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

        // Define image orientation subpaths
        string horizontalPath = Path.Combine(categoryFolder, "horizontal");
        string verticalPath = Path.Combine(categoryFolder, "vertical");
        if (!Directory.Exists(horizontalPath) || !Directory.Exists(verticalPath))
            return CliUtils.ExitWithError(
                 $"No 'horizontal' and/or 'vertical' subfolders in category '{Path.GetFileName(categoryFolder)}'.");

        // Fetch all category images
        List<string> horizontalImages = GetImages(horizontalPath);
        List<string> verticalImages = GetImages(verticalPath);
        if (horizontalImages.Count == 0 || verticalImages.Count == 0)
            return CliUtils.ExitWithError("One or both layout folders are empty!");

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

    // Helper to grab all images from a directory
    private static List<string> GetImages(string path) =>
        Directory.EnumerateFiles(path, "*.*")
            .Where(FileUtils.IsSupportedImageFormat)
            .ToList();

    // Helper to pick a random image and remove it from the pool to avoid duplicates
    private static string PickAndRemoveRandom(List<string> list)
    {
        if (list.Count == 0) return string.Empty;
        int index = Random.Shared.Next(list.Count);
        string chosen = list[index];
        if (list.Count > 1) list.RemoveAt(index);
        return chosen;
    }
}
