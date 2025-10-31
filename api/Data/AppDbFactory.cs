using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace HQV.Data;
public class AppDbFactory : IDesignTimeDbContextFactory<AppDb>
{
    public AppDb CreateDbContext(string[] args)
    {
        var cs =
            Environment.GetEnvironmentVariable("ConnectionStrings__Default")
            ?? "Host=db;Port=5432;Database=app;Username=app;Password=app";
        var opts = new DbContextOptionsBuilder<AppDb>().UseNpgsql(cs).Options;
        return new AppDb(opts);
    }
}