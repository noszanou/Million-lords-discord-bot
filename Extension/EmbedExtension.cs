using DSharpPlus.Entities;

namespace Million_lords_Helper.Extension;

public static class EmbedExtension
{
    public static DiscordEmbedBuilder CreateDefaultEmbed(string title, DiscordColor color)
    {
        var embed = new DiscordEmbedBuilder
        {
            Title = title,
            Color = color,
            Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail { Url = "https://cdn.discordapp.com/icons/446977715707576321/a_25008f319670f93540c2ff74e9cefbf4.gif?size=4096" },
            Timestamp = DateTimeOffset.Now,
            Footer = new DiscordEmbedBuilder.EmbedFooter { Text = "Bot made with ♥ by zanou#2824" }
        };
        return embed;
    }

    public static void AddEmptyField(this DiscordEmbedBuilder e, bool inLine)
    {
        e.AddField("\u200b", "\u200b", inLine);
    }
}