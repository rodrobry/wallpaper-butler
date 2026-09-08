using WallpaperButler.Utils;

namespace WallpaperButler.Services;

class MessageService
{
    public static void WallpapersUpdated()
    {
        string[] messages =
        [
            "Wallpaper updated. Much better!",
            "There you go, Sir. Hope it is to your liking.",
            "There! Refreshing, isn't it?"
        ];
        CliUtils.WriteSuccess(messages[Random.Shared.Next(messages.Length)]);
    }
}
