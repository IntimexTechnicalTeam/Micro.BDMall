using Autofac;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using System.IO;
using Web.AutoFac;
using Web.Framework;
using Web.Mvc;

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
                    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";                ///保证后端输出给前端时，字段大小写一致
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                })
                .AddRazorRuntimeCompilation();              //可以在编译调试模式下编辑view

            services.AddMvc(options =>
            {
                //options.Filters.Add(typeof(UserAuthorizeAttribute));            //全局鉴权
                options.EnableEndpointRouting = false;
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                        .AddCookie(opt => { opt.LoginPath = new PathString("/Default/Index"); });

            Web.Framework.AutoMapperConfiguration.InitAutoMapper();
            services.AddSingleton(this.Configuration);
            //services.AddScoped(typeof(UserAuthorizeAttribute));             //注入Filter
            services.AddScoped(typeof(AdminApiAuthorizeAttribute));

            //services.AddControllers(options =>
            //{
            //    options.EnableEndpointRouting = false;
            //    options.Filters.Add(typeof(UserAuthorizeAttribute));
            //    //options.Filters.Add(typeof(AdminAuthorizeAttribute));
            //});

            WebCache.ServiceCollectionExtensions.AddCacheServices(services, Globals.Configuration);                        //注入redis组件
            BDMall.Repository.ServiceCollectionExtensions.AddServices(services, Globals.Configuration);                      //注入EFCore DataContext
            Web.MQ.ServiceCollectionExtensions.AddServices(services, Globals.Configuration);                                    //注入RabbitMQ  

            Web.Mvc.ServiceCollectionExtensions.AddHttpContextAccessor(services);
            Web.Mvc.ServiceCollectionExtensions.AddServiceProvider(services);
            Web.MediatR.ServiceCollectionExtensions.AddServices(services, typeof(Startup));

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
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    //pattern: "{controller=Home}/{action=Index}/{id?}");
                    pattern: "{controller=Account}/{action=Login}");


                endpoints.MapAreaControllerRoute(
                         name: "areas",
                         areaName: "AdminApi",
                         pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule<AutofacRegisterModuleFactory>();
            //builder.RegisterModule<ControllerRegisterModuleFactory>();
        }    
    }
}
