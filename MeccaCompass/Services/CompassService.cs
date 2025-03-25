using Microsoft.Maui.Devices.Sensors;

namespace MeccaCompass.Services;

public class CompassService : ICompassService
{
    private const double CALIBRATION_THRESHOLD = 15.0; // degrees
    private double _lastReading = 0;
    private double _currentHeading = 0;
    private bool _isCalibrationRequired = false;
    
    public bool IsCompassAvailable => Compass.Default.IsSupported;
    public double CurrentHeading => _currentHeading;
    public bool IsCalibrationRequired => _isCalibrationRequired;
    
    public event EventHandler<Services.CompassChangedEventArgs> CompassChanged;
    
    public CompassService()
    {
        // Configure compass reading interval
        Compass.Default.ReadingChanged += OnCompassReadingChanged;
    }
    
    public Task StartCompassAsync()
    {
        if (IsCompassAvailable && !Compass.Default.IsMonitoring)
        {
            try
            {
                // Start monitoring the compass
                Compass.Default.Start(SensorSpeed.UI);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error starting compass: {ex.Message}");
                return Task.FromException(ex);
            }
        }
        
        return Task.CompletedTask;
    }
    
    public Task StopCompassAsync()
    {
        if (IsCompassAvailable && Compass.Default.IsMonitoring)
        {
            try
            {
                // Stop monitoring the compass
                Compass.Default.Stop();
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error stopping compass: {ex.Message}");
                return Task.FromException(ex);
            }
        }
        
        return Task.CompletedTask;
    }
    
    public Task<bool> CalibrateCompassAsync()
    {
        // Calibration would typically involve asking the user to move their device in a figure-8 pattern
        // This is a simplified implementation that just resets the calibration flag
        _isCalibrationRequired = false;
        return Task.FromResult(true);
    }
    
    private void OnCompassReadingChanged(object sender, Microsoft.Maui.Devices.Sensors.CompassChangedEventArgs e)
    {
        _currentHeading = e.Reading.HeadingMagneticNorth;
        
        // Check if calibration might be needed (erratic readings)
        if (Math.Abs(_currentHeading - _lastReading) > CALIBRATION_THRESHOLD)
        {
            _isCalibrationRequired = true;
        }
        
        _lastReading = _currentHeading;
        
        // Notify subscribers of heading change
        CompassChanged?.Invoke(this, new Services.CompassChangedEventArgs(_currentHeading));
    }
}