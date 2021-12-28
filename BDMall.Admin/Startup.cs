using Autofac;
using BDMall.BLL;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.IO;
using System.Linq;
using Web.AutoFac;
using Web.Framework;
using Web.Mvc;
using UEditorNetCore;

namespace BDMall.Admin
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
            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                {
                    // 忽略循环引用
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    // 不使用驼峰
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    // 设置时间格式
                    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                    // 如字段为null值，该字段不会返回到前端
                    // options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                })
                .AddRazorRuntimeCompilation();              //可以在编译调试模式下编辑view

            services.AddMvc(options =>
            {
                //options.Filters.Add(typeof(UserAuthorizeAttribute));            //全局鉴权
                options.EnableEndpointRouting = false;
            });

            this.ConfigureApiBehaviorOptions(services);

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                        .AddCookie(opt => { opt.LoginPath = new PathString("/Default/Index"); });

            Web.Framework.AutoMapperConfiguration.InitAutoMapper();
            services.AddSingleton(this.Configuration);
            services.AddScoped(typeof(AdminApiAuthorizeAttribute));         //注入Filter

            WebCache.ServiceCollectionExtensions.AddCacheServices(services, Globals.Configuration);                        //注入redis组件
            BDMall.Repository.ServiceCollectionExtensions.AddServices(services, Globals.Configuration);                      //注入EFCore DataContext
            Web.MQ.ServiceCollectionExtensions.AddServices(services, Globals.Configuration);                                    //注入RabbitMQ  

            Web.Mvc.ServiceCollectionExtensions.AddHttpContextAccessor(services);
            Web.Mvc.ServiceCollectionExtensions.AddServiceProvider(services);
            Web.MediatR.ServiceCollectionExtensions.AddServices(services, typeof(Startup));

            Web.Mvc.ServiceCollectionExtensions.AddFileProviderServices(services, Globals.Configuration);
            services.AddUEditorService("Config/config.json");

            //AddScopedIServiceProvider(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Globals.Services = app.ApplicationServices;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseMiddleware<GlobalErrorHandlingMiddleware>();         //全局异常处理
            //app.UseMiddleware<JwtAuthenticationMiddleware>();

            app.UseHttpsRedirection();

            var staticFiles = new StaticFileOptions
            {
                FileProvider = new CompositeFileProvider(
                    new PhysicalFileProvider(Path.Combine(Configuration["UploadPath"], "ClientResources"))
                    //new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "wwwroot"))                    
                 ),                
                RequestPath ="/ClientResources"      //必须设置，否则上传完后相对路径下访问不了
            };
            app.UseStaticFiles(staticFiles);
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    //pattern: "{controller=Home}/{action=Index}/{id?}");
                    pattern: "{controller=Account}/{action=Login}/{id?}/{para2?}/{para3?}");  //让MVC Controller支持无参或一个参数以上

                endpoints.MapAreaControllerRoute(
                         name: "areas",
                         areaName: "AdminApi",
                         pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}/{para2?}");      //让Api Controller支持无参或一个参数以上

            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule<AutofacRegisterModuleFactory>();
            //builder.RegisterModule<ControllerRegisterModuleFactory>();
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

        void AddScopedIServiceProvider(IServiceCollection services)
        {
            var preHeatList = RuntimeHelper.Discovery().FirstOrDefault(type => type.GetName().Name == "BDMall.BLL")?
                    .GetTypes()?.Where(type => type.Name.StartsWith("PreHeat"))?.ToArray();

            foreach (var item in preHeatList)
            {
                services.AddScoped(item);
            }
            //return services;
        }
    }
}
