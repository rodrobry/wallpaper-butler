using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using WallpaperButler.Utils;

namespace WallpaperButler.Services;

public static class WallpaperCreator
{
    public static string MergeImages(string[] imagePaths, Screen[] screens)
    {
        // User Temp folder -> C:\Users\<user>\AppData\Local\Temp
        string tempPath = Path.Combine(Path.GetTempPath(), "spanned_wallpaper.png");
        bool previousImgAvailable = false;

        Rectangle virtualScreen = SystemInformation.VirtualScreen; // Exact total desktop bounds from Windows
        using Bitmap canvas = CreateCanvas(tempPath, virtualScreen, previousImgAvailable);
        using Graphics canvasGraphics = Graphics.FromImage(canvas);

        // Enable high-quality rendering modes
        canvasGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        canvasGraphics.SmoothingMode = SmoothingMode.HighQuality;
        canvasGraphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
        canvasGraphics.CompositingQuality = CompositingQuality.HighQuality;

        // Draw each image at its exact OS-defined coordinates
        for (int i = 0; i < screens.Length && i < imagePaths.Length; i++)
        {
            int screen = i + 1;
            if (string.IsNullOrEmpty(imagePaths[i]))
            {
                MessageService.MissingImgForOrientation(screen);
                MessageService.HandlingMissingImage(screen, previousImgAvailable);
                continue;
            }
            if (!File.Exists(imagePaths[i]))
            {
                MessageService.ImageNotFound(imagePaths[i]);
                MessageService.HandlingMissingImage(screen, previousImgAvailable);
                continue;
            }

            // Normalize Windows screen coordinates to 0-based canvas coordinates
            int drawX = screens[i].Bounds.X - virtualScreen.X;
            int drawY = screens[i].Bounds.Y - virtualScreen.Y;
            int width = screens[i].Bounds.Width;
            int height = screens[i].Bounds.Height;

            try
            {
                using var img = Image.FromFile(imagePaths[i]);
                canvasGraphics.DrawImage(img, drawX, drawY, width, height);
            }
            catch (Exception ex)
            {
                CliUtils.WriteWarning($"Issue drawing an image -> {ex.Message}");
            }
        }

        // Save as PNG to prevent lossy re-compression
        canvas.Save(tempPath, ImageFormat.Png);

        return tempPath;
    }

    // Create a new canvas or use the last merged wallpaper if possible
    // Using the previous wallpaper avoids black wallpapers if there are issues with the new merge.
    private static Bitmap CreateCanvas(string path, Rectangle virtualScreen, bool previousImgAvailable)
    {
        if (File.Exists(path))
        {
            using var previousWallpaper = Image.FromFile(path);
            if (previousWallpaper.Width == virtualScreen.Width &&
                previousWallpaper.Height == virtualScreen.Height)
            {
                previousImgAvailable = true;
                return new Bitmap(previousWallpaper);
            }
        }
        return new Bitmap(virtualScreen.Width, virtualScreen.Height);
    }
}
