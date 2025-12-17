# FitMan - Gym Management Software

A comprehensive gym management system built with C# for managing members, memberships, attendance tracking, and business operations for traditional gyms.

## Features

- **Member Management**: Complete member profiles with photo, contact info, and health details
- **Membership Tracking**: Multiple membership types, renewal alerts, and freeze management
- **Attendance System**: Quick check-in with barcode/manual entry, attendance logs and reporting
- **Payment Processing**: Multi-payment methods, receipt generation, and overdue tracking
- **Staff Management**: Staff profiles, roles, and trainer assignments
- **Equipment Tracking**: Equipment inventory and maintenance schedules
- **Reporting**: Dashboard with daily metrics, attendance trends, and revenue reports

## Technology Stack

- **Framework**: .NET 8.0 (WPF Desktop Application)
- **Architecture**: Clean Architecture with MVVM pattern
- **Database**: SQLite (migrations ready for SQL Server)
- **ORM**: Entity Framework Core 8
- **UI**: MaterialDesignThemes for modern interface
- **Reporting**: LiveCharts for visualizations, QuestPDF for reports

## Prerequisites

Before running this application, you need to install:

1. **.NET 8.0 SDK** - Download from: https://dotnet.microsoft.com/download/dotnet/8.0
   - After installation, verify with: `dotnet --version`

2. **Visual Studio 2022** (recommended) or **Visual Studio Code**
   - Visual Studio 2022 Community Edition (Free): https://visualstudio.microsoft.com/downloads/
   - Workloads needed: ".NET desktop development"

## Project Structure

```
FitMan/
├── src/
│   ├── FitMan.Domain/           # Core entities, enums, interfaces
│   ├── FitMan.Application/      # Business logic, DTOs, services
│   ├── FitMan.Infrastructure/   # Data access, EF Core, repositories
│   └── FitMan.WPF/             # Desktop UI with MVVM
├── tests/
│   ├── FitMan.UnitTests/       # Unit tests
│   └── FitMan.IntegrationTests/ # Integration tests
└── docs/                        # Documentation and SQL scripts
```

## Getting Started

### 1. Install .NET SDK

Download and install .NET 8.0 SDK from Microsoft's website. After installation, restart your terminal and verify:

```powershell
dotnet --version
```

### 2. Restore Dependencies

```powershell
cd e:\FitMan
dotnet restore
```

### 3. Build the Solution

```powershell
dotnet build
```

### 4. Run Database Migrations

```powershell
cd src\FitMan.Infrastructure
dotnet ef database update
```

### 5. Run the Application

```powershell
cd src\FitMan.WPF
dotnet run
```

## Database

The application uses **SQLite** for easy setup and portability:
- Database file: `FitMan.db` (created automatically in app directory)
- No server installation required
- Perfect for single-location gyms
- Easy backup (just copy the .db file)

### Migration to SQL Server (Future)

When ready for network deployment:
1. Install SQL Server Express
2. Update connection string in `appsettings.json`
3. Change provider from SQLite to SqlServer
4. Run migrations: `dotnet ef database update`

Connection string example:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.\\SQLEXPRESS;Database=FitManDB;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

## Default Login

After first run, use these credentials:
- **Username**: admin
- **Password**: admin123
- ⚠️ Change password immediately after first login

## Building Installer

To create a standalone installer:

```powershell
cd src\FitMan.WPF
dotnet publish -c Release -r win-x64 --self-contained
```

Output will be in: `src\FitMan.WPF\bin\Release\net8.0\win-x64\publish\`

## Support & Documentation

- User Manual: See `docs/UserManual.md`
- Database Schema: See `docs/DatabaseSchema.md`
- API Documentation: See `docs/API.md`

## License

Proprietary - All rights reserved

## Author

SamSunny4

---

**Note**: Please install .NET 8.0 SDK before proceeding with the setup!
