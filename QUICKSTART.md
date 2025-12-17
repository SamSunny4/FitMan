# 🚀 Quick Start Guide - FitMan Gym Management

## ⚡ 5-Minute Setup

### Step 1: Install .NET SDK (One-time setup)
```powershell
# Check if already installed
dotnet --version

# If not installed, download from:
# https://dotnet.microsoft.com/download/dotnet/8.0
```

### Step 2: Build the Project
```powershell
cd e:\FitMan
dotnet restore
dotnet build
```

### Step 3: Run the Application
```powershell
cd src\FitMan.WPF
dotnet run
```

### Step 4: Login
- **Username:** `admin`
- **Password:** `admin123`

---

## 🎯 What Works Now

✅ **Login System** - Secure authentication with BCrypt  
✅ **Main Dashboard** - Navigation sidebar with modern UI  
✅ **Database** - SQLite with 10 tables, auto-initialized  
✅ **Sample Data** - 2 members, 5 membership types, 30 days attendance  
✅ **Architecture** - Clean Architecture, MVVM, Repository pattern  

---

## 📁 Project Files Overview

```
FitMan/
├── FitMan.sln                 ← Open this in Visual Studio
├── README.md                  ← Project overview
├── BUILD_AND_RUN.md          ← Detailed instructions
├── IMPLEMENTATION_STATUS.md   ← What's done, what's next
├── INSTALL_DOTNET.md         ← .NET installation help
└── src/
    ├── FitMan.Domain/         ← Entities (Member, Payment, etc.)
    ├── FitMan.Application/    ← Business logic (future)
    ├── FitMan.Infrastructure/ ← Database (EF Core + SQLite)
    └── FitMan.WPF/           ← Desktop app (login works!)
```

---

## 🛠️ Using Visual Studio (Recommended)

1. **Install Visual Studio 2022 Community** (free)
   - https://visualstudio.microsoft.com/downloads/
   - Select ".NET desktop development" workload

2. **Open Solution**
   - Double-click `FitMan.sln`
   - Wait for packages to restore

3. **Run**
   - Press `F5` or click Start button
   - Login window appears

---

## 📊 Database

**Location:** `src\FitMan.WPF\bin\Debug\net8.0-windows\FitMan.db`

**View Database:**
- Download DB Browser for SQLite: https://sqlitebrowser.org/
- Open `FitMan.db` to explore tables

**Backup:**
```powershell
Copy-Item FitMan.db FitMan_backup.db
```

---

## 👥 Sample Data Included

### Users (for login)
| Username  | Password       | Role          |
|-----------|----------------|---------------|
| admin     | admin123       | Administrator |
| reception | reception123   | Receptionist  |

### Members
- Sarah Johnson (GYM001) - Expired monthly membership
- Michael Smith (GYM002) - Active annual membership with 30 days attendance

### Membership Types
- Daily Pass - $10 (1 day)
- Monthly - $50 (30 days)
- Quarterly - $135 (90 days)
- Half-Yearly - $250 (180 days)
- Annual - $450 (365 days)

---

## 🎨 What You'll See

1. **Login Window**
   - Material Design theme
   - Purple color scheme
   - Username/password fields

2. **Main Window**
   - Left sidebar with navigation
   - Dashboard, Members, Check-In, Payments
   - Currently showing placeholder views

---

## 🚧 What's NOT Implemented Yet

The foundation is complete, but these need implementation:

❌ **Dashboard** - Will show metrics (member count, check-ins, revenue)  
❌ **Members Module** - Add/edit/search members  
❌ **Check-In** - Quick attendance logging  
❌ **Payments** - Payment processing and receipts  
❌ **Reports** - Analytics and exports  

**These are next steps for development!**

---

## 🐛 Troubleshooting

### "dotnet is not recognized"
➜ **Install .NET SDK**, then restart terminal

### Build fails
```powershell
dotnet clean
dotnet restore
dotnet build
```

### Application crashes on startup
➜ **Check logs:** `src\FitMan.WPF\bin\Debug\net8.0-windows\logs\`

### Database locked error
➜ **Close app completely**, delete `.db-shm` and `.db-wal` files

---

## 📖 Need More Info?

- **Full Setup Guide:** `BUILD_AND_RUN.md`
- **Implementation Status:** `IMPLEMENTATION_STATUS.md`
- **Project Overview:** `README.md`
- **.NET Installation:** `INSTALL_DOTNET.md`

---

## ✅ Checklist

- [ ] .NET 8.0 SDK installed
- [ ] Project built successfully
- [ ] Application runs
- [ ] Can login with admin/admin123
- [ ] Main window shows navigation menu

**All checked?** You're ready to develop! 🎉

---

## 🚀 Next Steps for Development

1. **Start with Dashboard** - Most visible, motivating
2. **Then Member Management** - Core functionality
3. **Add Check-In** - Daily operations
4. **Implement Payments** - Business critical

**Estimated Time:** 4-6 weeks for full MVP

---

## 💡 Pro Tips

1. **Use Visual Studio** - Better debugging, IntelliSense
2. **Explore the database** - Understand the schema
3. **Read the code** - Well-commented and organized
4. **Start small** - Implement one feature at a time
5. **Test frequently** - Run after each change

---

## 🎯 Success Criteria

You'll know the foundation is working when:
✅ App starts without errors  
✅ Login succeeds  
✅ Main window loads  
✅ Navigation works  
✅ Database file exists  

**All of these should work right now!** ✨

---

## 📞 Questions?

Check the documentation files for answers to common questions about:
- Architecture decisions
- Database design
- Migration to SQL Server
- Adding new features
- Deployment options

---

**Happy Coding!** 💻🏋️‍♂️

Remember: The hard part (architecture, database, authentication) is DONE!  
Now it's just implementing the business features. You've got this! 💪
