using Discord;

namespace DiscordBotTemplate.Utilities.Extensions
{
    internal static class CommandContextExtension
    {
        internal static async Task ReplyWithEmbedAsync(this IInteractionContext context, string title, string description, int? deleteTimer = null)
        {
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
            try
            {
                await context.Interaction.RespondAsync(embed: embed);
                if (deleteTimer is not null)
                {
                    _ = Task.Run(() =>
                    {
                        Thread.Sleep(TimeSpan.FromSeconds((int)deleteTimer));
                        context.Interaction.GetOriginalResponseAsync().ContinueWith(async (x) => await x.Result?.DeleteAsync());
                    });
                }
            }
            catch
            {
                await context.Interaction.ModifyOriginalResponseAsync(x => x.Embed = embed);
                if (deleteTimer is not null)
                {
                    _ = Task.Run(() =>
                    {
                        Thread.Sleep(TimeSpan.FromSeconds((int)deleteTimer));
                        context.Interaction.GetOriginalResponseAsync().ContinueWith(async (x) => await x.Result?.DeleteAsync());
                    });
                }
            }
        }
    }
}
