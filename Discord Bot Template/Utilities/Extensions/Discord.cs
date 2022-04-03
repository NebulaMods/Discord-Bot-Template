using Discord;
using Discord.Interactions;

namespace DiscordBotTemplate.Utilities.Extensions;

internal static class DiscordExtensions
{
    private static Embed MainEmbed(string title, string description, string footer, string footerIcon, string url, string imageUrl, List<EmbedFieldBuilder>? embeds = null)
    {
        EmbedBuilder? embed = new EmbedBuilder()
        {
            Title = title,
            Color = Miscallenous.RandomDiscordColour(),
            Author = new EmbedAuthorBuilder
            {
                Url = "https://nebulamods.ca",
                Name = "Nebula",
                IconUrl = "https://nebulamods.ca/logo-nebulamods.png"
            },
            Footer = new EmbedFooterBuilder
            {
                Text = footer,
                IconUrl = footerIcon
            },
            Description = description,
            Url = url,
            ThumbnailUrl = imageUrl,
        }.WithCurrentTimestamp();
        return embeds is not null ? embed.WithFields(embeds).Build() : embed.Build();
    }

    internal static async Task ReplyWithEmbedAsync(this IInteractionContext context, string title, string description, string url = "", string imageUrl = "", List<EmbedFieldBuilder>? embeds = null, int? deleteTimer = null, bool invisible = false)
    {
        if (context is not ShardedInteractionContext shardedContext)
        {
            throw new ArgumentNullException(nameof(shardedContext), "Failed to convert context to a sharded context.");
        }

        Embed? embed = MainEmbed(title, description, $"Issued by: {context.User.Username} | {context.User.Id}", context.User.GetAvatarUrl(), url, imageUrl, embeds);

        if (shardedContext.Interaction.HasResponded)
        {
            await context.Interaction.ModifyOriginalResponseAsync(x => x.Embed = embed);
        }
        else
        {
            await context.Interaction.RespondAsync(embed: embed, ephemeral: invisible);
        }

        try
        {
            if (deleteTimer is not null && invisible is false)
            {
                _ = Task.Run(async () =>
                {
                    await Task.Delay(TimeSpan.FromSeconds((int)deleteTimer));
                    await context.Interaction.DeleteOriginalResponseAsync();
                });
            }
        }
        catch { }
    }
    internal static async Task SendEmbedAsync(this IChannel channel, string title, string description, string footer, string footerIcon, string url, string imageUrl, List<EmbedFieldBuilder>? embeds = null, int? deleteTimer = null)
    {
        if (channel is not ITextChannel textChannel)
        {
            throw new ArgumentNullException(nameof(textChannel), "Channel was not a text channel");
        }

        Embed? embed = MainEmbed(title, description, footer, footerIcon, url, imageUrl, embeds);

        IUserMessage msg = await textChannel.SendMessageAsync(embed: embed);
        try
        {
            if (deleteTimer is not null && msg is not null)
            {
                _ = Task.Run(async () =>
                {
                    await Task.Delay(TimeSpan.FromSeconds((int)deleteTimer));
                    await msg.DeleteAsync();
                });
            }
        }
        catch { }
    }
}
