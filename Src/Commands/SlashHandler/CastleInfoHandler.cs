using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Million_lords_Helper.Algorithm.Interfaces;
using Million_lords_Helper.Extension;

namespace Million_lords_Helper.Commands.SlashHandler;

public class CastleInfoHandler : ApplicationCommandModule
{
    private readonly IGoldUpgradeCost _goldUpgradeCost;
    private readonly IProductionHourly _productionHourly;
    private readonly IWallDefense _wallDefense;

    public CastleInfoHandler(IGoldUpgradeCost goldUpgradeCost, IProductionHourly productionHourly, IWallDefense wallDefense)
    {
        _goldUpgradeCost = goldUpgradeCost;
        _productionHourly = productionHourly;
        _wallDefense = wallDefense;
    }

    [SlashCommand("castle_info", "Get the Information for the castle at desired level")]
    public async Task HandleCmd(InteractionContext ctx,
        [Option("Level", "The Level of the castle")]
        [Minimum(1)]
        [Maximum(2000)]
        long level)
    {
        if (!ctx.User.CanRunCmd())
        {
            var time = UserCooldownCmd.CoolDownUser[ctx.User.Id].AddSeconds(UserCooldownCmd.BaseSecond);
            await ctx.CreateResponseAsync($"You have to wait {UserCooldownCmd.BaseSecond} sec between each cmd (Next command possible in {(time - DateTime.Now):ss} sec)", true);
            return;
        }
        var gold = _goldUpgradeCost.GetResult(level);
        var prod = _productionHourly.GetResult(level);
        var def = _wallDefense.GetResult(level);
        var embed = EmbedExtension.CreateDefaultEmbed($"Castle Level {level} {prod.NumStr()} KP (+{_productionHourly.GetIncrementResult(level).NumStr()})",
            DiscordColor.Green);
        embed.AddField("Gold", $"{prod.NumStr()}/h (+{_productionHourly.GetIncrementResult(level).NumStr()})", true);
        embed.AddField("Army", $"{prod.NumStr()}/h (+{_productionHourly.GetIncrementResult(level).NumStr()})", true);
        embed.AddEmptyField(true);
        embed.AddField("Defence", $"{level * 3}% (+3%)", true);
        embed.AddField("Wall", $"{def.NumStr()} (+{_wallDefense.GetIncrementResult(level).NumStr()})", true);
        embed.AddEmptyField(true);
        embed.AddEmptyField(true);
        embed.AddField("Gold Upgrade Required:", gold.NumStr(), true);
        embed.AddEmptyField(true);

        await ctx.CreateResponseAsync(embed: embed, true);
    }
}