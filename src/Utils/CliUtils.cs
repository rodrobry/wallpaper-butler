namespace WallpaperButler.Utils;

public static class CliUtils
{
    public static void ShowWarning(string message)
    {
        ShowMessage($"WARNING: {message}", ConsoleColor.Yellow);
    }

    public static void ShowError(string message)
    {
        ShowMessage($"ERROR: {message}", ConsoleColor.Red);
    }

    public static int ExitWithError(string message)
    {
        ShowError(message);
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey(intercept: true); // Wait for input without printing the key to the screen
        return 1;
    }

    private static void ShowMessage(string message, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ResetColor();
    }
}