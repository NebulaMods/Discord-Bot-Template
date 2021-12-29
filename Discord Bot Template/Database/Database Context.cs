using Microsoft.EntityFrameworkCore;

namespace DiscordBotTemplate.Database;

internal class DatabaseContext : DbContext
{
    private readonly string _connectionString = $"server=localhost;user=root;database=test_db;password=Fuckyouapple99";
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql(_connectionString, ServerVersion.AutoDetect(_connectionString)).UseLazyLoadingProxies();
    }
    //dbsets
    public DbSet<Models.Logs.ErrorLog> Errors { get; set; }
}
