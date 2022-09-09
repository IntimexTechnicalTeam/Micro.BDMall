
namespace BDMall.WebApi
{
    public static class Startup
    {
        // ��IServiceCollection������ע��ȫ������
        public static void ConfigureServices(WebApplicationBuilder builder)
        {
            AddControllers(builder.Services);
            ConfigureApiBehaviorOptions(builder.Services);
            
            //׷��API ��������
            SwaggerExtension.AddSwagger(builder.Services, "BDMall.WebApi.xml", "BDMall.Domain.xml");

            builder.Services.AddMvc(options =>
            {
                //options.Filters.Add(typeof(UserAuthorizeAttribute));            //ȫ�ּ�Ȩ
                options.EnableEndpointRouting = false;
            });
            builder.Services.TryAddEnumerable(ServiceDescriptor.Transient<IApplicationModelProvider, ProduceResponseTypeModelProvider>());

            Web.Framework.AutoMapperConfiguration.InitAutoMapper();
            builder.Services.AddSingleton(builder.Configuration);
                     
            WebCache.ServiceCollectionExtensions.AddCacheServices(builder.Services, builder.Configuration);                        //ע��redis���
            BDMall.Repository.ServiceCollectionExtensions.AddServices(builder.Services, builder.Configuration);                      //ע��EFCore DataContext
            Web.MQ.ServiceCollectionExtensions.AddServices(builder.Services, builder.Configuration);                                    //ע��RabbitMQ  
            Web.Mvc.ServiceCollectionExtensions.AddHttpContextAccessor(builder.Services);
            Web.Mvc.ServiceCollectionExtensions.AddServiceProvider(builder.Services);
            Web.MediatR.ServiceCollectionExtensions.AddServices(builder.Services, typeof(Program));

            //ע��֧����������
            //Web.AliPay.ServiceCollectionExtensions.AddServices(builder.Services, builder.Configuration);
        }

        // ���� HTTP request pipeline 
        public static void ConfigurePipeLine(IApplicationBuilder app, WebApplicationBuilder builder)
        {
            Globals.Services = app.ApplicationServices;
            Globals.Configuration = builder.Configuration;

            //����ǿ�����ģʽ
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
            app.UseMiddleware<GlobalErrorHandlingMiddleware>();         //ȫ���쳣����
            //app.UseMiddleware<JwtAuthenticationMiddleware>();

            app.UseHttpsRedirection();           ////HTTPS�ض���
            app.UseRouting();
            app.UseAuthorization();              //���������UseRouting �� UseEndpoints ֮��
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
            .AddControllersAsServices();   //�ڿ������н�������ע��
        }

        /// <summary>
        /// ģ����֤
        /// </summary>
        /// <param name="services"></param>
        static void ConfigureApiBehaviorOptions(IServiceCollection services)
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
