# 🎯 Next Steps: Implementing Features

## Quick Reference for Continuing Development

This guide shows you exactly how to implement the remaining features in the FitMan application.

---

## 🚀 Where to Start

**Recommended Order:**
1. ✅ Dashboard (Most visible, motivating)
2. ✅ Member Management (Core functionality)
3. ✅ Check-In System (Daily operations)
4. ✅ Payment Processing (Business critical)

---

## 1️⃣ Implementing the Dashboard

### Goal
Display real-time gym metrics and KPIs.

### Step-by-Step

#### A. Create Dashboard Service (Application Layer)

**File:** `src\FitMan.Application\Services\IDashboardService.cs`
```csharp
namespace FitMan.Application.Services;

public interface IDashboardService
{
    Task<DashboardStats> GetStatsAsync();
    Task<List<AttendanceTrend>> GetAttendanceTrendAsync(int days = 7);
    Task<List<Member>> GetExpiringMembershipsAsync(int daysThreshold = 7);
}

public class DashboardStats
{
    public int TotalMembers { get; set; }
    public int ActiveMembers { get; set; }
    public int TodayCheckIns { get; set; }
    public decimal MonthlyRevenue { get; set; }
    public int ExpiringMemberships { get; set; }
}

public class AttendanceTrend
{
    public DateTime Date { get; set; }
    public int Count { get; set; }
}
```

#### B. Implement Service (Infrastructure Layer)

**File:** `src\FitMan.Infrastructure\Services\DashboardService.cs`
```csharp
public class DashboardService : IDashboardService
{
    private readonly FitManDbContext _context;
    
    public DashboardService(FitManDbContext context)
    {
        _context = context;
    }
    
    public async Task<DashboardStats> GetStatsAsync()
    {
        var now = DateTime.UtcNow;
        var startOfMonth = new DateTime(now.Year, now.Month, 1);
        var today = now.Date;
        
        return new DashboardStats
        {
            TotalMembers = await _context.Members.CountAsync(),
            ActiveMembers = await _context.Members
                .CountAsync(m => m.Status == MemberStatus.Active),
            TodayCheckIns = await _context.AttendanceLogs
                .CountAsync(a => a.CheckInTime.Date == today),
            MonthlyRevenue = await _context.Payments
                .Where(p => p.PaymentDate >= startOfMonth && p.Status == PaymentStatus.Paid)
                .SumAsync(p => p.TotalAmount),
            ExpiringMemberships = await _context.MemberMemberships
                .CountAsync(m => m.EndDate <= now.AddDays(7) && m.EndDate >= now)
        };
    }
    
    // Implement other methods...
}
```

#### C. Update DashboardViewModel

**File:** `src\FitMan.WPF\ViewModels\DashboardViewModel.cs`
```csharp
public partial class DashboardViewModel : ViewModelBase
{
    private readonly IDashboardService _dashboardService;
    private DashboardStats _stats;
    
    public DashboardStats Stats
    {
        get => _stats;
        set => SetProperty(ref _stats, value);
    }
    
    public DashboardViewModel(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
        Title = "Dashboard";
        _ = LoadDataAsync();
    }
    
    private async Task LoadDataAsync()
    {
        IsBusy = true;
        try
        {
            Stats = await _dashboardService.GetStatsAsync();
        }
        finally
        {
            IsBusy = false;
        }
    }
    
    [RelayCommand]
    private async Task RefreshAsync()
    {
        await LoadDataAsync();
    }
}
```

#### D. Create Dashboard View

**File:** `src\FitMan.WPF\Views\DashboardView.xaml`
```xml
<UserControl x:Class="FitMan.WPF.Views.DashboardView"
             xmlns:materialDesign="http://materialdesigninxaml.net/winfx/xaml/themes">
    <ScrollViewer>
        <StackPanel Margin="20">
            <!-- Header -->
            <TextBlock Text="Dashboard" FontSize="32" FontWeight="Bold" Margin="0,0,0,20"/>
            
            <!-- Stats Cards -->
            <UniformGrid Columns="4" Rows="1">
                <!-- Total Members Card -->
                <materialDesign:Card Margin="0,0,10,0" Padding="20">
                    <StackPanel>
                        <materialDesign:PackIcon Kind="AccountGroup" Width="40" Height="40"/>
                        <TextBlock Text="{Binding Stats.TotalMembers}" FontSize="32" FontWeight="Bold"/>
                        <TextBlock Text="Total Members" FontSize="14" Opacity="0.7"/>
                    </StackPanel>
                </materialDesign:Card>
                
                <!-- Active Members Card -->
                <materialDesign:Card Margin="0,0,10,0" Padding="20">
                    <!-- Similar structure -->
                </materialDesign:Card>
                
                <!-- Today Check-Ins Card -->
                <!-- Monthly Revenue Card -->
            </UniformGrid>
            
            <!-- Charts Section -->
            <!-- Expiring Memberships List -->
        </StackPanel>
    </ScrollViewer>
</UserControl>
```

#### E. Register Services

**File:** `src\FitMan.WPF\App.xaml.cs`
```csharp
// In ConfigureServices method, add:
services.AddScoped<IDashboardService, DashboardService>();
```

---

## 2️⃣ Implementing Member Management

### A. Create Member Service

**File:** `src\FitMan.Application\Services\IMemberService.cs`
```csharp
public interface IMemberService
{
    Task<List<Member>> GetAllMembersAsync();
    Task<Member?> GetMemberByIdAsync(int id);
    Task<List<Member>> SearchMembersAsync(string searchTerm);
    Task<Member> AddMemberAsync(Member member);
    Task UpdateMemberAsync(Member member);
    Task DeleteMemberAsync(int id);
    Task<string> GenerateMembershipNumberAsync();
}
```

### B. Implement Service

**File:** `src\FitMan.Infrastructure\Services\MemberService.cs`
```csharp
public class MemberService : IMemberService
{
    private readonly IRepository<Member> _memberRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public MemberService(IRepository<Member> memberRepository, IUnitOfWork unitOfWork)
    {
        _memberRepository = memberRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<List<Member>> GetAllMembersAsync()
    {
        return (await _memberRepository.GetAllAsync()).ToList();
    }
    
    public async Task<List<Member>> SearchMembersAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return await GetAllMembersAsync();
            
        searchTerm = searchTerm.ToLower();
        return (await _memberRepository.FindAsync(m =>
            m.FirstName.ToLower().Contains(searchTerm) ||
            m.LastName.ToLower().Contains(searchTerm) ||
            m.Phone.Contains(searchTerm) ||
            m.Email.ToLower().Contains(searchTerm) ||
            m.MembershipNumber.ToLower().Contains(searchTerm)
        )).ToList();
    }
    
    public async Task<string> GenerateMembershipNumberAsync()
    {
        var lastMember = (await _memberRepository.GetAllAsync())
            .OrderByDescending(m => m.Id)
            .FirstOrDefault();
            
        int nextNumber = 1;
        if (lastMember != null)
        {
            var numberPart = lastMember.MembershipNumber.Replace("GYM", "");
            if (int.TryParse(numberPart, out int lastNumber))
                nextNumber = lastNumber + 1;
        }
        
        return $"GYM{nextNumber:D3}"; // GYM001, GYM002, etc.
    }
    
    // Implement other methods...
}
```

### C. Update MemberListViewModel

**File:** `src\FitMan.WPF\ViewModels\MemberListViewModel.cs`
```csharp
public partial class MemberListViewModel : ViewModelBase
{
    private readonly IMemberService _memberService;
    private ObservableCollection<Member> _members;
    private string _searchText;
    
    public ObservableCollection<Member> Members
    {
        get => _members;
        set => SetProperty(ref _members, value);
    }
    
    public string SearchText
    {
        get => _searchText;
        set
        {
            SetProperty(ref _searchText, value);
            _ = SearchAsync();
        }
    }
    
    public MemberListViewModel(IMemberService memberService)
    {
        _memberService = memberService;
        _ = LoadMembersAsync();
    }
    
    private async Task LoadMembersAsync()
    {
        IsBusy = true;
        try
        {
            var members = await _memberService.GetAllMembersAsync();
            Members = new ObservableCollection<Member>(members);
        }
        finally
        {
            IsBusy = false;
        }
    }
    
    private async Task SearchAsync()
    {
        var results = await _memberService.SearchMembersAsync(SearchText);
        Members = new ObservableCollection<Member>(results);
    }
    
    [RelayCommand]
    private void AddMember()
    {
        // Open add member dialog
    }
    
    [RelayCommand]
    private void EditMember(Member member)
    {
        // Open edit member dialog
    }
}
```

---

## 3️⃣ Implementing Check-In System

### A. Create Attendance Service

**File:** `src\FitMan.Application\Services\IAttendanceService.cs`
```csharp
public interface IAttendanceService
{
    Task<AttendanceLog> CheckInAsync(int memberId, int staffId, EntryMethod method = EntryMethod.Manual);
    Task<AttendanceLog?> CheckOutAsync(int attendanceId);
    Task<List<AttendanceLog>> GetTodayAttendanceAsync();
    Task<bool> ValidateMembershipAsync(int memberId);
    Task<Member?> FindMemberAsync(string searchTerm);
}
```

### B. Implement Service

**File:** `src\FitMan.Infrastructure\Services\AttendanceService.cs`
```csharp
public class AttendanceService : IAttendanceService
{
    private readonly FitManDbContext _context;
    private readonly IRepository<AttendanceLog> _attendanceRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public async Task<AttendanceLog> CheckInAsync(int memberId, int staffId, EntryMethod method)
    {
        // Validate membership first
        var isValid = await ValidateMembershipAsync(memberId);
        if (!isValid)
            throw new InvalidOperationException("Membership is not active or has expired");
        
        var attendance = new AttendanceLog
        {
            MemberId = memberId,
            CheckInTime = DateTime.UtcNow,
            EntryMethod = method,
            ProcessedByStaffId = staffId,
            FacilityArea = "Main Gym"
        };
        
        await _attendanceRepository.AddAsync(attendance);
        await _unitOfWork.SaveChangesAsync();
        
        return attendance;
    }
    
    public async Task<bool> ValidateMembershipAsync(int memberId)
    {
        var member = await _context.Members
            .Include(m => m.Memberships)
            .FirstOrDefaultAsync(m => m.Id == memberId);
            
        if (member == null || member.Status != MemberStatus.Active)
            return false;
            
        var activeMembership = member.Memberships
            .Where(m => m.Status == MembershipStatus.Active && m.EndDate >= DateTime.UtcNow)
            .OrderByDescending(m => m.EndDate)
            .FirstOrDefault();
            
        return activeMembership != null;
    }
    
    // Implement other methods...
}
```

---

## 4️⃣ Implementing Payment Processing

### A. Create Payment Service

**File:** `src\FitMan.Application\Services\IPaymentService.cs`
```csharp
public interface IPaymentService
{
    Task<Payment> ProcessPaymentAsync(Payment payment);
    Task<string> GenerateReceiptNumberAsync();
    Task<List<Payment>> GetMemberPaymentsAsync(int memberId);
    Task<List<Payment>> GetOverduePaymentsAsync();
    Task<byte[]> GenerateReceiptPdfAsync(int paymentId);
}
```

### B. Implement PDF Receipt Generation

**File:** `src\FitMan.Infrastructure\Services\ReceiptGenerator.cs`
```csharp
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

public class ReceiptGenerator
{
    public byte[] GenerateReceipt(Payment payment, Member member)
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(50);
                
                page.Header().Element(ComposeHeader);
                page.Content().Element(content => ComposeContent(content, payment, member));
                page.Footer().AlignCenter().Text(text =>
                {
                    text.Span("Page ");
                    text.CurrentPageNumber();
                });
            });
        });
        
        return document.GeneratePdf();
    }
    
    private void ComposeHeader(IContainer container)
    {
        container.Row(row =>
        {
            row.RelativeItem().Column(column =>
            {
                column.Item().Text("FitMan Fitness Center").FontSize(20).Bold();
                column.Item().Text("Payment Receipt").FontSize(14);
            });
        });
    }
    
    private void ComposeContent(IContainer container, Payment payment, Member member)
    {
        container.Column(column =>
        {
            column.Spacing(10);
            
            column.Item().Text($"Receipt #: {payment.ReceiptNumber}").FontSize(12);
            column.Item().Text($"Date: {payment.PaymentDate:yyyy-MM-dd}").FontSize(12);
            column.Item().Text($"Member: {member.FullName}").FontSize(12);
            column.Item().Text($"Amount: ${payment.TotalAmount:F2}").FontSize(12).Bold();
            column.Item().Text($"Payment Method: {payment.PaymentMethod}").FontSize(12);
        });
    }
}
```

---

## 🛠️ Common Patterns

### Pattern 1: Adding a New Entity

1. Create entity in `FitMan.Domain/Entities/`
2. Add DbSet to `FitManDbContext`
3. Configure relationships in `OnModelCreating`
4. Create migration: `dotnet ef migrations add AddEntityName`
5. Update database: `dotnet ef database update`

### Pattern 2: Adding a New Service

1. Create interface in `FitMan.Application/Services/`
2. Implement in `FitMan.Infrastructure/Services/`
3. Register in `App.xaml.cs` DI container
4. Inject into ViewModel constructor

### Pattern 3: Adding a New View

1. Create ViewModel in `FitMan.WPF/ViewModels/`
2. Create View (UserControl or Window) in `FitMan.WPF/Views/`
3. Set DataContext in View constructor
4. Register ViewModel in DI if needed

---

## 📊 Using LiveCharts for Graphs

**Install Package (already added):**
```xml
<PackageReference Include="LiveChartsCore.SkiaSharpView.WPF" Version="2.0.0-rc2" />
```

**Example: Attendance Trend Chart**
```xml
<lvc:CartesianChart Series="{Binding AttendanceSeries}" Height="300">
    <lvc:CartesianChart.XAxes>
        <lvc:Axis Labels="{Binding DateLabels}"/>
    </lvc:CartesianChart.XAxes>
</lvc:CartesianChart>
```

```csharp
// In ViewModel
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;

public ISeries[] AttendanceSeries { get; set; }
public string[] DateLabels { get; set; }

private void LoadChartData()
{
    AttendanceSeries = new ISeries[]
    {
        new LineSeries<int>
        {
            Values = new[] { 10, 15, 12, 18, 20, 16, 14 },
            Name = "Check-Ins"
        }
    };
    
    DateLabels = new[] { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };
}
```

---

## 🎨 Material Design Components

### Cards
```xml
<materialDesign:Card Padding="20" Margin="10">
    <StackPanel>
        <TextBlock Text="Title" FontSize="18" FontWeight="Bold"/>
        <TextBlock Text="Content"/>
    </StackPanel>
</materialDesign:Card>
```

### Buttons
```xml
<Button Content="Save" 
        Command="{Binding SaveCommand}"
        Style="{StaticResource MaterialDesignRaisedButton}"/>
```

### DataGrid
```xml
<DataGrid ItemsSource="{Binding Members}"
          AutoGenerateColumns="False"
          CanUserAddRows="False">
    <DataGrid.Columns>
        <DataGridTextColumn Header="Name" Binding="{Binding FullName}"/>
        <DataGridTextColumn Header="Phone" Binding="{Binding Phone}"/>
    </DataGrid.Columns>
</DataGrid>
```

---

## 🧪 Testing Your Code

### Unit Test Example

**File:** `tests\FitMan.UnitTests\Services\MemberServiceTests.cs`
```csharp
using Xunit;
using Moq;

public class MemberServiceTests
{
    [Fact]
    public async Task GenerateMembershipNumber_ShouldReturnGYM001_WhenNoMembers()
    {
        // Arrange
        var mockRepo = new Mock<IRepository<Member>>();
        mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Member>());
        
        var service = new MemberService(mockRepo.Object, null);
        
        // Act
        var result = await service.GenerateMembershipNumberAsync();
        
        // Assert
        Assert.Equal("GYM001", result);
    }
}
```

---

## 🚀 Deployment Checklist

### Before Release:
- [ ] Change default passwords
- [ ] Test all features thoroughly
- [ ] Add error handling for edge cases
- [ ] Configure logging appropriately
- [ ] Create user manual
- [ ] Test on target machines
- [ ] Create backup/restore procedures
- [ ] Set up database backup schedule

### Publishing:
```powershell
cd src\FitMan.WPF
dotnet publish -c Release -r win-x64 --self-contained -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true
```

---

## 📞 Getting Help

### When Stuck:
1. **Check existing code** - Similar patterns are already implemented
2. **Read documentation** - BUILD_AND_RUN.md has details
3. **Use IntelliSense** - VS/VS Code shows available methods
4. **Debug step-by-step** - Use breakpoints
5. **Check logs** - Serilog writes to logs folder

### Resources:
- EF Core Docs: https://docs.microsoft.com/en-us/ef/core/
- WPF Tutorial: https://docs.microsoft.com/en-us/dotnet/desktop/wpf/
- Material Design: https://materialdesigninxaml.net/
- Stack Overflow: Search for specific errors

---

## 🎯 Quick Tips

1. **Start Small** - Implement one feature at a time
2. **Test Often** - Run the app after each change
3. **Commit Frequently** - Save your progress with Git
4. **Follow Patterns** - Use existing code as templates
5. **Ask Questions** - Better to clarify than guess

---

**You've got everything you need to succeed!** 🚀

The foundation is solid, the patterns are clear, and the path forward is well-documented. Happy coding! 💻
