namespace DiscordBotTemplate.Database;

internal static class DatabaseContextExtensions
{
    internal static async Task<int> ApplyChangesAsync(this DatabaseContext database)
    {
        return await database.SaveChangesAsync();
    }

    internal static async Task<int> ApplyChangesAsync(this DatabaseContext database, object entity)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));
        database.Update(entity);
        return await database.SaveChangesAsync();
    }
}
