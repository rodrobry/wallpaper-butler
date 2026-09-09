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

    public static void MissingSubfolders(string baseFolder)
    {
        string[] missingMessages =
        [
            $"I can't find any subfolders inside '{baseFolder}'.",
            $"How unfortunate, no subfolders inside '{baseFolder}'.",
            $"The base folder '{baseFolder}' seems to be empty.",
        ];
        CliUtils.WriteWarning(missingMessages[Random.Shared.Next(missingMessages.Length)]);

        string[] defaultingMessages =
        [
            $"No choice but to use all these unorganised images.",
            $"I'll use the images here, but organizing them at some point would be nice.",
            $"Not a problem, I'll work with what I have until you manage to organize them.",
        ];
        CliUtils.WriteInfo(defaultingMessages[Random.Shared.Next(defaultingMessages.Length)]);
    }

    public static void NoValidImages(string folder)
    {
        string[] messages =
        [
            $"I can't find any usable images in '{folder}'.",
            $"Oh my, no valid images to be found in '{folder}'.",
            $"The folder '{folder}' seems to be void of valid images.",
        ];
        CliUtils.WriteError(messages[Random.Shared.Next(messages.Length)]);
    }
}
