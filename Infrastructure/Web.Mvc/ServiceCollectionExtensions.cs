using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System;
using System.IO;
using Web.Framework;

namespace Web.Mvc
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHttpContextAccessor(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            return services;
        }

        /// <summary>
        /// 使用Redis来支持session
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IServiceCollection AddSessionServices(this IServiceCollection services, IConfiguration config)
        {          
            //var cacheConfig = new RedisOptions(config);

            //var configOptions = new ConfigurationOptions()
            //{
            //    AbortOnConnectFail = false,
            //    ConnectRetry = 10,
            //    ConnectTimeout = 5000,
            //    SyncTimeout = 5000,
            //    EndPoints = { { cacheConfig.WriteHost, cacheConfig.Port } },
            //    Password = cacheConfig.Password,
            //    AllowAdmin = true,
            //    KeepAlive = 180
            //};

            ////services.AddDistributedRedisCache

            //services.AddStackExchangeRedisCache(option =>
            //{
            //    option.InstanceName = "session";
            //    option.Configuration = $"{cacheConfig.WriteHost}:{cacheConfig.Port}";
            //    option.ConfigurationOptions = configOptions;

            //}).AddSession(option =>
            //{
            //    option.IdleTimeout = TimeSpan.FromMinutes(60);
            //});

            return services;
        }

        /// <summary>
        /// 注入ServiceProvider,方便在service,dal,controller层使用
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddServiceProvider(this IServiceCollection services)
        {
            Globals.Services = services.BuildServiceProvider();
            return services;
        }

        public static IServiceCollection AddFileProviderServices(this IServiceCollection services, IConfiguration config)
        {
            string path = Path.Combine(config["UploadPath"], "ClientResources");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            services.AddSingleton<IFileProvider>(new PhysicalFileProvider(path));

            return services;
        }

      
    }



}
