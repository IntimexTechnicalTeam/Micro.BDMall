namespace BDMall.Admin
{
    public  class Startup
    {
        // ��IServiceCollection������ע��ȫ������
       public static void ConfigureServices(WebApplicationBuilder builder)
       {
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                       .AddCookie(opt => { opt.LoginPath = new PathString("/Default/Index"); });

            builder.Services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                {
                    // ����ѭ������
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    // ��ʹ���շ�
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    // ����ʱ���ʽ
                    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                    // ���ֶ�Ϊnullֵ�����ֶβ��᷵�ص�ǰ��
                    // options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                })
                .AddRazorRuntimeCompilation();              //�����ڱ������ģʽ�±༭view
            builder.Services.AddScoped(typeof(AdminApiAuthorizeAttribute));         //ע��Filter

            builder.Services.AddMvc(options =>
            {
                //options.Filters.Add(typeof(UserAuthorizeAttribute));            //ȫ�ּ�Ȩ
                options.EnableEndpointRouting = false;
            });

            ConfigureApiBehaviorOptions(builder.Services);

            Web.Framework.AutoMapperConfiguration.InitAutoMapper();
            builder.Services.AddSingleton(builder.Configuration);

            WebCache.ServiceCollectionExtensions.AddCacheServices(builder.Services, builder.Configuration);                        //ע��redis���
            BDMall.Repository.ServiceCollectionExtensions.AddServices(builder.Services, builder.Configuration);                      //ע��EFCore DataContext
            Web.MQ.ServiceCollectionExtensions.AddServices(builder.Services, builder.Configuration);                                    //ע��RabbitMQ  

            Web.Mvc.ServiceCollectionExtensions.AddHttpContextAccessor(builder.Services);
            Web.Mvc.ServiceCollectionExtensions.AddServiceProvider(builder.Services);
            Web.MediatR.ServiceCollectionExtensions.AddServices(builder.Services, typeof(Program));
            Web.Mvc.ServiceCollectionExtensions.AddFileProviderServices(builder.Services, builder.Configuration);

            builder.Services.AddUEditorService("Config/config.json");
            //AddScopedIServiceProvider(services);
        }

        // ���� HTTP request pipeline 
       public static void ConfigurePipeLine(IApplicationBuilder app, WebApplicationBuilder builder)
        {
            Globals.Services = app.ApplicationServices;
            Globals.Configuration = builder.Configuration;

            if (builder.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseMiddleware<GlobalErrorHandlingMiddleware>();         //ȫ���쳣����
                                                                        //app.UseMiddleware<JwtAuthenticationMiddleware>();

            app.UseHttpsRedirection();      //����HTTPS�ض���

            #region �����ļ�����·��
            var staticFiles = new StaticFileOptions
            {
                FileProvider = new CompositeFileProvider(
                    new PhysicalFileProvider(Path.Combine(builder.Configuration["UploadPath"], "ClientResources"))
                 //new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "wwwroot"))                    
                 ),
                RequestPath = "/ClientResources"      //�������ã������ϴ�������·���·��ʲ���
            };
            app.UseStaticFiles(staticFiles);
            app.UseStaticFiles();
            #endregion

            app.UseRouting();
            app.UseAuthorization();         //���������UseRouting �� UseEndpoints ֮��
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    //pattern: "{controller=Home}/{action=Index}/{id?}");
                    pattern: "{controller=Account}/{action=Login}/{id?}/{para2?}/{para3?}");  //��MVC Controller֧���޲λ�һ����������

                endpoints.MapAreaControllerRoute(
                         name: "areas",
                         areaName: "AdminApi",
                         pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}/{para2?}");      //��Api Controller֧���޲λ�һ����������

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
