using MeccaCompass.Models;

namespace MeccaCompass.Services;

public interface IGeolocationService
{
    Task<QiblaLocation> GetCurrentLocationAsync();
    Task<QiblaLocation> GetCachedLocationAsync();
    Task<bool> SaveLocationAsync(QiblaLocation location);
    
    event EventHandler<LocationChangedEventArgs> LocationChanged;
}

public class LocationChangedEventArgs : EventArgs
{
    public QiblaLocation Location { get; }
    
    public LocationChangedEventArgs(QiblaLocation location)
    {
        Location = location;
    }
}