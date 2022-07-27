using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.EventArgs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Million_lords_Helper.Algorithm;
using Million_lords_Helper.Algorithm.Interfaces;
using Million_lords_Helper.Commands.SlashHandler;
using Million_lords_Helper.Enums;

namespace Million_lords_Helper;

public class Program
{
    public static DiscordClient? Client { get; set; }

    public SlashCommandsExtension? Slash { get; set; }

    public static void Main(string[] args)
    {
        new Program().RunBotAsync().GetAwaiter().GetResult();
    }

    public async Task RunBotAsync()
    {
        var token = Environment.GetEnvironmentVariable("DISCORD_TOKEN");

        var cfg = new DiscordConfiguration
        {
            Token = token,
            TokenType = TokenType.Bot,
            AutoReconnect = true,
            MinimumLogLevel = LogLevel.Information,
            MessageCacheSize = 65536,
            LogTimestampFormat = "dd/MM/yyyy HH:mm:ss",
        };

        await using var services = RegisterServices();
        Client = new DiscordClient(cfg);
        Client.Ready += ClientOnReady;
        Client.GuildAvailable += ClientOnGuildAvailable;
        Client.ClientErrored += ClientOnClientError;

        Slash = Client.UseSlashCommands(new SlashCommandsConfiguration
        {
            Services = services
        });

        // register Assembly ? na bullshit
        Slash.RegisterCommands<GetGoldCostHandler>();
        Slash.RegisterCommands<CastleInfoHandler>();
        Slash.RegisterCommands<PlayerInfoHandler>();
        Slash.RegisterCommands<AttackMalusBonusHandler>();
        Slash.SlashCommandExecuted += OnSlashCommandExecuted;
        Slash.SlashCommandErrored += SlashOnSlashCommandError;

        await Client.ConnectAsync();
        await Task.Delay(Timeout.Infinite);
    }

    private static Task SlashOnSlashCommandError(SlashCommandsExtension sender, SlashCommandErrorEventArgs e)
    {
        sender.Client.Logger.LogError(e.Exception.Message);
        return Task.CompletedTask;
    }

    private static Task ClientOnClientError(DiscordClient sender, ClientErrorEventArgs e)
    {
        sender.Logger.Log(LogLevel.Information, $"Exception occured: {e.Exception.GetType()}: {e.Exception.Message}");
        return Task.CompletedTask;
    }

    private static Task ClientOnGuildAvailable(DiscordClient sender, GuildCreateEventArgs e)
    {
        sender.Logger.Log(LogLevel.Information, $"Guild available: {e.Guild.MemberCount} Member in {e.Guild.Name}");
        return Task.CompletedTask;
    }

    private static Task ClientOnReady(DiscordClient sender, ReadyEventArgs e)
    {
        sender.Logger.Log(LogLevel.Information, "Client is ready to process events.");
        sender.UpdateStatusAsync(new DiscordActivity("Zanou in panty", ActivityType.Watching));
        return Task.CompletedTask;
    }

    private static Task OnSlashCommandExecuted(SlashCommandsExtension sender, SlashCommandExecutedEventArgs e)
    {
        sender.Client.Logger.Log(LogLevel.Information, $"Member {e.Context.Interaction.User.Username} used {e.Context.CommandName} in {e.Context.Guild.Name}");
        return Task.CompletedTask;
    }

    private static ServiceProvider RegisterServices()
    {
        var services = new ServiceCollection();
        services.AddSingleton<IGoldUpgradeCost>(new GenericAlgorithm(AlgorithmConstance.Constance[AlgorithmDataType.UpgradeGoldsRequired]));
        services.AddSingleton<IProductionHourly>(new GenericAlgorithm(AlgorithmConstance.Constance[AlgorithmDataType.Production]));
        services.AddSingleton<IRewardsLevelUp>(new GenericAlgorithm(AlgorithmConstance.Constance[AlgorithmDataType.Rewards]));
        services.AddSingleton<IXpNeeded>(new GenericAlgorithm(AlgorithmConstance.Constance[AlgorithmDataType.XP]));
        services.AddSingleton<IWallDefense>(new GenericAlgorithm(AlgorithmConstance.Constance[AlgorithmDataType.WallDefense]));
        return services.BuildServiceProvider();
    }
}