using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NLog.Extensions.Logging;

namespace BDMall.Admin
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
                .ConfigureLogging(factory =>
                {
                    factory.AddNLog(new NLogProviderOptions { CaptureMessageTemplates = true, CaptureMessageProperties = true });
                    NLog.LogManager.LoadConfiguration("Config/nlog.config");
                })
          .UseServiceProviderFactory(new AutofacServiceProviderFactory());              //Ìæ»»ÎªAutoFacÈÝÆ÷¹¤³§
    }
}
