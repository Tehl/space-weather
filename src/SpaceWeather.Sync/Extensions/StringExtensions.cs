namespace SpaceWeather.Sync.Extensions;

internal static class StringExtensions
{
    public static string[] SplitLines(this string @this)
    {
        return @this.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
    }
}
