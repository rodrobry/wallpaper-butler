namespace WallpaperButler.Utils;

public static class CliUtils
{
    private enum Type
    {
        Info,
        Success,
        Warning,
        Error
    }

    public static void WriteWarning(string message)
    {
        Write($"WARNING: {message}", Type.Warning);
    }

    public static void WriteError(string message)
    {
        Write($"ERROR: {message}", Type.Error);
    }

    public static void WriteInfo(string message)
    {
        Write(message, Type.Info);
    }

    public static void WriteSuccess(string message)
    {
        Write(message, Type.Success);
    }

    public static int ExitWithError(string message)
    {
        WriteError(message);
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey(intercept: true); // Wait for input without printing the key to the screen
        return 1;
    }

    private static void Write(string message, Type type)
    {
        ConsoleColor color = type switch
        {
            Type.Info => ConsoleColor.Cyan,
            Type.Success => ConsoleColor.Green,
            Type.Warning => ConsoleColor.Yellow,
            Type.Error => ConsoleColor.Red,
            _ => Console.ForegroundColor // Fallback
        };
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ResetColor();
    }
}
