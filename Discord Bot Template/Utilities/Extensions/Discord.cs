using Discord;
using Discord.Interactions;

namespace DiscordBotTemplate.Utilities.Extensions;

internal static class DiscordExtensions
{
    internal static async Task ReplyWithEmbedAsync(this IInteractionContext context, string title, string description, string url = "", string imageUrl = "", List<EmbedFieldBuilder>? embeds = null, int? deleteTimer = null, bool invisible = false)
    {
        if (context is not ShardedInteractionContext shardedContext)
        {
            throw new ArgumentNullException(nameof(shardedContext), "Failed to convert context to a sharded context.");
        }

        var embed = new EmbedBuilder()
        {
            Title = title,
            Color = Miscallenous.RandomDiscordColour(),
            Author = new EmbedAuthorBuilder
            {
                Url = "",
                Name = "",
                IconUrl = ""
            },
            Footer = new EmbedFooterBuilder
            {
                Text = $"Issued by: {context.User.Username} | {context.User.Id}",
                IconUrl = context.User.GetAvatarUrl()
            },
            Description = description,
            Url = url,
            ThumbnailUrl = imageUrl,
        }.WithCurrentTimestamp().Build();
        if (embeds is not null)
        {
            embed = embed.ToEmbedBuilder().WithFields(embeds).Build();
        }

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
                _ = Task.Run(() =>
                {
                    Thread.Sleep(TimeSpan.FromSeconds((int)deleteTimer));
                    var msg = context.Interaction.GetOriginalResponseAsync().Result;
                    msg?.DeleteAsync();
                });
            }
        }
        catch { }
    }
    internal static async Task SendEmbedAsync(this IChannel channel, string title, string description, string footer, string footerIcon, List<EmbedFieldBuilder>? embeds = null, int? deleteTimer = null)
    {
        if (channel is not ITextChannel textChannel)
        {
            throw new ArgumentNullException(nameof(textChannel), "Channel was not a text channel");
        }

        var embed = new EmbedBuilder()
        {
            Title = title,
            Color = Miscallenous.RandomDiscordColour(),
            Author = new EmbedAuthorBuilder
            {
                Url = "",
                Name = "",
                IconUrl = ""
            },
            Footer = new EmbedFooterBuilder
            {
                Text = footer,
                IconUrl = footerIcon
            },
            Description = description,
        }.WithCurrentTimestamp().Build();
        if (embeds is not null)
        {
            embed = embed.ToEmbedBuilder().WithFields(embeds).Build();
        }

        IUserMessage msg = await textChannel.SendMessageAsync(embed: embed);
        try
        {
            if (deleteTimer is not null && msg is not null)
            {
                _ = Task.Run(() =>
                {
                    Thread.Sleep(TimeSpan.FromSeconds((int)deleteTimer));
                    msg.DeleteAsync();
                });
            }
        }
        catch { }
    }
}
