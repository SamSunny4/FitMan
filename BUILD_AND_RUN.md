# FitMan Gym Management Software - Setup Guide

## ⚠️ IMPORTANT: Install .NET SDK First!

Before you can build or run this application, you **MUST** install the .NET 8.0 SDK.

### Quick Installation

1. **Download .NET 8.0 SDK**
   - Visit: https://dotnet.microsoft.com/download/dotnet/8.0
   - Download the **SDK** (not just Runtime) for Windows x64
   - File size: ~200 MB

2. **Install**
   - Run the downloaded installer
   - Follow the installation wizard
   - Restart your terminal/PowerShell after installation

3. **Verify Installation**
   ```powershell
   dotnet --version
   ```
   Expected output: `8.0.404` or similar

For detailed installation instructions, see: **INSTALL_DOTNET.md**

---

## Project Overview

FitMan is a comprehensive gym management system built with:
- **C# .NET 8.0** - Modern, cross-platform framework
- **WPF** - Rich desktop UI with MaterialDesign
- **Entity Framework Core** - ORM for database access
- **SQLite** - Lightweight, file-based database (easy migration to SQL Server later)
- **Clean Architecture** - Maintainable, testable code structure

---

## Project Structure

```
FitMan/
├── FitMan.sln                          # Visual Studio Solution
├── README.md                           # Main documentation
├── INSTALL_DOTNET.md                   # .NET installation guide
├── BUILD_AND_RUN.md                    # This file
│
├── src/
│   ├── FitMan.Domain/                  # Core business entities
│   │   ├── Entities/                   # Member, Payment, Staff, etc.
│   │   ├── Enums/                      # Status enums
│   │   └── Interfaces/                 # Repository interfaces
│   │
│   ├── FitMan.Application/             # Business logic layer
│   │   ├── DTOs/                       # Data Transfer Objects (future)
│   │   └── Services/                   # Business services (future)
│   │
│   ├── FitMan.Infrastructure/          # Data access layer
│   │   ├── Data/                       # DbContext, migrations
│   │   └── Repositories/               # Repository implementations
│   │
│   └── FitMan.WPF/                     # Desktop application
│       ├── Views/                      # XAML windows
│       ├── ViewModels/                 # MVVM view models
│       ├── Services/                   # App services
│       ├── Converters/                 # Value converters
│       └── appsettings.json           # Configuration
│
├── tests/                              # Unit tests (future)
└── docs/                               # Documentation
```

---

## Build Instructions

### Option 1: Using Visual Studio (Recommended)

1. **Install Visual Studio 2022**
   - Download Community Edition (free): https://visualstudio.microsoft.com/downloads/
   - During installation, select: ".NET desktop development" workload

2. **Open Solution**
   - Double-click `FitMan.sln`
   - Visual Studio will open and restore NuGet packages automatically

3. **Build**
   - Press `Ctrl + Shift + B` or
   - Menu: Build → Build Solution

4. **Run**
   - Press `F5` or click "Start" button
   - Application will launch with login window

### Option 2: Using Command Line

1. **Navigate to project root**
   ```powershell
   cd e:\FitMan
   ```

2. **Restore dependencies**
   ```powershell
   dotnet restore
   ```
   This downloads all required NuGet packages.

3. **Build the solution**
   ```powershell
   dotnet build
   ```

4. **Run the application**
   ```powershell
   cd src\FitMan.WPF
   dotnet run
   ```

---

## First Run

### Database Initialization

On first run, the application will automatically:
1. Create `FitMan.db` database file in the application directory
2. Create all required tables
3. Seed initial data:
   - 5 membership types (Daily, Monthly, Quarterly, Half-Yearly, Annual)
   - 2 staff members (Admin, Receptionist)
   - 2 user accounts
   - 2 sample members with attendance history

### Default Login Credentials

**Administrator Account:**
- Username: `admin`
- Password: `admin123`
- Role: Full access to all features

**Receptionist Account:**
- Username: `reception`
- Password: `reception123`
- Role: Check-in and basic operations

⚠️ **Change these passwords immediately after first login!** (Feature coming soon)

---

## Database Details

### SQLite (Current Setup)

**Pros:**
- ✅ No server installation required
- ✅ Single file database (FitMan.db)
- ✅ Perfect for single-location gyms
- ✅ Easy backup (just copy the file)
- ✅ Zero configuration

**Location:**
- Development: `src\FitMan.WPF\bin\Debug\net8.0-windows\FitMan.db`
- Production: Same folder as the .exe

**Backup:**
```powershell
# Copy the database file
Copy-Item FitMan.db FitMan_backup_$(Get-Date -Format 'yyyyMMdd').db
```

### Migration to SQL Server (Future)

When you're ready for network deployment:

1. **Install SQL Server Express**
   - Download: https://www.microsoft.com/en-us/sql-server/sql-server-downloads
   - Free edition, perfect for small to medium businesses

2. **Update Connection String**
   
   Edit `src\FitMan.WPF\appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=.\\SQLEXPRESS;Database=FitManDB;Trusted_Connection=True;TrustServerCertificate=True"
     }
   }
   ```

3. **Change Database Provider**
   
   Edit `src\FitMan.WPF\App.xaml.cs`:
   ```csharp
   // Change from:
   options.UseSqlite(Configuration.GetConnectionString("DefaultConnection"))
   
   // To:
   options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
   ```

4. **Add SQL Server Package**
   ```powershell
   cd src\FitMan.Infrastructure
   dotnet add package Microsoft.EntityFrameworkCore.SqlServer
   ```

5. **Run the application** - it will create the database automatically!

---

## Features Implemented

### ✅ Core Infrastructure
- [x] Clean Architecture with 4 layers
- [x] Entity Framework Core with SQLite
- [x] Repository pattern with Unit of Work
- [x] MVVM pattern with CommunityToolkit
- [x] Material Design UI theme
- [x] Dependency injection
- [x] Logging with Serilog
- [x] User authentication with BCrypt

### ✅ Database Entities
- [x] Members (profile, contact, health info)
- [x] MembershipTypes (pricing, duration, benefits)
- [x] MemberMemberships (active memberships, expiry tracking)
- [x] AttendanceLogs (check-in/out tracking)
- [x] Payments (transactions, receipts)
- [x] Staff (employees, roles, trainers)
- [x] Equipment (inventory, maintenance)
- [x] Users (authentication)

### ✅ Application Features
- [x] Login/Logout with authentication
- [x] Main window with navigation sidebar
- [x] Dashboard (placeholder)
- [x] Members module (placeholder)
- [x] Check-In module (placeholder)
- [x] Payments module (placeholder)

### 🚧 In Progress / Planned
- [ ] Dashboard with real-time metrics
- [ ] Member registration form
- [ ] Member search and listing
- [ ] Quick check-in interface
- [ ] Payment processing
- [ ] Membership renewal
- [ ] Reports and analytics
- [ ] Receipt printing (PDF)
- [ ] Barcode/QR code scanning
- [ ] Email/SMS notifications

---

## Troubleshooting

### "dotnet is not recognized"

**Problem:** PowerShell doesn't find the `dotnet` command.

**Solution:**
1. Close ALL PowerShell/terminal windows
2. Open a NEW PowerShell window
3. Try again: `dotnet --version`

If still not working:
- Verify .NET SDK is installed: Check "Add or Remove Programs"
- Reinstall .NET SDK
- Restart your computer

### Build Errors

**"The type or namespace name 'X' could not be found"**

**Solution:**
```powershell
dotnet restore
dotnet clean
dotnet build
```

**NuGet package errors**

**Solution:**
```powershell
# Clear NuGet cache
dotnet nuget locals all --clear

# Restore packages
dotnet restore
```

### Database Errors

**"Unable to open database file"**

**Solution:**
- Check file permissions in the application folder
- Run as Administrator
- Check if antivirus is blocking file creation

**"SQLite Error 5: 'database is locked'"**

**Solution:**
- Close any other instances of the application
- Delete `FitMan.db-shm` and `FitMan.db-wal` files
- Restart the application

### Application Won't Start

**Check the logs:**
```powershell
# Logs are in the 'logs' folder
cd src\FitMan.WPF\bin\Debug\net8.0-windows\logs
Get-Content .\fitman-*.txt -Tail 50
```

---

## Development Workflow

### Adding New Features

1. **Domain Layer** - Add entities to `FitMan.Domain/Entities/`
2. **Infrastructure** - Update `DbContext`, create repositories
3. **Application** - Create services/DTOs in `FitMan.Application/`
4. **UI** - Create ViewModels and Views in `FitMan.WPF/`

### Running Migrations (When Database Changes)

```powershell
cd src\FitMan.Infrastructure

# Add a new migration
dotnet ef migrations add MigrationName --startup-project ..\FitMan.WPF

# Update database
dotnet ef database update --startup-project ..\FitMan.WPF

# Remove last migration (if needed)
dotnet ef migrations remove --startup-project ..\FitMan.WPF
```

### Debugging

**Visual Studio:**
- Set breakpoints: Click left margin of code editor
- Press F5 to start debugging
- Use Watch windows to inspect variables

**VS Code:**
- Install C# Dev Kit extension
- Press F5 to launch debugger

---

## Building for Production

### Create Standalone Executable

```powershell
cd src\FitMan.WPF

# Single-file executable (includes all dependencies)
dotnet publish -c Release -r win-x64 --self-contained -p:PublishSingleFile=true

# Output location:
# bin\Release\net8.0-windows\win-x64\publish\
```

### Distribution Package

The `publish` folder will contain:
- `FitMan.WPF.exe` - Main executable
- `FitMan.db` - Will be created on first run
- Configuration files

**To distribute:**
1. Copy the entire `publish` folder
2. Zip it: `FitMan-Setup-v1.0.zip`
3. Users just extract and run `FitMan.WPF.exe`

No installation required! ✨

---

## Next Steps

Now that the foundation is complete, the next features to implement are:

1. **Dashboard with Live Metrics**
   - Total active members
   - Today's check-ins
   - Revenue this month
   - Expiring memberships alert

2. **Member Management**
   - Add new member form
   - Search and filter members
   - Edit member details
   - View membership history

3. **Quick Check-In**
   - Search member by name/phone/card
   - Validate membership status
   - Log attendance
   - Show check-in confirmation

4. **Payment Processing**
   - Record new payment
   - Generate receipt
   - Payment history
   - Overdue tracking

5. **Reports**
   - Attendance report
   - Revenue report
   - Member analytics
   - Export to PDF/Excel

---

## Support & Contribution

### Getting Help

- Check `README.md` for project overview
- See `INSTALL_DOTNET.md` for setup issues
- Review code comments for implementation details

### Customization

The project uses clean architecture, making it easy to:
- Add new membership types
- Customize UI colors/theme
- Add new reports
- Integrate with hardware (barcode scanners, printers)
- Add new user roles and permissions

---

## License

Proprietary - All rights reserved

---

**Ready to build?** Make sure .NET SDK is installed, then run:

```powershell
cd e:\FitMan
dotnet build
cd src\FitMan.WPF
dotnet run
```

**Login with:** `admin` / `admin123`

Enjoy building your gym management system! 💪🏋️‍♂️
