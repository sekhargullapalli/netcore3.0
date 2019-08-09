using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CountryService1818
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureLogging(loggerFactory => loggerFactory.AddEventLog(new Microsoft.Extensions.Logging.EventLog.EventLogSettings() {
                     LogName = "CountryService1818Log",
                     SourceName = "CountryService1818"                      
                } ))
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<CountriesContext>();
                    services.AddHostedService<Worker>();
                });
    }
}
