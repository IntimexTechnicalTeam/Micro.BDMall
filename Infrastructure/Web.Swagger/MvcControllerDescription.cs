using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;

namespace Web.Swagger
{
    /// <summary>
    /// 解析接口说明 
    /// </summary>
    public class MvcControllerDescription : IDocumentFilter
    {
        /// <summary>
        /// apply
        /// </summary>
        /// <param name="swaggerDoc"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            swaggerDoc.Tags = GetControllerDescList();
        }

        /// <summary>
        /// 从API文档中读取控制器描述
        /// </summary>
        /// <returns>所有控制器描述</returns>
        public List<OpenApiTag> GetControllerDescList()
        {
            Assembly entryAssembly = Assembly.GetEntryAssembly();
            var basePath = Path.GetDirectoryName(entryAssembly.Location);
            var xmlpath = Path.Combine(basePath, $"{entryAssembly.GetName().Name}.xml");
            List<OpenApiTag> list = new List<OpenApiTag>();
            if (File.Exists(xmlpath))
            {
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(xmlpath);
                string type = string.Empty, path = string.Empty, controllerName = string.Empty;

                string[] arrPath;
                int length = -1, cCount = "Controller".Length;
                XmlNode summaryNode = null;
                foreach (XmlNode node in xmldoc.SelectNodes("//member"))
                {
                    type = node.Attributes["name"].Value;
                    if (type.StartsWith("T:"))
                    {
                        OpenApiTag tagInfo = new OpenApiTag();
                        //控制器
                        arrPath = type.Split('.');
                        length = arrPath.Length;
                        controllerName = arrPath[length - 1];
                        if (controllerName.EndsWith("Controller"))
                        {
                            //获取控制器注释
                            summaryNode = node.SelectSingleNode("summary");
                            string key = controllerName.Remove(controllerName.Length - cCount, cCount);
                            tagInfo.Name = key;
                            if (summaryNode != null && !string.IsNullOrEmpty(summaryNode.InnerText))
                            {
                                tagInfo.Description = summaryNode.InnerText.Trim();
                            }
                            list.Add(tagInfo);
                        }
                    }
                }
            }
            return list;
        }
    }
}
