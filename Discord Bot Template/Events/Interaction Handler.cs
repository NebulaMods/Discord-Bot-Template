using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using DiscordBotTemplate.Database;
using DiscordBotTemplate.Utilities.Extensions;
using System.Reflection;

namespace DiscordBotTemplate.Events;

internal class InteractionHandler
{
    private readonly DiscordShardedClient _client;
    private readonly InteractionService _commands;
    private readonly IServiceProvider _services;
    internal InteractionHandler(DiscordShardedClient client, InteractionService interactionService, IServiceProvider service)
    {
        _client = client;
        _commands = interactionService;
        _services = service;
    }
    internal async Task InitializeAsync()
    {
        // Add the public modules that inherit InteractionModuleBase<T> to the InteractionService
        IEnumerable<ModuleInfo>? modules = await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        _client.ShardReady += ExecuteOnShardReadyAsync;
        // Process the InteractionCreated payloads to execute Interactions commands
        _client.InteractionCreated += HandleInteractionAsync;

        // Process the command execution results 
        _commands.SlashCommandExecuted += SlashCommandExecutedAsync;
        _commands.ContextCommandExecuted += ContextCommandExecutedAsync;
        _commands.ComponentCommandExecuted += ComponentCommandExecutedAsync;
    }

    private async Task ExecuteOnShardReadyAsync(DiscordSocketClient arg) => await _commands.RegisterCommandsGloballyAsync(true);

    private async Task HandleInteractionAsync(SocketInteraction arg) => await _commands.ExecuteCommandAsync(new ShardedInteractionContext(_client, arg), _services);

    private async Task ComponentCommandExecutedAsync(ComponentCommandInfo arg1, IInteractionContext arg2, IResult arg3)
    {
        if (!arg3.IsSuccess)
        {
            switch (arg3.Error)
            {
                case InteractionCommandError.UnmetPrecondition:
                    await arg2.ReplyWithEmbedAsync("Error Occured", arg3.ErrorReason, deleteTimer: 60);
                    break;
                case InteractionCommandError.UnknownCommand:
                    // implement
                    break;
                case InteractionCommandError.BadArgs:
                    await arg2.ReplyWithEmbedAsync("Error Occured", arg3.ErrorReason, deleteTimer: 60);
                    break;
                case InteractionCommandError.Exception:
                    await using (var database = new DatabaseContext())
                    {
                        var entry = new Models.LogModels.Errors
                        {
                            errorTime = DateTime.Now,
                            location = arg1.Name,
                            message = arg3.ErrorReason
                        };
                        await database.AddAsync(entry);
                        await database.ApplyChangesAsync();
                    };
                    await arg2.ReplyWithEmbedAsync("Error Occured", arg3.ErrorReason, deleteTimer: 60);
                    break;
                case InteractionCommandError.Unsuccessful:
                    await arg2.ReplyWithEmbedAsync("Error Occured", arg3.ErrorReason, deleteTimer: 60);
                    break;
                default:
                    break;
            }
        }
    }

    private async Task ContextCommandExecutedAsync(ContextCommandInfo arg1, IInteractionContext arg2, IResult arg3)
    {
        if (!arg3.IsSuccess)
        {
            switch (arg3.Error)
            {
                case InteractionCommandError.UnmetPrecondition:
                    await arg2.ReplyWithEmbedAsync("Error Occured", arg3.ErrorReason, deleteTimer: 60);
                    break;
                case InteractionCommandError.UnknownCommand:
                    // implement
                    break;
                case InteractionCommandError.BadArgs:
                    await arg2.ReplyWithEmbedAsync("Error Occured", arg3.ErrorReason, deleteTimer: 60);
                    break;
                case InteractionCommandError.Exception:
                    await using (var database = new DatabaseContext())
                    {
                        var entry = new Models.LogModels.Errors
                        {
                            errorTime = DateTime.Now,
                            location = arg1.Name,
                            message = arg3.ErrorReason
                        };
                        await database.AddAsync(entry);
                        await database.ApplyChangesAsync();
                    };
                    await arg2.ReplyWithEmbedAsync("Error Occured", arg3.ErrorReason, deleteTimer: 60);
                    break;
                case InteractionCommandError.Unsuccessful:
                    await arg2.ReplyWithEmbedAsync("Error Occured", arg3.ErrorReason, deleteTimer: 60);
                    break;
                default:
                    break;
            }
        }
    }

    private async Task SlashCommandExecutedAsync(SlashCommandInfo arg1, IInteractionContext arg2, IResult arg3)
    {
        if (!arg3.IsSuccess)
        {
            switch (arg3.Error)
            {
                case InteractionCommandError.UnmetPrecondition:
                    await arg2.ReplyWithEmbedAsync("Error Occured", arg3.ErrorReason, deleteTimer: 60);
                    break;
                case InteractionCommandError.UnknownCommand:
                    // implement
                    break;
                case InteractionCommandError.BadArgs:
                    await arg2.ReplyWithEmbedAsync("Error Occured", arg3.ErrorReason, deleteTimer: 60);
                    break;
                case InteractionCommandError.Exception:
                    await using (var database = new DatabaseContext())
                    {
                        var entry = new Models.LogModels.Errors
                        {
                            errorTime = DateTime.Now,
                            location = arg1.Name,
                            message = arg3.ErrorReason
                        };
                        await database.AddAsync(entry);
                        await database.ApplyChangesAsync();
                    };
                    await arg2.ReplyWithEmbedAsync("Error Occured", arg3.ErrorReason, deleteTimer: 60);
                    break;
                case InteractionCommandError.Unsuccessful:
                    await arg2.ReplyWithEmbedAsync("Error Occured", arg3.ErrorReason, deleteTimer: 60);
                    break;
                default:
                    break;
            }
        }
    }
}
