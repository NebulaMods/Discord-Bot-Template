namespace DiscordBotTemplate.Utilities.Extensions;

internal static class DatabaseContext_Extension
{
    internal static async Task<int> ApplyChangesAsync(this Database.DatabaseContext database, object entity = null)
    {
        if (entity is not null)
            database.Update(entity);
        return await database.SaveChangesAsync();
    }
}
