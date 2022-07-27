using DSharpPlus.SlashCommands;
using Million_lords_Helper.Algorithm.Interfaces;
using Million_lords_Helper.Extension;

namespace Million_lords_Helper.Commands.SlashHandler;

public class GetGoldCostHandler : ApplicationCommandModule
{
    private readonly IGoldUpgradeCost _goldUpgradeCost;

    public GetGoldCostHandler(IGoldUpgradeCost goldUpgradeCost)
    {
        _goldUpgradeCost = goldUpgradeCost;
    }

    [SlashCommand("Gold_Range", "Get the amount of gold required to x - y level")]
    public async Task HandleCmd(InteractionContext ctx,
        [Option("CurrentLevel", "The Current Level of the castle")]
        [Minimum(1)]
        [Maximum(2000)]
        long baseLevel,
        [Option("NextLevel", "The level desired")]
        [Minimum(1)]
        [Maximum(2000)]
        long nextLevel)
    {
        if (baseLevel > nextLevel)
        {
            await ctx.CreateResponseAsync("please specify a range baseLevel < NextLevel", true);
            return;
        }

        if (!ctx.User.CanRunCmd())
        {
            var time = UserCooldownCmd.CoolDownUser[ctx.User.Id].AddSeconds(UserCooldownCmd.BaseSecond);
            await ctx.CreateResponseAsync($"You have to wait {UserCooldownCmd.BaseSecond} sec between each cmd (Next command possible in {(time - DateTime.Now):ss} sec)", true);
            return;
        }

        double value = 0;
        double totalSpend = 0;
        for (long i = baseLevel; i != nextLevel; i++)
        {
            value += _goldUpgradeCost.GetResult(i);
        }
        for (long i = 1; i != nextLevel; i++)
        {
            totalSpend += _goldUpgradeCost.GetResult(i);
        }
        // Embed ?
        await ctx.CreateResponseAsync($"{ctx.User.Mention}\n" +
                                      $"**Gold Required for Level {baseLevel} to {nextLevel}:** {value.NumStr()}\n" +
                                      $"**Total Gold spend:** {totalSpend.NumStr()}", true);
    }
}