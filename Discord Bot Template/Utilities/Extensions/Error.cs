using DiscordBotTemplate.Database;

namespace DiscordBotTemplate.Utilities.Extensions;

internal static class ExceptionErrorExtension
{
    internal static async Task LogErrorAsync(this Exception e, string? extraInfo = null)
    {
        await using var database = new DatabaseContext();
        var entry = new Models.LogModels.Errors
        {
            errorTime = DateTime.UtcNow,
            location = e.Source,
            message = e.Message,
            stackTrace = e.StackTrace,
            extraInfo = extraInfo
        };
        await database.Errors.AddAsync(entry);
        await database.ApplyChangesAsync();
    }
}
