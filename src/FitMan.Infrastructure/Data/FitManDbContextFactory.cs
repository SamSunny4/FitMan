using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FitMan.Infrastructure.Data;

public class FitManDbContextFactory : IDesignTimeDbContextFactory<FitManDbContext>
{
    public FitManDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<FitManDbContext>();
        
        // Use SQLite for design-time
        optionsBuilder.UseSqlite("Data Source=FitMan.db");

        return new FitManDbContext(optionsBuilder.Options);
    }
}
