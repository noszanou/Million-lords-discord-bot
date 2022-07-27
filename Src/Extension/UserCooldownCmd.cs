using DSharpPlus.Entities;

namespace Million_lords_Helper.Extension;

public static class UserCooldownCmd
{
    public static readonly double BaseSecond = 30;
    public static readonly Dictionary<ulong, DateTime> CoolDownUser = new();

    public static bool CanRunCmd(this DiscordUser user)
    {
        if (!CoolDownUser.TryGetValue(user.Id, out _))
        {
            CoolDownUser[user.Id] = DateTime.Now;
            return true;
        }

        if (CoolDownUser[user.Id].AddSeconds(BaseSecond) > DateTime.Now)
        {
            return false;
        }
        CoolDownUser[user.Id] = DateTime.Now;
        return true;
    }
}