using System.Text;

namespace SpaceWeather.Sync.Utilities;

internal class StringBuilderWithReset
{
    private StringBuilder _sb;

    public StringBuilderWithReset()
    {
        _sb = new StringBuilder();
    }

    public int Length => _sb.Length;

    public void Append(char c)
    {
        _ = _sb.Append(c);
    }

    public string Read()
    {
        return _sb.ToString();
    }

    public void Reset()
    {
        _sb = new StringBuilder();
    }

    public string ReadAndReset()
    {
        var result = Read();
        Reset();
        return result;
    }
}
