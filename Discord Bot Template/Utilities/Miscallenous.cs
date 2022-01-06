using Discord;

namespace DiscordBotTemplate.Utilities;

internal class Miscallenous
{
    internal static Color RandomDiscordColour()
    {
        return new Color(new Random().Next(0, 255), new Random().Next(0, 255), new Random().Next(0, 255));
    }
}
