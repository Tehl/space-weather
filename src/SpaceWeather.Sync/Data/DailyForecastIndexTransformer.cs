using System.Globalization;
using System.Text.RegularExpressions;
using SpaceWeather.Domain.Models;
using SpaceWeather.Sync.Pipeline;

namespace SpaceWeather.Sync.Data;

internal partial class DailyForecastIndexTransformer : IDataTransformer<string, MagneticIndexReading>
{
    public MagneticIndexReading[] Transform(string rawData)
    {
        var kpData = KpDataPattern().Match(rawData);
        if (!kpData.Success)
        {
            return Array.Empty<MagneticIndexReading>();
        }

        var kpTimePeriod = KpTimePeriodPattern().Match(kpData.Value);
        if (!kpTimePeriod.Success)
        {
            return Array.Empty<MagneticIndexReading>();
        }

        var day = kpTimePeriod.Groups[2].Value;
        var month = kpTimePeriod.Groups[1].Value;
        var year = kpTimePeriod.Groups[3].Value;

        if (!DateTimeOffset.TryParseExact(
            $"{day} {month} {year}",
            "dd MMM yyyy",
            CultureInfo.InvariantCulture,
            DateTimeStyles.AssumeUniversal,
            out var startDate
        ))
        {
            return Array.Empty<MagneticIndexReading>();
        }

        var rows = kpData.Value.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

        var indices = new List<MagneticIndexReading>();

        foreach (var row in rows)
        {
            var kpStartTime = KpRowStartTimePattern().Match(row);
            if (!kpStartTime.Success)
            {
                continue;
            }

            var kpRowData = KpRowDataPattern().Matches(row, kpStartTime.Value.Length);
            if (kpRowData.Count != 3)
            {
                continue;
            }

            var hours = int.Parse(kpStartTime.Groups[1].Value);

            for (var i = 0; i < 3; i++)
            {
                var startTimeUtc = startDate.AddDays(i).AddHours(hours);

                var indexData = new MagneticIndexReading()
                {
                    StartTimeUtc = startTimeUtc,
                    EndTimeUtc = startTimeUtc.AddHours(3),
                    Type = MagneticIndexType.K,
                    Station = MeasurementStation.Forecast,
                    Value = double.Parse(kpRowData[i].Value),
                };

                indices.Add(indexData);
            }
        }

        return indices.ToArray();
    }

    [GeneratedRegex("NOAA Kp index breakdown.+?Rationale", RegexOptions.Singleline)]
    private static partial Regex KpDataPattern();

    [GeneratedRegex("([A-Za-z]{3}) (\\d{2})-[A-Za-z]{3} \\d{2} (\\d{4})")]
    private static partial Regex KpTimePeriodPattern();

    [GeneratedRegex("(\\d{2})-\\d{2}UT")]
    private static partial Regex KpRowStartTimePattern();

    [GeneratedRegex("\\d\\.\\d{2}")]
    private static partial Regex KpRowDataPattern();
}
