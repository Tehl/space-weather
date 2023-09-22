namespace SpaceWeather.Domain.Models;

public class MagneticIndexReading
{
    public DateTimeOffset StartTimeUtc { get; set; }
    public DateTimeOffset EndTimeUtc { get; set; }
    public MagneticIndexType Type { get; set; }
    public MeasurementStation Station { get; set; }
    public double Value { get; set; }
}
