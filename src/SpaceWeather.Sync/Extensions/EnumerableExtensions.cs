namespace SpaceWeather.Sync.Extensions;

internal static class EnumerableExtensions
{
    public static IEnumerable<(TItem Item, int Idx)> WithItemIndex<TItem>(this IEnumerable<TItem> @this)
    {
        return @this.Select((x, idx) => (x, idx));
    }
}
