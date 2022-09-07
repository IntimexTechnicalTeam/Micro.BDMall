using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Web.Swagger
{
    public static class SwaggerExtension
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services, params string[] xmlFiles)
        {
            return AddSwagger(services, xmlFiles, new string[] { "v1" });
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services, string[] xmlFiles, string[] apiVersions)
        {
            Assembly entryAssembly = Assembly.GetEntryAssembly();
            string appName = entryAssembly.GetName().Name;

            services.AddSwaggerGen(c =>
            {
                if (apiVersions != null)
                    foreach (var version in apiVersions)
                    {
                        c.SwaggerDoc(version, new OpenApiInfo
                        {
                            Version = version,
                            Title = appName,
                            Description = appName,
                            //TermsOfService = "None",
                        });
                    }

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Scheme = "bearer",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT"
                });

                OpenApiSecurityRequirement requirement = new OpenApiSecurityRequirement();
                requirement.Add(new OpenApiSecurityScheme() { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } }, new List<string>());
                c.AddSecurityRequirement(requirement);

                var basePath = Path.GetDirectoryName(entryAssembly.Location);
                if (xmlFiles != null)
                {
                    foreach (var xmlFile in xmlFiles)
                    {
                        var xmlPath = Path.Combine(basePath, xmlFile);
                        if (File.Exists(xmlPath))
                            c.IncludeXmlComments(xmlPath);
                    }
                }

                c.DocumentFilter<MvcControllerDescription>();//接口说明

                c.SchemaFilter<SchemaFilter>();   //保证可以自定义实体参数的默认值
            });

            return services;
        }

        //public static IServiceCollection AddSwagger(this IServiceCollection services,string basePath)
        //{
        //    services.AddSwaggerGen(c =>
        //    {                
        //        if (File.Exists(basePath))
        //            c.IncludeXmlComments(basePath);
        //    });
        //    return services;
        //}

        /// <summary>
        /// 注册Swagger
        /// </summary>
        /// <param name="app"></param>
        /// <param name="version"></param>
        public static void ConfigureSwagger(this IApplicationBuilder app, string version = "v1")
        {
            Assembly entryAssembly = Assembly.GetEntryAssembly();
            string appName = entryAssembly.GetName().Name;

            app.UseSwagger();
            app.UseSwaggerUI(c =>
           {
               c.SwaggerEndpoint($"/swagger/{version}/swagger.json", appName);
           });
        }

    }
}
