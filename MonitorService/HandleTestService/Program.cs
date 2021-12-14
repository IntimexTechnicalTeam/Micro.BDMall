using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.AutoFac;
using Web.Framework;
using Web.RegisterConfig;

/// <summary>
///  
/// </summary>
namespace HandleTestService
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await RegisterHelper.RunConfig((services, config) =>
            {
                //依赖注入
                Web.RegisterConfig.ServiceCollectionExtensions.AddServices<Program>(services, Globals.Configuration);    //注入配置文件
                WebCache.ServiceCollectionExtensions.AddCacheServices(services, Globals.Configuration);                          //注入redis组件
                BDMall.Repository.ServiceCollectionExtensions.AddServices(services, Globals.Configuration);                      //注入EFCore DataContext
                Web.MQ.ServiceCollectionExtensions.AddServices(services, Globals.Configuration);                                      //注入RabbitMQ  
                return services;
            }, args);

        }
    }
}
