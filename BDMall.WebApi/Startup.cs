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

            //׷��API ��������
            var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);
            basePath = Path.Combine(basePath, "BDMall.Domain.xml");
            SwaggerExtension.AddSwagger(services,basePath);

            services.AddMvc(options =>
            {
                //options.Filters.Add(typeof(UserAuthorizeAttribute));            //ȫ�ּ�Ȩ
                options.EnableEndpointRouting = false;
            });

            Web.Framework.AutoMapperConfiguration.InitAutoMapper();
            services.AddSingleton(this.Configuration);
                     
            WebCache.ServiceCollectionExtensions.AddCacheServices(services, Globals.Configuration);                          //ע��redis���
            BDMall.Repository.ServiceCollectionExtensions.AddServices(services, Globals.Configuration);                      //ע��EFCore DataContext
            Web.MQ.ServiceCollectionExtensions.AddServices(services, Globals.Configuration);                                      //ע��RabbitMQ  

            Web.Mvc.ServiceCollectionExtensions.AddHttpContextAccessor(services);
            Web.Mvc.ServiceCollectionExtensions.AddServiceProvider(services);
            Web.MediatR.ServiceCollectionExtensions.AddServices(services, typeof(Startup));

            //ע��֧����������
            //Web.AliPay.ServiceCollectionExtensions.AddServices(services, this.Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Globals.Services = app.ApplicationServices;

            //����ǿ�����ģʽ
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();            
            }

            app.ConfigureSwagger();
            app.UseMiddleware<GlobalErrorHandlingMiddleware>();         //ȫ���쳣����
            //app.UseMiddleware<JwtAuthenticationMiddleware>();

            app.UseHttpsRedirection();  ////HTTPS�ض���
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
            .AddControllersAsServices();   //�ڿ������н�������ע��
        }

        /// <summary>
        /// ģ����֤
        /// </summary>
        /// <param name="services"></param>
        void ConfigureApiBehaviorOptions(IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    //��ȡ��֤ʧ�ܵ�ģ���ֶ� 
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
