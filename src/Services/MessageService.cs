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

    public static void ConfigFileException(string configFilePath)
    {
        string[] missingMessages =
        [
            $"Thought I had you preferences written down at '{configFilePath}', but for some reason I'm having trouble with it.",
            $"Oh dear, there is some sort of issue with your preferences at '{configFilePath}'.",
            $"Something must be terribly wrong with '{configFilePath}'.",
        ];
        CliUtils.WriteWarning(missingMessages[Random.Shared.Next(missingMessages.Length)]);

        string[] defaultingMessages =
        [
            $"I'll just reset your preferences to some sensible values.",
            $"Going to have to create a new file, but you may edit it later if you so please.",
        ];
        CliUtils.WriteInfo(defaultingMessages[Random.Shared.Next(defaultingMessages.Length)]);
    }

    public static void AskForBaseFolder()
    {
        string[] messages =
        [
            "Could you please indicate a valid base folder path:",
            "Would you mind pointing me to the right base folder:",
        ];
        CliUtils.WritePrompt(messages[Random.Shared.Next(messages.Length)]);
    }

    public static void DirectoryDoesNotExist(string folder)
    {
        string[] messages =
        [
            $"The directory '{folder}' does not seem to exist.",
            $"The path '{folder}' seems to lead nowhere.",
            $"Nothing to be found at '{folder}'.",
        ];
        CliUtils.WriteWarning(messages[Random.Shared.Next(messages.Length)]);
    }

    public static void UserCancel()
    {
        string[] messages =
        [
            "\nVery well, procedure aborted.",
            "\nOperation canceled as per your request.",
        ];
        CliUtils.WriteInfo(messages[Random.Shared.Next(messages.Length)]);
    }

    public static void PathInputEmpty()
    {
        string[] messages =
        [
            "The path cannot be empty Master.",
            "Nothing? Can't really get anything done without this.",
        ];
        CliUtils.WriteWarning(messages[Random.Shared.Next(messages.Length)]);
    }
}
