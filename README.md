# ErzaWinUtility

ErzaWinUtility is a Windows optimization and management utility built with **WPF** and **.NET 10.0**.

The project is designed for users who want a clean interface for managing selected Windows settings, installing common applications, applying system tweaks, and performing basic maintenance tasks.

## Development Note

This project is my personal learning environment where I am actively improving my skills in C#, WPF, and Windows API interaction.

The main goal of this project is to better understand how desktop applications can interact with Windows system components such as services, registry settings, power plans, network configuration, and package management tools.

> [!WARNING]
> This project is currently in early development / beta.
>
> Some features may not work correctly on every system configuration. Use this application with caution, especially when applying system-level tweaks.

## Current Features

### App Installer

The application includes a Winget-based installer for quickly deploying popular Windows applications.

Features include:

- one-click installation of 30+ popular applications,
- automated package deployment through Windows Package Manager,
- silent installation flow where supported,
- automatic acceptance of package agreements.

### System Optimization

ErzaWinUtility includes a set of system optimization options focused on performance and reducing unwanted Windows components.

Available features include:

- deployment of the custom **Erza Ultimate Power Plan**,
- registry-based system tweaks,
- options for disabling selected Windows telemetry-related features,
- options for disabling Copilot-related components,
- safety lock requiring user confirmation before accessing critical tweaks.

### System Updates and Maintenance

The application provides basic maintenance tools for Windows Update and system repair workflows.

Available features include:

- pause or resume the Windows Update service,
- reset Windows Update components,
- clear the `SoftwareDistribution` folder,
- clear the `catroot2` folder,
- repair common cases of stuck or broken Windows Update behavior.

### System Configuration

ErzaWinUtility also includes several configuration toggles for common Windows settings.

Available features include:

- Core Isolation management,
- System Protection management,
- detailed BSoD reporting configuration,
- hidden files visibility toggle,
- file extensions visibility toggle,
- taskbar clock seconds toggle,
- DNS switching between Cloudflare, Google, AdGuard, and Quad9,
- automatic DNS cache flushing after DNS changes.

## Requirements

### Administrator Rights

Administrator privileges are required.

The application modifies system-level settings, including registry keys, Windows services, power plans, and network configuration.

### Supported Operating Systems

- Windows 10
- Windows 11

### Runtime

- .NET 10.0 Runtime

## Distribution

ErzaWinUtility is distributed as a portable application.

All required application files and libraries are included in the release package, so the tool is intended to run without a traditional installer.

## Technology Stack

- **Language:** C#
- **Framework:** .NET 10.0
- **UI:** WPF
- **System Tools:** Windows Package Manager, PowerShell, Registry Interop
- **Target Platform:** Windows 10 / Windows 11

## Safety Notice

ErzaWinUtility can change important Windows settings.

Before using optimization, update repair, registry, or security-related features, make sure you understand what the selected option does. Some changes may require a system restart, and some may affect Windows behavior, security features, or update functionality.

Use this tool at your own discretion.

## Feedback and Contributions

This is a learning project, so feedback is welcome.

If you encounter a bug, have a suggestion, or want to review the code, please open an issue in the repository.

Constructive feedback helps improve both the project and my development skills.

## Author

Created by 9Erza.
