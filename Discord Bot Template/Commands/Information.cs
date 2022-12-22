using System.Diagnostics;
using System.Reflection;

using Discord.Interactions;

using DiscordBotTemplate.Utilities.Extensions;

namespace DiscordBotTemplate.Commands;
internal class Information : InteractionModuleBase<ShardedInteractionContext>
{
    [SlashCommand("information", "Display information about the bot.")]
    public async Task ExecuteCommand()
    {
        Discord.Rest.RestApplication? appInfo = await Context.Client.GetApplicationInfoAsync();
        await Context.ReplyWithEmbedAsync("Information",
            $"Guild Count: {Context.Client.Guilds.Count}\n" +
            $"Guild Member Count: {(Context.Guild is null ? "N/A" : Context.Guild.MemberCount)}\n" +
            $"Developer: {(Context.Guild is null ? $"{appInfo.Owner.Username}#{appInfo.Owner.Discriminator}" : appInfo.Owner.Mention)}\n" +
            $"Uptime: <t:{((DateTimeOffset)Process.GetCurrentProcess().StartTime).ToUnixTimeSeconds()}:R>\n" +
            $"Build Version: {Assembly.GetExecutingAssembly().GetName().Version}\n" +
            $"{appInfo.Description}\n" +
            $"Terms of Service: {appInfo.TermsOfService}"
            );
    }
}
