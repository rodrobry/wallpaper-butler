using WallpaperButler.Utils;

namespace WallpaperButler.Services;

public static class FolderSelector
{
    public static void PromptForBaseFolder(AppConfig config)
    {
        while (!Directory.Exists(config.BaseFolder))
        {
            MessageService.AskForBaseFolder();
            string? input = Console.ReadLine()?.Trim(' ', '"');
            if (input is null)
            {
                MessageService.UserCancel();
                Environment.Exit(0);
            }
            if (string.IsNullOrWhiteSpace(input))
            {
                MessageService.PathInputEmpty();
                continue;
            }
            if (!Directory.Exists(input))
            {
                MessageService.DirectoryDoesNotExist(input);
                continue;
            }
            config.BaseFolder = input;
        }
        config.Save();
    }

    public static string PromptForCategoryFolder(string[] folderPaths)
    {
        // Display available categories
        MessageService.PresentSubfolders();
        for (int i = 0; i < folderPaths.Length; i++)
        {
            CliUtils.WriteInfo($"  [{i + 1}] {Path.GetFileName(folderPaths[i])}");
        }

        string? selectedFolder = null;
        while (selectedFolder == null)
        {
            MessageService.AskForSubfolder();
            string? input = Console.ReadLine()?.Trim();
            selectedFolder = ParseCategoryInput(input, folderPaths);
        }

        MessageService.SubfolderSelected(Path.GetFileName(selectedFolder));
        return selectedFolder;
    }

    private static string? ParseCategoryInput(string? input, string[] folderPaths)
    {
        // Stream closed or aborted (like Ctrl+Z or Ctrl+C)
        if (input is null)
        {
            MessageService.UserCancel();
            Environment.Exit(0);
        }
        // Blank input -> Random selection
        if (string.IsNullOrWhiteSpace(input))
        {
            MessageService.SelectingRandomFolder();
            return folderPaths[Random.Shared.Next(folderPaths.Length)];
        }
        // Valid index number
        if (int.TryParse(input, out int choice) &&
            choice >= 1 && choice <= folderPaths.Length)
        {
            return folderPaths[choice - 1];
        }
        // Valid name - assign to "matchedFolder" if LINQ doesn't return null (the default)
        if (folderPaths
            .FirstOrDefault(f => Path.GetFileName(f)
                .Equals(input, StringComparison.OrdinalIgnoreCase)) is string matchedFolder)
        {
            return matchedFolder;
        }

        MessageService.InvalidCategory(input);
        return null;
    }
}
