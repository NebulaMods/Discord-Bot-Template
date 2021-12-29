using DiscordBotTemplate.Database;

namespace DiscordBotTemplate.Utilities.Extensions;

internal static class ExceptionErrorExtension
{
    internal static async Task LogErrorAsync(this Exception e)
    {
        await using var database = new DatabaseContext();
        var entry = new Models.Logs.ErrorLog
        {
            errorTime = DateTime.UtcNow,
            location = e.Source,
            message = e.Message,
        };
        await database.Errors.AddAsync(entry);
        await database.ApplyChangesAsync();
    }
}
