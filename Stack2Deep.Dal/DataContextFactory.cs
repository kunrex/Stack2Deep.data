using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

using Stack2Deep.Dal.Configuration;

namespace Stack2Deep.Dal;

public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
{
    public DataContext CreateDbContext(string[] args)
    {
        var config = DataConfigurationManager.DataConfiguration;
        
        var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
        optionsBuilder.UseNpgsql($"Host={config.Host};Username={config.Username};Password={config.Password};Database={config.Database}");

        return new DataContext(optionsBuilder.Options);
    }
}