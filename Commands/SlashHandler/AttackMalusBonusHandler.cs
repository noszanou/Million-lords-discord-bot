using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Million_lords_Helper.Extension;

namespace Million_lords_Helper.Commands.SlashHandler;

public class AttackMalusBonusHandler : ApplicationCommandModule
{
    [SlashCommand("attack_info", "Get the Bonus or malus of KP attack")]
    public async Task HandleCmd(InteractionContext ctx,
        [Option("KP", "Kingdom power")] [Minimum(1)]
        double kp)
    {
        if (!ctx.User.CanRunCmd())
        {
            var time = UserCooldownCmd.CoolDownUser[ctx.User.Id].AddSeconds(UserCooldownCmd.BaseSecond);
            await ctx.CreateResponseAsync(
                $"You have to wait {UserCooldownCmd.BaseSecond} sec between each cmd (Next command possible in {(time - DateTime.Now):ss} sec)",
                true);
            return;
        }

        var embed = EmbedExtension.CreateDefaultEmbed($"Player attack Malus/Bonus: {kp.NumStr()}", DiscordColor.Yellow);
        var minValue = kp * 0.30 - 1;
        var maxValue = kp * 0.30 - 1;
        embed.AddField("Attack reduced:", $"Max: **{minValue.NumStr()}**", true);
        minValue = kp * 0.30;
        maxValue = kp * 0.40 - 1;
        embed.AddField("0% Exp", $"Min: **{minValue.NumStr()}**\nMax: **{maxValue.NumStr()}**", true);
        embed.AddEmptyField(true);
        minValue = kp * 0.40;
        maxValue = kp * 0.50 - 1;
        embed.AddField("50% Exp", $"Min: **{minValue.NumStr()}**\nMax: **{maxValue.NumStr()}**", true);
        minValue = kp * 0.50;
        maxValue = kp * 1.50 - 1;
        embed.AddField("100% Exp", $"Min: **{minValue.NumStr()}**\nMax: **{maxValue.NumStr()}**", true);
        embed.AddEmptyField(true);
        minValue = kp * 1.50;
        maxValue = kp * 3 - 1;
        embed.AddField("150% Exp", $"Min: **{minValue.NumStr()}**\nMax: **{maxValue.NumStr()}**", true);
        minValue = kp * 3;
        embed.AddField("200% Exp", $"Min: **{minValue.NumStr()}**", true);
        embed.AddEmptyField(true);
        await ctx.CreateResponseAsync(embed: embed, true);
    }
}