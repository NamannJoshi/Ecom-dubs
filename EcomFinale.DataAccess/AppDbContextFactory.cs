using EcomFinale.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MyApp.DataAccess;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        optionsBuilder.UseNpgsql(
            "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=3590");

        return new AppDbContext(optionsBuilder.Options);
    }
}