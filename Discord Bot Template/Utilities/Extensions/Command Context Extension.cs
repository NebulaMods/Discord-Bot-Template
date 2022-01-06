using Discord;
using Discord.Interactions;

namespace DiscordBotTemplate.Utilities.Extensions;

internal static class CommandContextExtension
{
    internal static async Task ReplyWithEmbedAsync(this IInteractionContext context, string title, string description, int? deleteTimer = null, bool invisible = false)
    {
        if (context is not ShardedInteractionContext shardedContext)
            throw new ArgumentNullException(nameof(shardedContext), "Failed to convert context to a sharded context.");
        var embed = new EmbedBuilder()
        {
            Title = title,
            Color = Miscallenous.RandomDiscordColour(),
            Author = new EmbedAuthorBuilder
            {
                Url = "https://nebulamods.ca",
                Name = "Nebula Mods Inc.",
                IconUrl = "https://nebulamods.ca/content/media/images/Home.png"
            },
            Footer = new EmbedFooterBuilder
            {
                Text = $"Issued by: {context.User.Username} | {context.User.Id}",
                IconUrl = context.User.GetAvatarUrl()
            },
            Description = description,
        }.WithCurrentTimestamp().Build();
        if (shardedContext.Interaction.HasResponded)
            await context.Interaction.ModifyOriginalResponseAsync(x => x.Embed = embed);
        else
            await context.Interaction.RespondAsync(embed: embed, ephemeral: invisible);

        try
        {
            if (deleteTimer is not null)
                _ = Task.Run(() =>
                {
                    Thread.Sleep(TimeSpan.FromSeconds((int)deleteTimer));
                    var msg = context.Interaction.GetOriginalResponseAsync().Result;
                    msg.DeleteAsync();
                });
        }
        catch { }
    }
}
