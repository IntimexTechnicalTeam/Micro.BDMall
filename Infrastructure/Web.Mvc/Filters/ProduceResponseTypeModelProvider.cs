using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace Web.Mvc.Filters
{
    /// <summary>
    /// 全局通用的ProduceResponseType
    /// </summary>
    public class ProduceResponseTypeModelProvider : IApplicationModelProvider
    {
        public int Order => 0;

        public void OnProvidersExecuted(ApplicationModelProviderContext context)
        {
            
        }

        public void OnProvidersExecuting(ApplicationModelProviderContext context)
        {
            foreach (ControllerModel controller in context.Result.Controllers)
            {
                foreach (ActionModel action in controller.Actions)
                {
                    if (!action.Filters.Any(e => (e is ProducesResponseTypeAttribute producesResponseType) && producesResponseType.StatusCode == StatusCodes.Status200OK))
                    {
                        //跳过带有ApiIgnoreAttribute的action
                        //if (action.Attributes.Any(f => (f is ApiIgnoreAttribute)))
                        //{
                        //    continue;
                        //}
                        if (action.ActionMethod.ReturnType != null)
                        {                          
                            var type = typeof(SystemResult);
                            if (action.ActionMethod.ReturnType.IsGenericType && action.ActionMethod.ReturnType.GenericTypeArguments[0] == type)
                            {
                                action.Filters.Add(new ProducesResponseTypeAttribute(type, StatusCodes.Status200OK));
                            }

                            var type2 = typeof(SystemResult<>);
                            if (action.ActionMethod.ReturnType.IsGenericType && action.ActionMethod.ReturnType.GenericTypeArguments[0].IsGenericType &&
                                 action.ActionMethod.ReturnType.GenericTypeArguments[0].Name == type2.Name )
                            {
                                action.Filters.Add(new ProducesResponseTypeAttribute(type2, StatusCodes.Status200OK));
                            }
                        }
                    }
                }
            }
        }
    }
}
