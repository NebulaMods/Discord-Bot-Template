using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using DiscordBotTemplate.Database;
using DiscordBotTemplate.Events;
using DiscordBotTemplate.Loggers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordBotTemplate.Services;

internal class StartupService
{ 
    private readonly DiscordShardedClient _client;
    internal StartupService() => _client = new DiscordShardedClient(new DiscordSocketConfig
    {
        LogLevel = LogSeverity.Verbose,
        AlwaysDownloadUsers = true,
        GatewayIntents = GatewayIntents.GuildMembers | GatewayIntents.AllUnprivileged,
        UseSystemClock = false,
        MessageCacheSize = 250,
        UseInteractionSnowflakeDate = true,
        AlwaysDownloadDefaultStickers = false,
        AlwaysResolveStickers = false,
    });

    internal async Task RunAsync()
    {
        var services = new ServiceCollection();
        ConfigureServices(services);
        ServiceProvider? provider = services.BuildServiceProvider();
        await using (var databse = new DatabaseContext())
            await databse.Database.MigrateAsync();
        provider.GetRequiredService<DiscordLogger>();
        await provider.GetRequiredService<InteractionHandler>().InitializeAsync();
        await _client.LoginAsync(TokenType.Bot, "token");
        await _client.StartAsync();
        await Task.Delay(Timeout.Infinite);
    }

    private void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton(_client)
        .AddSingleton<DiscordLogger>()
        .AddSingleton<Custom>()
        .AddSingleton(new Random())
        .AddSingleton<InteractionHandler>()
        .AddSingleton(new HttpClient())
        .AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordShardedClient>(), new InteractionServiceConfig
        {
            DefaultRunMode = RunMode.Async,
            LogLevel = LogSeverity.Verbose
        }));
    }
}
