using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using System.IO;
using System.Linq;
using System.Reflection;
using Web.AutoFac;
using Web.Framework;
using Web.Mvc;
using Web.Swagger;

namespace BDMall.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            var builder = new ConfigurationBuilder()
                       .AddJsonFile("Config/appsettings.json", optional: true, reloadOnChange: true)
                       .AddEnvironmentVariables();
            this.Configuration = builder.Build();
            Globals.Configuration = this.Configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            this.AddControllers(services);
            this.ConfigureApiBehaviorOptions(services);

            string assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            SwaggerExtension.AddSwagger(services, $"{assemblyName}.xml", "BDMall.WebApi.xml");

            //追加API 参数描述
            var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);
            basePath = Path.Combine(basePath, "BDMall.Domain.xml");
            SwaggerExtension.AddSwagger(services,basePath);

            services.AddMvc(options =>
            {
                //options.Filters.Add(typeof(UserAuthorizeAttribute));            //全局鉴权
                options.EnableEndpointRouting = false;
            });

            Web.Framework.AutoMapperConfiguration.InitAutoMapper();
            services.AddSingleton(this.Configuration);
                     
            WebCache.ServiceCollectionExtensions.AddCacheServices(services, Globals.Configuration);                          //注入redis组件
            BDMall.Repository.ServiceCollectionExtensions.AddServices(services, Globals.Configuration);                      //注入EFCore DataContext
            Web.MQ.ServiceCollectionExtensions.AddServices(services, Globals.Configuration);                                      //注入RabbitMQ  

            Web.Mvc.ServiceCollectionExtensions.AddHttpContextAccessor(services);
            Web.Mvc.ServiceCollectionExtensions.AddServiceProvider(services);
            Web.MediatR.ServiceCollectionExtensions.AddServices(services, typeof(Startup));

            //注入支付宝的配置
            //Web.AliPay.ServiceCollectionExtensions.AddServices(services, this.Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Globals.Services = app.ApplicationServices;

            //如果是开发者模式
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();            
            }

            app.ConfigureSwagger();
            app.UseMiddleware<GlobalErrorHandlingMiddleware>();         //全局异常处理
            //app.UseMiddleware<JwtAuthenticationMiddleware>();

            app.UseHttpsRedirection();  ////HTTPS重定向
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule<AutofacRegisterModuleFactory>();
            //builder.RegisterModule<ControllerRegisterModuleFactory>();
        }

        void AddControllers(IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.EnableEndpointRouting = false;
                options.Filters.Add(typeof(UserAuthorizeAttribute));
            }).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            })           
            .AddControllersAsServices();   //在控制器中进行属性注入
        }

        /// <summary>
        /// 模型验证
        /// </summary>
        /// <param name="services"></param>
        void ConfigureApiBehaviorOptions(IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    //获取验证失败的模型字段 
                    var errors = actionContext.ModelState
                     .Where(e => e.Value.Errors.Count > 0)
                     .Select(e => e.Value.Errors.First().ErrorMessage)
                     .ToList();
                    string str = errors.FirstOrDefault();
                    var result = new SystemResult
                    {
                        Succeeded = false,
                        Message = str,
                    };
                    return new OkObjectResult(result);
                };
            });
        }
    }
}
