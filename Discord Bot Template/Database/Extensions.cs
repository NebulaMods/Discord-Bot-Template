namespace DiscordBotTemplate.Database;

internal static class DatabaseContextExtensions
{
    internal static async Task<int> ApplyChangesAsync(this DatabaseContext database, object? entity = null)
    {
        if (entity is not null)
            database.Update(entity);
        return await database.SaveChangesAsync();
    }
}
