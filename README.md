# ErzaWinUtility 🚀

ErzaWinUtility is a high-performance Windows optimization and management suite built with **WPF** and **.NET 10.0**. It is designed for power users who want full control over their operating system through a modern, clean interface.

### 📝 A Personal Note
This project is my personal playground where I am **actively learning** C#, WPF, and Windows API interaction. I am building this tool to explore how software can interact with system internals to improve user experience and performance.

> **Status:** ⚠️ **Early Development / Beta**. Because I am still learning and testing many concepts, some features may not behave as expected on all system configurations. Use with caution!

---

## ✨ Current Features

### 📦 App Installer (Winget Engine)
* **Automated Deployment:** Fast, one-click installation for over 30+ popular applications.
* **Silent Execution:** Automatically accepts package agreements and runs installations in the background.

### 🛠️ System Optimization
* **Erza Ultimate Power Plan:** Deployment of my custom-tuned performance power profile.
* **System Tweaks:** A collection of registry-based modifications to disable Telemetry, Copilot, and other system bloat.
* **Safety Lock:** Integrated safety toggle that requires user acknowledgment before accessing critical tweaks.

### 🔄 System Updates & Maintenance
* **Update Control:** Easily Pause or Resume the Windows Update service.
* **Component Repair:** Advanced tool to reset Windows Update cache and fix stuck updates by clearing `SoftwareDistribution` and `catroot2`.

### ⚙️ System Configuration
* **Security Toggles:** Manage Core Isolation, System Protection, and detailed BSoD reporting.
* **Interface Tweaks:** Control hidden files visibility, file extensions, and taskbar clock precision (seconds).
* **DNS Switcher:** Rapidly switch between Cloudflare, Google, AdGuard, and Quad9 with automatic DNS cache flushing.

---

## ⚠️ Important Requirements
* **Administrator Rights:** Mandatory! The application modifies system-level registry keys, services, and network settings.
* **Operating System:** Windows 10 or Windows 11.
* **Framework:** .NET 10.0 Runtime.

---

## 🛠️ Technical Note (Portable)
ErzaWinUtility is distributed as a **portable version**. All necessary system libraries are included in the package to ensure it runs "out of the box" without a traditional installer.

---

## 🛠️ Technology Stack
* **Language:** C#
* **Framework:** .NET 10.0 (WPF)
* **Engine:** Windows Package Manager (Winget), PowerShell, and Registry Interop.

---

## 💬 Feedback & Contribution
Since this is a learning project, your feedback is extremely valuable. If you encounter bugs, have suggestions, or want to review my code, please open an **Issue**. Help me become a better developer!

---
*Created with ❤️ by 9Erza*
