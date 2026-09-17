using WallpaperButler.Services;
using WallpaperButler.Utils;

namespace WallpaperButler;

class Program
{
    static int Main()
    {
        // Initialization and Setup
        CliUtils.InitializeConsole();
        AppConfig config = AppConfig.LoadOrCreate();

        // Get image folder
        if (!Directory.Exists(config.BaseFolder))
        {
            MessageService.MissingBaseFolder(config.BaseFolder);
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
            MessageService.MissingSubfolders(config.BaseFolder);
            imagesFolder = config.BaseFolder; // Default to base folder if no subfolders
        }

        // Get all images in the target folder
        List<string> allImages = Directory.EnumerateFiles(imagesFolder, "*.*")
                                       .Where(FileUtils.IsSupportedImageFormat)
                                       .ToList();
        if (allImages.Count == 0)
        {
            MessageService.NoValidImages(imagesFolder);
            return CliUtils.Exit(1);
        }

        // Separate images into orientation
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

        // Get monitors
        var screens = Screen.AllScreens.ToArray();

        // Dynamically pick horizontal or vertical images per screen orientation
        var imagePaths = screens.Select(s =>
            s.Bounds.Width >= s.Bounds.Height
                ? PickAndRemoveRandom(horizontalImages)
                : PickAndRemoveRandom(verticalImages)
        ).ToArray();

        // Get and apply new wallpaper (stich images if multiple monitors)
        string newWallpaperPath = screens.Length > 1
            ? WallpaperCreator.MergeImages(imagePaths, screens)
            : imagePaths[0];
        WallpaperSwapper.ApplyNewWallpaper(newWallpaperPath);

        return CliUtils.Exit(0);
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
