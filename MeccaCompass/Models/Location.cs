namespace MeccaCompass.Models;

public class QiblaLocation
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DateTimeOffset Timestamp { get; set; }

    public QiblaLocation(double latitude, double longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
        Timestamp = DateTimeOffset.Now;
    }
}