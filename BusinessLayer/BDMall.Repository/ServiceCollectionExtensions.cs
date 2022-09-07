﻿namespace BDMall.Repository
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 注入数据库连接和SQL日志处理
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<MallDbContext>(option =>
                    option.UseSqlServer(config["ConnectionStrings:sqlcon"])
                    .UseLoggerFactory(logger)
                    .EnableSensitiveDataLogging()
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
             );
            return services;
        }

        public static readonly ILoggerFactory logger = LoggerFactory.Create(builder =>
        {
            builder.AddFilter((category, level) => category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information).AddConsole();
        });

    }
}
