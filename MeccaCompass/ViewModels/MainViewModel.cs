using MeccaCompass.Services;
using MeccaCompass.Models;
using System.Windows.Input;

namespace MeccaCompass.ViewModels;

public class MainViewModel : BaseViewModel
{
    private readonly IGeolocationService _geolocationService;
    private readonly ICompassService _compassService;
    private readonly IQiblaCalculationService _qiblaCalculationService;
    
    private double _compassHeading;
    private double _qiblaDirection;
    private double _qiblaAngle;
    private double _distanceToMecca;
    private string _locationText;
    private bool _isLocationAvailable;
    private bool _isCompassAvailable;
    private bool _showCalibrationAlert;
    private Location _currentLocation;

    public MainViewModel(
        IGeolocationService geolocationService,
        ICompassService compassService,
        IQiblaCalculationService qiblaCalculationService)
    {
        Title = "Mecca Compass";
        
        _geolocationService = geolocationService;
        _compassService = compassService;
        _qiblaCalculationService = qiblaCalculationService;
        
        // Initialize commands
        RefreshCommand = new Command(async () => await RefreshAsync());
        NavigateToSettingsCommand = new Command(async () => await NavigateToSettingsAsync());
        CalibrateCompassCommand = new Command(async () => await CalibrateCompassAsync());
        
        // Subscribe to events
        _compassService.CompassChanged += OnCompassChanged;
        _geolocationService.LocationChanged += OnLocationChanged;
    }
    
    public double CompassHeading
    {
        get => _compassHeading;
        set => SetProperty(ref _compassHeading, value);
    }
    
    public double QiblaDirection
    {
        get => _qiblaDirection;
        set => SetProperty(ref _qiblaDirection, value);
    }
    
    public double QiblaAngle
    {
        get => _qiblaAngle;
        set => SetProperty(ref _qiblaAngle, value);
    }
    
    public double DistanceToMecca
    {
        get => _distanceToMecca;
        set => SetProperty(ref _distanceToMecca, value);
    }
    
    public string LocationText
    {
        get => _locationText;
        set => SetProperty(ref _locationText, value);
    }
    
    public bool IsLocationAvailable
    {
        get => _isLocationAvailable;
        set => SetProperty(ref _isLocationAvailable, value);
    }
    
    public bool IsCompassAvailable
    {
        get => _isCompassAvailable;
        set => SetProperty(ref _isCompassAvailable, value);
    }
    
    public bool ShowCalibrationAlert
    {
        get => _showCalibrationAlert;
        set => SetProperty(ref _showCalibrationAlert, value);
    }
    
    public ICommand RefreshCommand { get; }
    public ICommand NavigateToSettingsCommand { get; }
    public ICommand CalibrateCompassCommand { get; }
    
    public async Task InitializeAsync()
    {
        IsBusy = true;
        IsCompassAvailable = _compassService.IsCompassAvailable;
        
        try
        {
            await _compassService.StartCompassAsync();
            await RefreshLocationAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Initialization error: {ex.Message}");
        }
        
        IsBusy = false;
    }
    
    private async Task RefreshAsync()
    {
        IsBusy = true;
        
        try
        {
            await RefreshLocationAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Refresh error: {ex.Message}");
        }
        
        IsBusy = false;
    }
    
    private async Task RefreshLocationAsync()
    {
        try
        {
            _currentLocation = await _geolocationService.GetCurrentLocationAsync();
            
            if (_currentLocation != null)
            {
                IsLocationAvailable = true;
                LocationText = $"Lat: {_currentLocation.Latitude:F6}, Long: {_currentLocation.Longitude:F6}";
                CalculateQibla();
            }
            else
            {
                IsLocationAvailable = false;
                LocationText = "Location unavailable";
            }
        }
        catch (Exception ex)
        {
            IsLocationAvailable = false;
            LocationText = "Error getting location";
            System.Diagnostics.Debug.WriteLine($"Location error: {ex.Message}");
        }
    }
    
    private void CalculateQibla()
    {
        if (_currentLocation == null)
            return;
            
        // Calculate Qibla direction
        QiblaDirection = _qiblaCalculationService.CalculateQiblaDirection(_currentLocation);
        
        // Calculate distance to Mecca
        DistanceToMecca = _qiblaCalculationService.CalculateDistanceToMecca(_currentLocation);
        
        // Update the Qibla angle (relative to current compass heading)
        UpdateQiblaAngle();
    }
    
    private void UpdateQiblaAngle()
    {
        // Calculate the angle between current heading and Qibla direction
        QiblaAngle = (QiblaDirection - CompassHeading + 360) % 360;
    }
    
    private void OnCompassChanged(object sender, CompassChangedEventArgs e)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            CompassHeading = e.Heading;
            UpdateQiblaAngle();
            
            // Check if calibration is needed
            ShowCalibrationAlert = _compassService.IsCalibrationRequired;
        });
    }
    
    private void OnLocationChanged(object sender, LocationChangedEventArgs e)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            _currentLocation = e.Location;
            LocationText = $"Lat: {_currentLocation.Latitude:F6}, Long: {_currentLocation.Longitude:F6}";
            IsLocationAvailable = true;
            CalculateQibla();
        });
    }
    
    private async Task NavigateToSettingsAsync()
    {
        await Shell.Current.GoToAsync("SettingsPage");
    }
    
    private async Task CalibrateCompassAsync()
    {
        bool success = await _compassService.CalibrateCompassAsync();
        if (success)
        {
            ShowCalibrationAlert = false;
        }
    }
}