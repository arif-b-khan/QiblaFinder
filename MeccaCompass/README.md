# Mecca Compass (Qibla Finder)

A .NET MAUI cross-platform mobile application designed to help Muslims determine the direction of the Qibla (the sacred direction toward the Kaaba in Mecca) from their current location.

## Features

- **Accurate Qibla Direction**: Uses GPS and compass to show the exact direction toward Mecca
- **Real-time Updates**: Compass rotates as you move your device
- **Distance to Mecca**: Shows how far you are from the Holy Kaaba
- **Manual Location Entry**: Can enter coordinates manually if GPS is unavailable
- **Dark/Light Mode**: Adapts to system theme or allows manual selection
- **Offline Capability**: Works without internet once location is cached

## Requirements

- iOS 14.2 or later
- Android 5.0 (API 21) or later
- Device with compass and location sensors

## Building the Application

### Prerequisites

- .NET 8.0 SDK or later
- Visual Studio 2022 or later with .NET MAUI workload, or Visual Studio Code with .NET MAUI extensions
- Xcode 13+ (for iOS/macOS builds)
- Android SDK (for Android builds)

### Build Steps

1. Clone the repository
2. Open the solution in Visual Studio or Visual Studio Code
3. Restore NuGet packages
4. Build the solution
5. Deploy to your device or emulator

## App Structure

- **Services**: Core services for geolocation, compass, and Qibla calculations
- **ViewModels**: MVVM implementation for UI logic
- **Views**: XAML UI components
- **Models**: Data models for the application

## Permissions

This app requires the following permissions:
- Location access (for determining your current coordinates)
- Compass/Magnetometer access (for determining device orientation)

## Calculation Method

The app uses the Haversine formula to calculate the bearing angle between two geographic points (your location and Mecca). The Kaaba's coordinates (21.3891° N, 39.8579° E) are used as the fixed point toward which the Qibla direction is calculated.

## Contributing

Contributions to improve the Mecca Compass app are welcome. Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Acknowledgments

- Thanks to the .NET MAUI team for providing the cross-platform framework
- Special acknowledgment to the Muslim community for their guidance on Qibla accuracy requirements