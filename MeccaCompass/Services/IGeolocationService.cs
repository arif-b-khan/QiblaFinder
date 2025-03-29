using MeccaCompass.Models;

namespace MeccaCompass.Services;

public interface IGeolocationService
{
    Task<Location> GetCurrentLocationAsync();
    Task<Location> GetCachedLocationAsync();
    Task<bool> SaveLocationAsync(Location location);
    
    event EventHandler<LocationChangedEventArgs> LocationChanged;
}

public class LocationChangedEventArgs : EventArgs
{
    public Location Location { get; }
    
    public LocationChangedEventArgs(Location location)
    {
        Location = location;
    }
}