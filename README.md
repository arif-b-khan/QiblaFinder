# Mecca Compass (Qibla Finder)

A .NET MAUI cross-platform mobile application designed to help Muslims determine the direction of the Qibla (the sacred direction toward the Kaaba in Mecca) from their current location.

## Project Overview

This mobile application provides Muslims with an accurate tool to find the Qibla direction for prayer. It features a compass interface with visual indicators pointing toward Mecca from the user's current location.

### Key Features

- Real-time Qibla direction based on GPS location
- Interactive compass with Mecca pointer
- Distance calculation to Mecca
- Manual location input option
- Dark/light mode support
- Offline capability with cached location data
- Compass calibration alert

## Getting Started

### Prerequisites

- .NET 8.0 SDK with MAUI workload installed
- Visual Studio 2022 or Visual Studio Code with MAUI extensions
- For iOS builds: macOS with Xcode 13+
- For Android builds: Android SDK installed

### Building the Project

1. Clone this repository
2. Open `QiblaFinder.sln` in Visual Studio or open the folder in VS Code
3. Restore NuGet packages
4. Build and run on your target platform (Android/iOS)

## Project Structure

- **MeccaCompass/**: Main application project
  - **Models/**: Data structures including Location
  - **Services/**: Core functionality (Geolocation, Compass, Qibla calculations)
  - **ViewModels/**: MVVM implementation 
  - **Views/**: UI components and pages
  - **Platforms/**: Platform-specific implementations

## Development

This project follows MVVM architecture pattern and is built with .NET MAUI, allowing it to run on both Android and iOS from a single codebase.

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Acknowledgments

- The Muslim community for guidance on Qibla accuracy requirements
- .NET MAUI team for the cross-platform framework