var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("Config/appsettings.json", optional: true, reloadOnChange: true).AddEnvironmentVariables();
builder.Logging.AddNLog(new NLogProviderOptions { CaptureMessageTemplates = true, CaptureMessageProperties = true });
NLog.LogManager.LoadConfiguration("Config/nlog.config");

//��һ��ע��
//builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()).ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule<AutofacRegisterModuleFactory>());
//�ڶ���ע��
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory(autofacBuilder => { autofacBuilder.RegisterModule<AutofacRegisterModuleFactory>(); }));


Startup.ConfigureServices(builder);
var app = builder.Build();
Startup.ConfigurePipeLine(app, builder);
app.Run();