using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Web.Mvc
{
    public static class RequestExtensions
    {
        /// <summary>
        /// 获取模型的验证属性
        /// </summary>
        /// <param name="modelState"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string GetValidationSummary(this ModelStateDictionary modelState, string separator = "\r\n")
        {
            if (modelState.IsValid) return null;

            var error = new StringBuilder();

            foreach (var item in modelState)
            {
                var state = item.Value;
                var message = state.Errors.FirstOrDefault(p => !string.IsNullOrWhiteSpace(p.ErrorMessage))?.ErrorMessage;
                if (string.IsNullOrWhiteSpace(message))
                {
                    message = state.Errors.FirstOrDefault(o => o.Exception != null)?.Exception.Message;
                }
                if (string.IsNullOrWhiteSpace(message)) continue;

                if (error.Length > 0)
                {
                    error.Append(separator);
                }

                error.Append(message);
            }

            return error.ToString();
        }
    }
}
