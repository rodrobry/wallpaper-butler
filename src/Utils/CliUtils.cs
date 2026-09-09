using System.Reflection;

namespace WallpaperButler.Utils;

public static class CliUtils
{
    private enum Type
    {
        Info,
        Prompt,
        Success,
        Warning,
        Error
    }

    public static void WriteWarning(string message)
    {
        Write($"(Warning): {message}", Type.Warning);
    }

    public static void WriteError(string message)
    {
        Write($"(Error) {message}", Type.Error);
    }

    public static void WriteInfo(string message)
    {
        Write(message, Type.Info);
    }

    public static void WritePrompt(string message)
    {
        Write(message, Type.Prompt);
    }

    public static void WriteSuccess(string message)
    {
        Write(message, Type.Success);
    }

    public static int Exit(int status = 0)
    {
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey(intercept: true); // Wait for input without printing the key to the screen
        return status;
    }

    public static void InitializeConsole()
    {
        ConfigureConsole();
        ShowBanner();
    }

    private static void ConfigureConsole()
    {
        Console.Title = "Wallpaper Butler";
        Console.OutputEncoding = System.Text.Encoding.UTF8;
    }

    private static void ShowBanner()
    {
        string version = Assembly.GetExecutingAssembly()
            .GetName()
            .Version?
            .ToString() ?? "unknown";

        Console.WriteLine("===============================================");
        Console.WriteLine($"          Wallpaper Butler v{version}          ");
        Console.WriteLine("===============================================");
    }

    private static void Write(string message, Type type)
    {
        ConsoleColor color = type switch
        {
            Type.Info => ConsoleColor.White,
            Type.Prompt => ConsoleColor.Cyan,
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
