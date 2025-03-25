using System.Threading.Tasks;
using Microsoft.Maui.Devices.Sensors;
using MeccaCompass.Models;

namespace MeccaCompass.Services;

public class GeolocationService : IGeolocationService
{
    private QiblaLocation _cachedLocation;
    private readonly IPreferences _preferences;
    
    public event EventHandler<LocationChangedEventArgs> LocationChanged;

    public GeolocationService(IPreferences preferences)
    {
        _preferences = preferences;
        LoadCachedLocation();
    }

    public async Task<QiblaLocation> GetCurrentLocationAsync()
    {
        try
        {
            // Request location permission
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                if (status != PermissionStatus.Granted)
                {
                    return _cachedLocation; // Return cached location if permission is denied
                }
            }

            // Get current location with high accuracy
            var request = new GeolocationRequest(GeolocationAccuracy.Best);
            var mauilocation = await Microsoft.Maui.Devices.Sensors.Geolocation.GetLocationAsync(request);

            if (mauilocation != null)
            {
                var location = new QiblaLocation(mauilocation.Latitude, mauilocation.Longitude);
                _cachedLocation = location;
                await SaveLocationAsync(location);
                
                // Trigger location changed event
                LocationChanged?.Invoke(this, new LocationChangedEventArgs(location));
                
                return location;
            }
        }
        catch (Exception ex)
        {
            // Log error
            System.Diagnostics.Debug.WriteLine($"Error getting location: {ex.Message}");
        }

        return await GetCachedLocationAsync();
    }

    public Task<QiblaLocation> GetCachedLocationAsync()
    {
        return Task.FromResult(_cachedLocation);
    }

    public Task<bool> SaveLocationAsync(QiblaLocation location)
    {
        if (location == null)
            return Task.FromResult(false);

        try
        {
            _preferences.Set("LastLatitude", location.Latitude);
            _preferences.Set("LastLongitude", location.Longitude);
            _preferences.Set("LastLocationTimestamp", location.Timestamp.ToString());
            return Task.FromResult(true);
        }
        catch
        {
            return Task.FromResult(false);
        }
    }

    private void LoadCachedLocation()
    {
        if (_preferences.ContainsKey("LastLatitude") && _preferences.ContainsKey("LastLongitude"))
        {
            var latitude = _preferences.Get("LastLatitude", 0.0);
            var longitude = _preferences.Get("LastLongitude", 0.0);
            
            _cachedLocation = new QiblaLocation(latitude, longitude);
            
            if (_preferences.ContainsKey("LastLocationTimestamp"))
            {
                var timestampStr = _preferences.Get("LastLocationTimestamp", string.Empty);
                if (DateTimeOffset.TryParse(timestampStr, out var timestamp))
                {
                    _cachedLocation.Timestamp = timestamp;
                }
            }
        }
        else
        {
            // Default location (can be customized)
            _cachedLocation = new QiblaLocation(0, 0);
        }
    }
}