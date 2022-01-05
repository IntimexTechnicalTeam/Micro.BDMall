using Intimex.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Web.Mvc
{
    /// <summary>
    /// 资源转换过滤器
    /// </summary>
    public class LanguageResourceAttribute : Attribute, IResultFilter
    {
        public string Lang { get; set; }

        public void OnResultExecuted(ResultExecutedContext context)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void OnResultExecuting(ResultExecutingContext context)
        {
            string langCode = ((ViewResult)context.Result).ViewData["Lang"]?.ToString() ?? "C";

            string cultureName = CultureHelper.GetSupportCulture(langCode);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }
    }
}
