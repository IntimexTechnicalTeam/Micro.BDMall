var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("Config/appsettings.json", optional: true, reloadOnChange: true).AddEnvironmentVariables();
builder.Logging.AddNLog(new NLogProviderOptions { CaptureMessageTemplates = true, CaptureMessageProperties = true });
NLog.LogManager.LoadConfiguration("Config/nlog.config");

//第一种注入
//builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()).ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule<AutofacRegisterModuleFactory>());
//第二种注入
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory(autofacBuilder => { autofacBuilder.RegisterModule<AutofacRegisterModuleFactory>(); }));


Startup.ConfigureServices(builder);
var app = builder.Build();
Startup.ConfigurePipeLine(app, builder);
app.Run();