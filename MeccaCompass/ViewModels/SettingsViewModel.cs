using System.Windows.Input;
using MeccaCompass.Services;
using MeccaCompass.Models;

namespace MeccaCompass.ViewModels;

public class SettingsViewModel : BaseViewModel
{
    private readonly IGeolocationService _geolocationService;
    private readonly IPreferences _preferences;
    
    private double _manualLatitude;
    private double _manualLongitude;
    private bool _useManualLocation;
    private bool _useDarkMode;

    public SettingsViewModel(IGeolocationService geolocationService, IPreferences preferences)
    {
        Title = "Settings";
        
        _geolocationService = geolocationService;
        _preferences = preferences;
        
        // Initialize commands
        SaveCommand = new Command(async () => await SaveSettingsAsync());
        CancelCommand = new Command(async () => await CancelAsync());
        
        // Load settings
        LoadSettings();
    }
    
    public double ManualLatitude
    {
        get => _manualLatitude;
        set => SetProperty(ref _manualLatitude, value);
    }
    
    public double ManualLongitude
    {
        get => _manualLongitude;
        set => SetProperty(ref _manualLongitude, value);
    }
    
    public bool UseManualLocation
    {
        get => _useManualLocation;
        set => SetProperty(ref _useManualLocation, value);
    }
    
    public bool UseDarkMode
    {
        get => _useDarkMode;
        set => SetProperty(ref _useDarkMode, value);
    }
    
    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }
    
    private void LoadSettings()
    {
        // Load preferences
        UseManualLocation = _preferences.Get("UseManualLocation", false);
        ManualLatitude = _preferences.Get("ManualLatitude", 0.0);
        ManualLongitude = _preferences.Get("ManualLongitude", 0.0);
        UseDarkMode = _preferences.Get("UseDarkMode", false);
    }
    
    private async Task SaveSettingsAsync()
    {
        IsBusy = true;
        
        try
        {
            // Save preferences
            _preferences.Set("UseManualLocation", UseManualLocation);
            _preferences.Set("ManualLatitude", ManualLatitude);
            _preferences.Set("ManualLongitude", ManualLongitude);
            _preferences.Set("UseDarkMode", UseDarkMode);
            
            // If using manual location, save it
            if (UseManualLocation)
            {
                var location = new Location(ManualLatitude, ManualLongitude);
                await _geolocationService.SaveLocationAsync(location);
            }
            
            // Apply theme change
            ApplyTheme();
            
            // Navigate back
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Save settings error: {ex.Message}");
        }
        
        IsBusy = false;
    }
    
    private async Task CancelAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
    
    private void ApplyTheme()
    {
        if (Application.Current == null)
            return;
            
        Application.Current.UserAppTheme = UseDarkMode ? 
            AppTheme.Dark : 
            AppTheme.Light;
    }
}