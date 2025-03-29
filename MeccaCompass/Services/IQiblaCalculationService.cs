using MeccaCompass.Models;

namespace MeccaCompass.Services;

public interface IQiblaCalculationService
{
    // Mecca coordinates
    double MeccaLatitude { get; }
    double MeccaLongitude { get; }
    
    // Calculate the Qibla direction (bearing) from a given location
    double CalculateQiblaDirection(double latitude, double longitude);
    
    // Calculate the Qibla direction (bearing) from a location object
    double CalculateQiblaDirection(Location location);
    
    // Calculate the distance to Mecca from a given location
    double CalculateDistanceToMecca(double latitude, double longitude);
    
    // Calculate the distance to Mecca from a location object
    double CalculateDistanceToMecca(Location location);
}