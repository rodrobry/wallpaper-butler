using WallpaperButler.Utils;

namespace WallpaperButler.Services;

class MessageService
{
    public static void WallpapersUpdated()
    {
        string[] messages =
        [
            "Wallpaper updated. Much better!",
            "There you go, hope it is to your liking.",
            "How marvelous! If I may say so myself.",
            "There! Refreshing, isn't it?"
        ];
        CliUtils.WriteSuccess(messages[Random.Shared.Next(messages.Length)]);
    }

    public static void MissingBaseFolder(string baseFolder)
    {
        string[] messages =
        [
            $"I can't seem to find the base folder. There is nothing to be found at '{baseFolder}'.",
            $"The base folder seems to be missing. Wasn't it at '{baseFolder}'?",
            $"Oh my, nothing at '{baseFolder}'! My hands are tied without a base folder...",
        ];
        CliUtils.WriteWarning(messages[Random.Shared.Next(messages.Length)]);
    }
}
