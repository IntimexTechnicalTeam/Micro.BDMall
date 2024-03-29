﻿

await RegisterHelper.RunConfig((services, config) =>
{
    //依赖注入

    Web.RegisterConfig.ServiceCollectionExtensions.AddServices(services, Globals.Configuration);
    WebCache.ServiceCollectionExtensions.AddCacheServices(services, Globals.Configuration);
    BDMall.Repository.ServiceCollectionExtensions.AddServices(services, Globals.Configuration);
    Web.MQ.ServiceCollectionExtensions.AddServices(services, Globals.Configuration);                                      //注入RabbitMQ  
    Web.Framework.AutoMapperConfiguration.InitAutoMapper();
    return services;
}, args);

