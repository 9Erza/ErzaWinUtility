<h1 align="center">ErzaWinUtility</h1>

<p align="center">
  Windows optimization and management utility for system tweaks, app installation, update repair, and configuration tools.
</p>

<p align="center">
  <img src="https://img.shields.io/badge/platform-Windows%2010%20%2F%2011-0078D6?style=for-the-badge" alt="Platform Windows 10 / 11" />
  <img src="https://img.shields.io/badge/.NET-10.0-512BD4?style=for-the-badge" alt=".NET 10.0" />
  <img src="https://img.shields.io/badge/UI-WPF-5C2D91?style=for-the-badge" alt="WPF" />
  <img src="https://img.shields.io/badge/distribution-Portable-2EA44F?style=for-the-badge" alt="Portable" />
  <img src="https://img.shields.io/badge/status-Early%20Development-orange?style=for-the-badge" alt="Status Early Development" />
</p>

---

**ErzaWinUtility** is a Windows optimization and management utility built with **WPF** and **.NET 10.0**.

The application is designed to provide a clean interface for managing selected Windows settings, installing common applications, applying system tweaks, and performing basic maintenance tasks.

> Platform: **Windows 10 / Windows 11**  
> Framework: **.NET 10.0**  
> Status: **early development**

---

## Development status

ErzaWinUtility is currently an early-stage project and also my personal learning environment for improving my skills in **C#**, **WPF**, and Windows system integration.

The project helps me explore how desktop applications can interact with Windows components such as:

- services
- registry settings
- power plans
- network configuration
- package management tools
- PowerShell-based maintenance workflows

Some features may not behave the same way on every system configuration. Use the application with caution, especially when applying system-level tweaks.

---

## What ErzaWinUtility does

ErzaWinUtility is focused on practical Windows management tasks that are often performed manually through separate tools, scripts, settings pages, or command-line commands.

Current core areas:

- App installation through Windows Package Manager
- Windows optimization options
- Registry-based system tweaks
- Windows Update control and repair tools
- Selected security and system configuration toggles
- DNS profile switching
- Basic maintenance workflows
- Portable distribution model

---

## Current features

### App Installer

The application includes a Winget-based installer for quickly deploying common Windows applications.

Current features:

- one-click installation of 30+ popular applications
- automated package deployment through Windows Package Manager
- silent installation flow where supported
- automatic acceptance of package agreements where supported

---

### System Optimization

ErzaWinUtility includes selected optimization options focused on performance tuning and reducing unwanted Windows components.

Current features:

- deployment of the custom **Erza Ultimate Power Plan**
- registry-based system tweaks
- options for disabling selected telemetry-related Windows features
- options for disabling Copilot-related components
- safety lock requiring user confirmation before accessing critical tweaks

---

### System Updates and Maintenance

The application provides maintenance tools for Windows Update and common update repair workflows.

Current features:

- pause Windows Update service
- resume Windows Update service
- reset Windows Update components
- clear the `SoftwareDistribution` folder
- clear the `catroot2` folder
- repair common cases of stuck or broken Windows Update behavior

---

### System Configuration

ErzaWinUtility includes several toggles for common Windows configuration options.

Current features:

- Core Isolation management
- System Protection management
- detailed BSoD reporting configuration
- hidden files visibility toggle
- file extensions visibility toggle
- taskbar clock seconds toggle
- DNS switching between Cloudflare, Google, AdGuard, and Quad9
- automatic DNS cache flushing after DNS changes

---

## Requirements

### Administrator rights

Administrator privileges are required.

The application modifies system-level settings, including registry keys, Windows services, power plans, and network configuration.

### Supported operating systems

- Windows 10
- Windows 11

### Runtime

- .NET 10.0 Runtime

---

## Distribution

ErzaWinUtility is distributed as a portable application.

All required application files and libraries are included in the release package, so the tool is intended to run without a traditional installer.

---

## Safety notice

ErzaWinUtility can change important Windows settings.

Before using optimization, update repair, registry, security, or network-related features, make sure you understand what the selected option does.

Some changes may:

- require a system restart
- affect Windows Update behavior
- change security-related Windows settings
- modify network configuration
- change system services
- modify registry values

Use this tool at your own discretion.

---

## Important notes

ErzaWinUtility is not intended to be a magic performance booster.

The goal of the project is to provide a cleaner interface for selected Windows management tasks and to make common tweaks easier to understand and apply.

Performance improvements, system behavior changes, and compatibility may vary depending on hardware, Windows version, installed software, security settings, and user configuration.

---

## Technology stack

- **Language:** C#
- **Framework:** .NET 10.0
- **UI:** WPF
- **System tools:** Windows Package Manager, PowerShell, Registry Interop
- **Target platform:** Windows 10 / Windows 11
- **Distribution model:** Portable application

---

## Feedback and contributions

This is a learning-focused project, so feedback is welcome.

If you encounter a bug, have a suggestion, or want to review the code, please open an issue in the repository.

Constructive feedback helps improve both the application and my development skills.

---

## Author

Created by **[Eryk / 9Erza](https://github.com/9Erza)**.
