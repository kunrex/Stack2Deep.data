using System;

using Microsoft.EntityFrameworkCore;

using Stack2Deep.Dal;

using Stack2Deep.CodeForces;
using Stack2Deep.Configuration;

using Stack2Deep.Services.Interfaces;
using Stack2Deep.Services.Implementations;

namespace Stack2Deep;

public class StartUp
{
    public void ConfigureServices(IServiceCollection services)
    {
        Console.WriteLine("Configuring Services...");
        
        var config = StackConfigurationManager.Configuration;
        
        services.AddDbContext<DataContext>(options =>
        {
            options.UseNpgsql($"Host={config.Host};Username={config.Username};Password={config.Password};Database={config.Database}", x => x.MigrationsAssembly("Stack2Deep.Dal.Migrations"));
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }, ServiceLifetime.Transient);
        
        services.AddScoped<IRegistrationService, RegistrationService>();
        services.AddScoped<ICodeForcesService, CodeForcesService>();
        
        services.AddSingleton<CompetitionManager>();
        
        services.AddControllers();
        
        BuildServices(services);
    }

    private void BuildServices(IServiceCollection services)
    {
        Console.WriteLine("Building Services...");
        services.BuildServiceProvider();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        Console.WriteLine("Configuring...");
        
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers(); 
        });
    }
}