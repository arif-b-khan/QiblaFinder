using System;
using MeccaCompass.Models;

namespace MeccaCompass.Services;

public class QiblaCalculationService : IQiblaCalculationService
{
    // Constants for Mecca coordinates (Ka'bah)
    public double MeccaLatitude => 21.3891;
    public double MeccaLongitude => 39.8579;
    
    // Earth radius in kilometers
    private const double EARTH_RADIUS = 6371.0;
    
    public double CalculateQiblaDirection(double latitude, double longitude)
    {
        // Convert degrees to radians
        double lat1 = ToRadians(latitude);
        double lon1 = ToRadians(longitude);
        double lat2 = ToRadians(MeccaLatitude);
        double lon2 = ToRadians(MeccaLongitude);
        
        // Calculate the longitude difference
        double dLon = lon2 - lon1;
        
        // Calculate the Qibla direction using the Haversine formula
        double y = Math.Sin(dLon) * Math.Cos(lat2);
        double x = Math.Cos(lat1) * Math.Sin(lat2) - Math.Sin(lat1) * Math.Cos(lat2) * Math.Cos(dLon);
        double qiblaRadians = Math.Atan2(y, x);
        
        // Convert radians to degrees and normalize to 0-360
        double qiblaDegrees = ToDegrees(qiblaRadians);
        qiblaDegrees = (qiblaDegrees + 360) % 360;
        
        return qiblaDegrees;
    }
    
    public double CalculateQiblaDirection(QiblaLocation location)
    {
        if (location == null)
            return 0;
            
        return CalculateQiblaDirection(location.Latitude, location.Longitude);
    }
    
    public double CalculateDistanceToMecca(double latitude, double longitude)
    {
        // Convert degrees to radians
        double lat1 = ToRadians(latitude);
        double lon1 = ToRadians(longitude);
        double lat2 = ToRadians(MeccaLatitude);
        double lon2 = ToRadians(MeccaLongitude);
        
        // Haversine formula for distance calculation
        double dLat = lat2 - lat1;
        double dLon = lon2 - lon1;
        
        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                  Math.Cos(lat1) * Math.Cos(lat2) *
                  Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
                  
        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        double distance = EARTH_RADIUS * c; // Distance in kilometers
        
        return distance;
    }
    
    public double CalculateDistanceToMecca(QiblaLocation location)
    {
        if (location == null)
            return 0;
            
        return CalculateDistanceToMecca(location.Latitude, location.Longitude);
    }
    
    // Helper method to convert degrees to radians
    private static double ToRadians(double degrees)
    {
        return degrees * (Math.PI / 180.0);
    }
    
    // Helper method to convert radians to degrees
    private static double ToDegrees(double radians)
    {
        return radians * (180.0 / Math.PI);
    }
}