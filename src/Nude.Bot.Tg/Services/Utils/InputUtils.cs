namespace Nude.Bot.Tg.Services.Utils;

public static class InputUtils
{
    public static string[] SplitInputByComma(string input)
    {
        return input
            .Split(",")
            .Select(x => x.Trim())
            .ToArray();
    }
}