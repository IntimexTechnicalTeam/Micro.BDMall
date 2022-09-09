
namespace BDMall.WebApi
{
    public static class Startup
    {
        // 在IServiceCollection容器中注册全局设置
        public static void ConfigureServices(WebApplicationBuilder builder)
        {
            AddControllers(builder.Services);
            ConfigureApiBehaviorOptions(builder.Services);
            
            //追加API 参数描述
            SwaggerExtension.AddSwagger(builder.Services, "BDMall.WebApi.xml", "BDMall.Domain.xml");

            builder.Services.AddMvc(options =>
            {
                //options.Filters.Add(typeof(UserAuthorizeAttribute));            //全局鉴权
                options.EnableEndpointRouting = false;
            });
            builder.Services.TryAddEnumerable(ServiceDescriptor.Transient<IApplicationModelProvider, ProduceResponseTypeModelProvider>());

            Web.Framework.AutoMapperConfiguration.InitAutoMapper();
            builder.Services.AddSingleton(builder.Configuration);
                     
            WebCache.ServiceCollectionExtensions.AddCacheServices(builder.Services, builder.Configuration);                        //注入redis组件
            BDMall.Repository.ServiceCollectionExtensions.AddServices(builder.Services, builder.Configuration);                      //注入EFCore DataContext
            Web.MQ.ServiceCollectionExtensions.AddServices(builder.Services, builder.Configuration);                                    //注入RabbitMQ  
            Web.Mvc.ServiceCollectionExtensions.AddHttpContextAccessor(builder.Services);
            Web.Mvc.ServiceCollectionExtensions.AddServiceProvider(builder.Services);
            Web.MediatR.ServiceCollectionExtensions.AddServices(builder.Services, typeof(Program));

            //注入支付宝的配置
            //Web.AliPay.ServiceCollectionExtensions.AddServices(builder.Services, builder.Configuration);
        }

        // 设置 HTTP request pipeline 
        public static void ConfigurePipeLine(IApplicationBuilder app, WebApplicationBuilder builder)
        {
            Globals.Services = app.ApplicationServices;
            Globals.Configuration = builder.Configuration;

            //如果是开发者模式
            if (builder.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();            
            }
            else
            {
                //app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.ConfigureSwagger();
            app.UseMiddleware<GlobalErrorHandlingMiddleware>();         //全局异常处理
            //app.UseMiddleware<JwtAuthenticationMiddleware>();

            app.UseHttpsRedirection();           ////HTTPS重定向
            app.UseRouting();
            app.UseAuthorization();              //这个必须在UseRouting 和 UseEndpoints 之间
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        static void AddControllers(IServiceCollection services)
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
        static void ConfigureApiBehaviorOptions(IServiceCollection services)
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
