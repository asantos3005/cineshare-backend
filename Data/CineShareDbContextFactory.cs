using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace cineshare_backend.Data;

public class CineShareDbContextFactory : IDesignTimeDbContextFactory<CineShareDbContext>
{
    public CineShareDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddUserSecrets<CineShareDbContextFactory>(optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        var optionsBuilder = new DbContextOptionsBuilder<CineShareDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new CineShareDbContext(optionsBuilder.Options);
    }
}
