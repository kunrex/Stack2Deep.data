namespace Stack2Deep;

public static class Program
{
    public static void Main(string[] args) => CreateHostBuilder(args).Build().Run();
    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<StartUp>().ConfigureKestrel(options =>
            {
                options.ListenAnyIP(5000); 
            });
        });
}