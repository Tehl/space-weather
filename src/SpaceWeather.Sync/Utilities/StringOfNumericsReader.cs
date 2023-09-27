using System.Globalization;

namespace SpaceWeather.Sync.Utilities;

internal static class StringOfNumericsReader
{
    public static IEnumerable<T> Read<T>(string data)
        where T : IParsable<T>
    {
        return ReadNumerics(data).Select(x => T.Parse(x, CultureInfo.InvariantCulture));
    }

    private static IEnumerable<string> ReadNumerics(string data)
    {
        var sb = new StringBuilderWithReset();
        foreach (var character in data)
        {
            switch (character)
            {
                case '-':
                    if (sb.Length > 0)
                    {
                        yield return sb.ReadAndReset();
                    }
                    sb.Append(character);
                    break;

                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                case '.':
                    sb.Append(character);
                    break;

                default:
                    if (sb.Length > 0)
                    {
                        yield return sb.ReadAndReset();
                    }
                    break;
            }
        }

        if (sb.Length > 0)
        {
            yield return sb.Read();
        }
    }
}
