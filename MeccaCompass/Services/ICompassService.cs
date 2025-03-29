namespace MeccaCompass.Services;

public interface ICompassService
{
    bool IsCompassAvailable { get; }
    double CurrentHeading { get; }
    bool IsCalibrationRequired { get; }
    
    Task StartCompassAsync();
    Task StopCompassAsync();
    Task<bool> CalibrateCompassAsync();
    
    event EventHandler<CompassChangedEventArgs> CompassChanged;
}

public class CompassChangedEventArgs : EventArgs
{
    public double Heading { get; }
    
    public CompassChangedEventArgs(double heading)
    {
        Heading = heading;
    }
}