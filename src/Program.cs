using WallpaperButler.Services;
using WallpaperButler.Utils;

namespace WallpaperButler;

class Program
{
    static int Main()
    {
        AppConfig config = AppConfig.LoadOrCreate();
        if (!Directory.Exists(config.BaseFolder))
        {
            CliUtils.WriteWarning($"Base wallpapers folder does not exist: {config.BaseFolder}");
            FolderSelector.PromptForBaseFolder(config);
        }

        string[] categoryFolders = Directory.GetDirectories(config.BaseFolder);
        string imagesFolder;
        if (categoryFolders.Length > 0)
        {
            imagesFolder = FolderSelector.PromptForCategoryFolder(categoryFolders);
        }
        else
        {
            CliUtils.WriteWarning("No category/topic subfolders inside the base directory.");
            CliUtils.WriteInfo("Defaulting to the root folder.");
            imagesFolder = config.BaseFolder; // Default to base folder if no subfolders
        }

        List<string> allImages = Directory.EnumerateFiles(imagesFolder, "*.*")
                                       .Where(FileUtils.IsSupportedImageFormat)
                                       .ToList();
        if (allImages.Count == 0)
            return CliUtils.ExitWithError("Selected folder does not have valid images.");

        List<string> horizontalImages = [];
        List<string> verticalImages = [];

        foreach (var imagePath in allImages)
        {
            try
            {
                using var stream = File.OpenRead(imagePath);
                using var img = Image.FromStream(stream, useEmbeddedColorManagement: false, validateImageData: false);
                if (img.Width >= img.Height)
                    horizontalImages.Add(imagePath);
                else
                    verticalImages.Add(imagePath);
            }
            catch
            {
                continue; // Skip unreadable/corrupt files
            }
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
}
