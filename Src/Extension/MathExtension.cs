namespace Million_lords_Helper.Extension;

public static class MathExtension
{
    private static readonly string[] PrefixUnit = { "y", "z", "a", "f", "p", "n", "µ", "m", "", "k", "M", "G", "T", "P", "E", "Z", "Y" };

    public static string NumStr(this double num)
    {
        int log10 = (int)Math.Log10(Math.Abs(num));
        if (log10 < -27)
        {
            return "0.000";
        }

        if (log10 % -3 < 0)
        {
            log10 -= 3;
        }

        int log1000 = Math.Max(-8, Math.Min(log10 / 3, 8));

        return (num / Math.Pow(10, log1000 * 3)).ToString("###.###" + PrefixUnit[log1000 + 8]);
    }

    public static double DataPercent(this double data, double percent)
    {
        return percent / 100 * data;
    }
}