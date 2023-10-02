using System.Globalization;
using System.Text.RegularExpressions;
using SpaceWeather.Domain.Models;
using SpaceWeather.Sync.Extensions;
using SpaceWeather.Sync.Pipeline;

namespace SpaceWeather.Sync.Data;

internal partial class DailyForecastIndexTransformer : IDataTransformer<string, MagneticIndexReading>
{
    private const int NumberOfDaysInForecast = 3;
    private const int HoursPerForecastPeriod = 3;

    public MagneticIndexReading[] Transform(string rawData)
    {
        if (
            !TryLocateKpData(rawData, out var kpData)
            || !TryParseStartDate(kpData, out var startDate)
        )
        {
            return Array.Empty<MagneticIndexReading>();
        }

        var rows = kpData.SplitLines();

        var readings = new List<MagneticIndexReading>();

        foreach (var row in rows)
        {
            if (!TryParseRowData(row, out var hours, out var values))
            {
                continue;
            }

            for (var i = 0; i < NumberOfDaysInForecast; i++)
            {
                var startTimeUtc = startDate.AddDays(i).AddHours(hours);

                var reading = new MagneticIndexReading()
                {
                    StartTimeUtc = startTimeUtc,
                    EndTimeUtc = startTimeUtc.AddHours(HoursPerForecastPeriod),
                    Type = MagneticIndexType.K,
                    Station = MeasurementStation.Forecast,
                    Value = values[i]
                };

                readings.Add(reading);
            }
        }

        return readings.ToArray();
    }

    private static bool TryLocateKpData(string raw, out string kpData)
    {
        var kpDataMatch = KpDataPattern().Match(raw);
        if (kpDataMatch.Success)
        {
            kpData = kpDataMatch.Value;
            return true;
        }

        kpData = string.Empty;
        return false;
    }

    private static bool TryParseStartDate(string kpData, out DateTimeOffset startDate)
    {
        var kpTimePeriod = KpTimePeriodPattern().Match(kpData);
        if (!kpTimePeriod.Success)
        {
            startDate = default;
            return false;
        }

        var day = kpTimePeriod.Groups[2].Value;
        var month = kpTimePeriod.Groups[1].Value;
        var year = kpTimePeriod.Groups[3].Value;

        return DateTimeOffset.TryParseExact(
            $"{day} {month} {year}",
            "dd MMM yyyy",
            CultureInfo.InvariantCulture,
            DateTimeStyles.AssumeUniversal,
            out startDate
        );
    }

    private static bool TryParseRowData(string row, out int hours, out double[] values)
    {
        var kpStartTime = KpRowStartTimePattern().Match(row);
        if (!kpStartTime.Success)
        {
            hours = default;
            values = Array.Empty<double>();
            return false;
        }

        var kpRowData = KpRowDataPattern().Matches(row, kpStartTime.Value.Length);
        if (kpRowData.Count != NumberOfDaysInForecast)
        {
            hours = default;
            values = Array.Empty<double>();
            return false;
        }

        hours = int.Parse(kpStartTime.Groups[1].Value);
        values = kpRowData.Select(x => double.Parse(x.Value)).ToArray();
        return true;
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
