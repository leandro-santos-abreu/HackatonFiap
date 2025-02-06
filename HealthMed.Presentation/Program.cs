using HealthMed.Presentation;

public class Program
{
    private static string Ambiente { get; set; }
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                Ambiente = context.HostingEnvironment.EnvironmentName;
                config.SetBasePath(Directory.GetCurrentDirectory());
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                config.AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true);
                config.AddEnvironmentVariables();

                if (args != null)
                {
                    config.AddCommandLine(args);
                }

            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                if (Ambiente != "Production")
                {
                    webBuilder.ConfigureKestrel(serverOptions =>
                    {
                        serverOptions.ListenAnyIP(8080);  // HTTP port
                        serverOptions.ListenAnyIP(8443, listenOptions =>  // HTTPS port
                        {
                            var certPath = Environment.GetEnvironmentVariable("ASPNETCORE_Kestrel__Certificates__Default__Path");
                            var certPassword = Environment.GetEnvironmentVariable("ASPNETCORE_Kestrel__Certificates__Default__Password");
                            listenOptions.UseHttps(certPath!, certPassword);
                        });
                    });
                }

                webBuilder.UseStartup<Startup>();
            });
}