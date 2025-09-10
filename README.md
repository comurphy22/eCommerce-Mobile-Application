# eCommerce Mobile Application

A cross-platform eCommerce mobile application built with .NET MAUI (Multi-platform App UI) that runs on macOS, iOS, Android, and Windows.

## 🚀 Features

- **Cross-Platform**: Runs on macOS, iOS, Android, and Windows
- **Inventory Management**: Manage product inventory with dedicated views
- **Shopping Cart**: Full shopping cart functionality with tax calculation
- **Product Management**: Browse and manage products
- **Configuration**: Customizable app settings
- **Modern UI**: Built with .NET MAUI controls and styling

## 📋 Prerequisites

Before you begin, ensure you have the following installed:

### Required for All Platforms

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) or later
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/) with C# extension

### Platform-Specific Requirements

#### macOS / iOS Development

- **macOS**: macOS 12.0 (Monterey) or later
- **Xcode**: Latest version from the Mac App Store
- **iOS Simulator**: Included with Xcode

#### Android Development

- **Android SDK**: API level 21 or higher
- **Android Emulator** or physical Android device
- **Java Development Kit (JDK)**: JDK 11 or later

#### Windows Development

- **Windows 10**: Version 1903 or later, or Windows 11
- **Windows App SDK**: Automatically installed with Visual Studio

## 🛠️ Installation & Setup

### 1. Clone the Repository

```bash
git clone https://github.com/comurphy22/eCommerce-Mobile-Application.git
cd eCommerce-Mobile-Application
```

### 2. Restore Dependencies

```bash
dotnet restore
```

### 3. Install .NET MAUI Workloads

```bash
dotnet workload install maui
```

## 🏃‍♂️ Running the Application

### Method 1: Command Line

#### For macOS (MacCatalyst)

```bash
dotnet build Maui.eCommerce/Maui.eCommerce.csproj -t:Run -f net9.0-maccatalyst
```

#### For iOS Simulator

```bash
dotnet build Maui.eCommerce/Maui.eCommerce.csproj -t:Run -f net9.0-ios
```

#### For Android Emulator

First, make sure you have an Android emulator running or an Android device connected, then:

```bash
dotnet build Maui.eCommerce/Maui.eCommerce.csproj -t:Run -f net9.0-android
```

#### For Windows

```bash
dotnet build Maui.eCommerce/Maui.eCommerce.csproj -t:Run -f net9.0-windows10.0.19041.0
```

### Method 2: Visual Studio

1. Open `eCommerce.Mobile.sln` in Visual Studio 2022
2. Set `Maui.eCommerce` as the startup project
3. Select your target platform from the dropdown (Android, iOS, macOS, Windows)
4. Click the **Run** button or press `F5`

### Method 3: Visual Studio Code

1. Open the project folder in VS Code
2. Install the C# extension if not already installed
3. Open the integrated terminal (` Ctrl+``  ` or ` Cmd+``  `)
4. Run one of the command line options above

## 📱 Platform-Specific Notes

### macOS

- **Minimum Version**: macOS 12.0 (Monterey)
- **Architecture**: Supports both Intel (x64) and Apple Silicon (ARM64)
- The app runs as a Mac Catalyst application

### iOS

- **Minimum Version**: iOS 15.0
- **Device Support**: iPhone and iPad
- Requires a Mac for development and deployment

### Android

- **Minimum API Level**: 21 (Android 5.0)
- **Target API Level**: Latest available
- Supports ARM64 and x86_64 architectures

### Windows

- **Minimum Version**: Windows 10 version 1903
- **Architecture**: x64, x86, and ARM64

## 🏗️ Project Structure

```
eCommerce-Mobile-Application/
├── eCommerce.Mobile.sln              # Solution file
├── Library.eCommerce/                # Shared business logic library
│   ├── Models/                       # Data models (Item, Product)
│   └── Services/                     # Business services
└── Maui.eCommerce/                   # Main MAUI application
    ├── ViewModels/                   # MVVM ViewModels
    ├── Views/                        # UI Views (XAML)
    ├── Resources/                    # App resources (icons, fonts, etc.)
    └── Platforms/                    # Platform-specific code
```

## 🔧 Troubleshooting

### Common Issues

#### Build Errors

1. **"Assets file doesn't have a target"**: Run `dotnet restore` to restore NuGet packages
2. **Missing workloads**: Install MAUI workloads with `dotnet workload install maui`

#### iOS Issues

- Ensure Xcode is installed and up to date
- Check that iOS simulator is available and running

#### Android Issues

- Verify Android SDK is properly installed
- Ensure an Android emulator is running or device is connected
- Check that USB debugging is enabled on physical devices

#### macOS Issues

- Verify you're running on macOS 12.0 or later
- Check that Xcode Command Line Tools are installed

### Getting Help

If you encounter issues:

1. Check the [.NET MAUI documentation](https://docs.microsoft.com/en-us/dotnet/maui/)
2. Review the [.NET MAUI GitHub repository](https://github.com/dotnet/maui) for known issues
3. Create an issue in this repository with details about your problem


### Building for Release

#### iOS App Store

```bash
dotnet publish Maui.eCommerce/Maui.eCommerce.csproj -f net9.0-ios -c Release
```

#### Google Play Store

```bash
dotnet publish Maui.eCommerce/Maui.eCommerce.csproj -f net9.0-android -c Release
```

#### Mac App Store

```bash
dotnet publish Maui.eCommerce/Maui.eCommerce.csproj -f net9.0-maccatalyst -c Release
```

---
