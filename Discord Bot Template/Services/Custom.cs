using Discord.WebSocket;

namespace DiscordBotTemplate.Services;
internal class Custom
{
    private readonly DiscordShardedClient _client;
    internal Custom(DiscordShardedClient client)
    {
        _client = client;
        _client.ShardReady += ExecuteOnShardReadyAsync;
    }

    private async Task ExecuteOnShardReadyAsync(DiscordSocketClient arg)
    {
        await arg.SetStatusAsync(Discord.UserStatus.DoNotDisturb);
        await arg.SetGameAsync("nebulamods.ca", null, Discord.ActivityType.Watching);
    }
}
