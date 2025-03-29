using MeccaCompass.ViewModels;

namespace MeccaCompass.Views;

public partial class MainPage : ContentPage
{
    private readonly MainViewModel _viewModel;

    public MainPage(MainViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }
    
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        
        // Initialize the view model
        await _viewModel.InitializeAsync();
        
        // Setup animation bindings
        SetupCompassAnimation();
    }
    
    private void SetupCompassAnimation()
    {
        // Bind rotation animations to view model properties
        _viewModel.PropertyChanged += (sender, e) => {
            if (e.PropertyName == nameof(MainViewModel.CompassHeading))
            {
                // Rotate the compass rose in the opposite direction of the device heading
                // This makes the compass appear to stay fixed while the device rotates
                MainThread.BeginInvokeOnMainThread(() => {
                    CompassRose.Rotation = -_viewModel.CompassHeading;
                });
            }
            else if (e.PropertyName == nameof(MainViewModel.QiblaAngle))
            {
                // Set the rotation of the Qibla arrow to point to Mecca
                MainThread.BeginInvokeOnMainThread(() => {
                    QiblaArrow.Rotation = _viewModel.QiblaAngle;
                });
            }
        };
    }
}