using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Million_lords_Helper.Algorithm.Interfaces;
using Million_lords_Helper.Extension;

namespace Million_lords_Helper.Commands.SlashHandler;

public class PlayerInfoHandler : ApplicationCommandModule
{
    private readonly IRewardsLevelUp _rewardsLevelUp;
    private readonly IXpNeeded _xpNeeded;

    public PlayerInfoHandler(IRewardsLevelUp rewardsLevelUp, IXpNeeded xpNeeded)
    {
        _rewardsLevelUp = rewardsLevelUp;
        _xpNeeded = xpNeeded;
    }

    [SlashCommand("player_info", "Get the Information of the player")]
    public async Task HandleCmd(InteractionContext ctx,
        [Option("Level", "Level of player")] [Minimum(1)] [Maximum(2000)]
        long level)
    {
        if (!ctx.User.CanRunCmd())
        {
            var time = UserCooldownCmd.CoolDownUser[ctx.User.Id].AddSeconds(UserCooldownCmd.BaseSecond);
            await ctx.CreateResponseAsync(
                $"You have to wait {UserCooldownCmd.BaseSecond} sec between each cmd (Next command possible in {(time - DateTime.Now):ss} sec)",
                true);
            return;
        }

        double value = 0;
        double rewardsValue = 0;
        for (long i = 1; i != level; i++)
        {
            value += _xpNeeded.GetResult(i);
            rewardsValue += _rewardsLevelUp.GetResult(i);
        }

        var embed = EmbedExtension.CreateDefaultEmbed($"Player Stats info Level {level}", DiscordColor.Orange);
        embed.AddField("Xp Needed for next Level:", _xpNeeded.GetResult(level).NumStr(), true);
        embed.AddField("TotalXp earned:", value.NumStr(), true);
        embed.AddEmptyField(true);
        embed.AddField("Rewards for next Level:", _rewardsLevelUp.GetResult(level).NumStr(), true);
        embed.AddField("Total Rewards earned:", rewardsValue.NumStr(), true);
        embed.AddEmptyField(true);
        await ctx.CreateResponseAsync(embed: embed, true);
    }
}