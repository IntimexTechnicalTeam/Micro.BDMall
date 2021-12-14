using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Web.Swagger
{
    /// <summary>
    /// 添加验证登陆头部
    /// </summary>
    public class AuthorizationHeaderParameterOperationFilter : IOperationFilter
    {
        /// <summary>
        /// 验证登陆头部
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            //AddHeaderOperationFilter
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Description = "access token",
                Required = false,
                Schema = new OpenApiSchema
                {
                    Type = "string",
                    Default = new OpenApiString("Bearer {access token}"),
                }
            });

            //var filterPipeline = context.ApiDescription.ActionDescriptor.FilterDescriptors;
            //var isAuthorized = filterPipeline.Select(filterInfo => filterInfo.Filter).Any(filter => filter is IAsyncAuthorizationFilter || filter is IAuthorizationFilter);
            //var allowAnonymous = filterPipeline.Select(filterInfo => filterInfo.Filter).Any(filter => filter is IAllowAnonymousFilter);
            //if (isAuthorized && !allowAnonymous)
            //{
            //    //if (operation.Parameters == null)
            //    //    operation.Parameters = new List<OpenApiParameter>();
            //    operation.Parameters.Add(new OpenApiParameter
            //    {
            //        Name = "Authorization",
            //        In = ParameterLocation.Header,
            //        Description = "access token",
            //        Required = false,
            //        //Type = "string"
            //    });
            //}

        }
    }
}
