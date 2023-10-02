using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;
using SpaceWeather.Domain.Models;
using SpaceWeather.Sync.Extensions;
using SpaceWeather.Sync.Pipeline;
using SpaceWeather.Sync.Utilities;

namespace SpaceWeather.Sync.Data;

internal partial class DailyRecordedIndexTransformer : IDataTransformer<string, MagneticIndexReading>
{
    private const int ValuesPerStation = 9;

    public MagneticIndexReading[] Transform(string rawData)
    {
        var rows = rawData.SplitLines();

        var readings = new List<MagneticIndexReading>(rows.Length);

        foreach (var row in rows)
        {
            if (!TryParseStartDate(row, out var startDate, out var offsetIdx))
            {
                continue;
            }

            var values = StringOfNumericsReader.Read<double>(row[offsetIdx..]);
            Debug.Assert(values.Count() == 27);
            var stationData = values.Chunk(ValuesPerStation);

            foreach (var (stationEntry, idx) in stationData.WithItemIndex())
            {
                var station = GetStationName(idx);

                readings.AddRange(GetAIndexReading(station, startDate, stationEntry.First()));
                readings.AddRange(GetKIndexReadings(station, startDate, stationEntry.Skip(1)));
            }
        }

        return readings.ToArray();
    }

    private static bool TryParseStartDate(string row, out DateTimeOffset startDate, out int dateInformationLength)
    {
        var rowDate = RowDatePattern().Match(row);
        if (!rowDate.Success)
        {
            startDate = default;
            dateInformationLength = default;
            return false;
        }

        dateInformationLength = rowDate.Value.Length;

        return DateTimeOffset.TryParseExact(
            rowDate.Value,
            "yyyy MM dd ",
            CultureInfo.InvariantCulture,
            DateTimeStyles.AssumeUniversal,
            out startDate
        );
    }

    private static IEnumerable<MagneticIndexReading> GetAIndexReading(
        MeasurementStation station,
        DateTimeOffset startDate,
        double value
    )
    {
        if (value > -1)
        {
            yield return new MagneticIndexReading
            {
                StartTimeUtc = startDate,
                EndTimeUtc = startDate.AddDays(1),
                Station = station,
                Type = MagneticIndexType.A,
                Value = value
            };
        }
    }

    private static IEnumerable<MagneticIndexReading> GetKIndexReadings(
        MeasurementStation station,
        DateTimeOffset startDate,
        IEnumerable<double> values
    )
    {
        foreach (var (value, idx) in values.WithItemIndex().Where(x => x.Item > -1))
        {
            yield return new MagneticIndexReading
            {
                StartTimeUtc = startDate.AddHours(idx * 3),
                EndTimeUtc = startDate.AddHours((idx + 1) * 3),
                Station = station,
                Value = value
            };
        }
    }

    private static bool TryParseIndexReading(string rawValue, out double value)
        => double.TryParse(rawValue, out value) && value > -1;

    private static MeasurementStation GetStationName(int idx)
        => idx switch
        {
            0 => MeasurementStation.Fredericksburg,
            1 => MeasurementStation.College,
            2 => MeasurementStation.Planetary,
            _ => throw new IndexOutOfRangeException("Unexpected station index: " + idx)
        };

    [GeneratedRegex("^(\\d{4}) (\\d{2}) (\\d{2}) ")]
    private static partial Regex RowDatePattern();
}
