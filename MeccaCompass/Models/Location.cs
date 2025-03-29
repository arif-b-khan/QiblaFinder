namespace MeccaCompass.Models;

public class Location
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DateTimeOffset Timestamp { get; set; }

    public Location(double latitude, double longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
        Timestamp = DateTimeOffset.Now;
    }
}