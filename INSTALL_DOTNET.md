# Install .NET 8.0 SDK - Quick Guide

## Why Do You Need This?

The FitMan Gym Management Software is built using C# and .NET 8.0. You need the .NET SDK installed on your computer to:
- Build the application from source code
- Run the application
- Install required packages (Entity Framework, UI libraries, etc.)

## Installation Steps

### Option 1: Download from Microsoft (Recommended)

1. **Visit the official .NET download page**:
   - Go to: https://dotnet.microsoft.com/download/dotnet/8.0
   - OR direct link: https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/sdk-8.0.404-windows-x64-installer

2. **Download the installer**:
   - Click on "Download .NET 8.0 SDK (v8.0.404)" for Windows x64
   - File size: ~200 MB
   - File name: `dotnet-sdk-8.0.404-win-x64.exe`

3. **Run the installer**:
   - Double-click the downloaded file
   - Click "Install" (requires administrator privileges)
   - Wait for installation to complete (~5 minutes)
   - Click "Close" when finished

4. **Verify installation**:
   - Open a **new** PowerShell window (important: must be new)
   - Type: `dotnet --version`
   - You should see: `8.0.404` or similar

### Option 2: Using Winget (Windows Package Manager)

If you have Windows 11 or Windows 10 with winget installed:

```powershell
winget install Microsoft.DotNet.SDK.8
```

### Option 3: Using Chocolatey

If you have Chocolatey package manager:

```powershell
choco install dotnet-sdk
```

## Verification

After installation, **close and reopen** your terminal, then run:

```powershell
dotnet --version
```

Expected output: `8.0.404` (or newer)

Also verify available runtimes:

```powershell
dotnet --list-sdks
dotnet --list-runtimes
```

## Troubleshooting

### "dotnet is not recognized" error

**Solution**: Close ALL terminal windows and open a new one. The PATH environment variable is only updated in new sessions.

### Installation failed

**Solutions**:
1. Run installer as Administrator (right-click → "Run as Administrator")
2. Ensure Windows is up to date (Windows Update)
3. Ensure you have at least 2 GB free disk space
4. Temporarily disable antivirus during installation

### Multiple versions installed

Check installed versions:
```powershell
dotnet --list-sdks
```

.NET 8.0 can coexist with other versions without issues.

## What Gets Installed?

- .NET 8.0 SDK (~200 MB)
- .NET Runtime (included with SDK)
- Command-line tools (`dotnet` CLI)
- Templates for creating new projects

## Next Steps

After successful installation:

1. Navigate to the FitMan folder:
   ```powershell
   cd e:\FitMan
   ```

2. Restore the project (I'll create the solution first):
   ```powershell
   dotnet restore
   ```

3. Build the project:
   ```powershell
   dotnet build
   ```

4. Run the application:
   ```powershell
   cd src\FitMan.WPF
   dotnet run
   ```

---

**Need help?** If you encounter any issues during installation, check:
- Microsoft's official troubleshooting guide: https://docs.microsoft.com/en-us/dotnet/core/install/windows
- .NET installation issues on GitHub: https://github.com/dotnet/core/issues
