# Build and deploy on Xbox

## Requirements

- Windows development PC.
- Visual Studio with Universal Windows Platform development tools.
- Compatible Windows SDK and Xbox UWP development support.
- Xbox Series X in Developer Mode, configured for remote deployment.

## Create the UWP host project

1. In Visual Studio, create a C# **Blank App (Universal Windows)** project named `SteamX`.
2. Choose a Windows SDK target supported by your installed Visual Studio and Xbox development setup.
3. Copy `src/MainPage.xaml` and `src/MainPage.xaml.cs` into the project, replacing the starter page.
4. Add `src/Models/Game.cs`.
5. Add `src/Services/NetworkService.cs` and `src/Services/SteamStoreService.cs`.
6. If the project's root namespace is not `SteamX`, update the namespaces and XAML `x:Class`.
7. Open `Package.appxmanifest` and enable **Internet (Client)**.
8. Build locally. Fix any SDK/template mismatch before attempting Xbox deployment.
9. Configure the Xbox as a remote device in Visual Studio and deploy the package.

## Linux note

The repository can be edited on Linux. The classic UWP/Xbox packaging and deployment toolchain is Windows/Visual Studio based; a Linux machine alone is not a supported replacement for that toolchain.
