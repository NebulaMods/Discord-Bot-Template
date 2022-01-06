using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using DiscordBotTemplate.Utilities.Extensions;
using System.Reflection;

namespace DiscordBotTemplate.Events;

internal class InteractionHandler
{
    private readonly DiscordShardedClient _client;
    private readonly InteractionService _commands;
    private readonly IServiceProvider _services;
    internal InteractionHandler(DiscordShardedClient discord, InteractionService interactionService, IServiceProvider service)
    {
        _client = discord;
        _commands = interactionService;
        _services = service;
    }
    internal async Task InitializeAsync()
    {
        // Add the public modules that inherit InteractionModuleBase<T> to the InteractionService
        var modules = await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        _client.ShardReady += GuildReady;
        // Process the InteractionCreated payloads to execute Interactions commands
        _client.InteractionCreated += HandleInteraction;

        // Process the command execution results 
        _commands.SlashCommandExecuted += SlashCommandExecuted;
        _commands.ContextCommandExecuted += ContextCommandExecuted;
        _commands.ComponentCommandExecuted += ComponentCommandExecuted;
    }

    private async Task GuildReady(DiscordSocketClient arg) => await _commands.RegisterCommandsGloballyAsync(true);

    private async Task HandleInteraction(SocketInteraction arg) => await _commands.ExecuteCommandAsync(new ShardedInteractionContext(_client, arg), _services);

    private async Task ComponentCommandExecuted(ComponentCommandInfo arg1, IInteractionContext arg2, IResult arg3)
    {
        if (!arg3.IsSuccess)
        {
            switch (arg3.Error)
            {
                case InteractionCommandError.UnmetPrecondition:
                    await arg2.ReplyWithEmbedAsync("Error Occured", arg3.ErrorReason, 60);
                    break;
                case InteractionCommandError.UnknownCommand:
                    // implement
                    break;
                case InteractionCommandError.BadArgs:
                    await arg2.ReplyWithEmbedAsync("Error Occured", arg3.ErrorReason, 60);
                    break;
                case InteractionCommandError.Exception:
                    await using (var database = new Database.DatabaseContext())
                    {
                        var entry = new Models.Logs.ErrorLog
                        {
                            errorTime = DateTime.Now,
                            location = arg1.Name,
                            message = arg3.ErrorReason,
                        };
                        await database.AddAsync(entry);
                        await database.ApplyChangesAsync();
                    };
                    await arg2.ReplyWithEmbedAsync("Error Occured", arg3.ErrorReason, 60);
                    break;
                case InteractionCommandError.Unsuccessful:
                    await arg2.ReplyWithEmbedAsync("Error Occured", arg3.ErrorReason, 60);
                    break;
                default:
                    break;
            }
        }
    }

    private Task ContextCommandExecuted(ContextCommandInfo arg1, IInteractionContext arg2, IResult arg3)
    {
        if (!arg3.IsSuccess)
        {
            switch (arg3.Error)
            {
                case InteractionCommandError.UnmetPrecondition:
                    // implement
                    break;
                case InteractionCommandError.UnknownCommand:
                    // implement
                    break;
                case InteractionCommandError.BadArgs:
                    // implement
                    break;
                case InteractionCommandError.Exception:
                    // implement
                    break;
                case InteractionCommandError.Unsuccessful:
                    // implement
                    break;
                default:
                    break;
            }
        }

        return Task.CompletedTask;
    }

    private async Task SlashCommandExecuted(SlashCommandInfo arg1, IInteractionContext arg2, IResult arg3)
    {
        if (!arg3.IsSuccess)
        {
            switch (arg3.Error)
            {
                case InteractionCommandError.UnmetPrecondition:
                    await arg2.ReplyWithEmbedAsync("Error Occured", arg3.ErrorReason, 60);
                    break;
                case InteractionCommandError.UnknownCommand:
                    // implement
                    break;
                case InteractionCommandError.BadArgs:
                    await arg2.ReplyWithEmbedAsync("Error Occured", arg3.ErrorReason, 60);
                    break;
                case InteractionCommandError.Exception:
                    await using (var database = new Database.DatabaseContext())
                    {
                        var entry = new Models.Logs.ErrorLog
                        {
                            errorTime = DateTime.Now,
                            location = arg1.Name,
                            message = arg3.ErrorReason
                        };
                        await database.ApplyChangesAsync(entry);
                        await database.SaveChangesAsync();
                    };
                    await arg2.ReplyWithEmbedAsync("Error Occured", arg3.ErrorReason, 60);
                    break;
                case InteractionCommandError.Unsuccessful:
                    await arg2.ReplyWithEmbedAsync("Error Occured", arg3.ErrorReason, 60);
                    break;
                default:
                    break;
            }
        }
    }
}
