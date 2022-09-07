namespace Web.Framework
{
    public class BundleConfig 
    {
        ///// <summary>
        ///// 生成Js
        ///// </summary>
        ///// <param name="jsRootPath"></param>
        ///// <param name="jsFileName"></param>
        ///// <returns></returns>
        //public static IHtmlContent ScriptBundle(string jsRootPath ,params string[] jsFileName)
        //{
        //    string js = string.Empty;            
        //    foreach (var item in jsFileName)
        //    {
        //       //判断文件是否存在，存在才生成js代码
        //       js += $"<script src='{item}'></script>\r\n";
        //    }

        //    return new HtmlString(js);
        //}

        ///// <summary>
        ///// 生成Css代码
        ///// </summary>
        ///// <param name="styleRootPath"></param>
        ///// <param name="styleFileName"></param>
        ///// <returns></returns>
        //public static IHtmlContent StyleBundle(string styleRootPath, params string[] styleFileName)
        //{
        //    string css = string.Empty;
        //    foreach (var item in styleFileName)
        //    {
        //        //判断文件是否存在，存在才生成css代码
        //        css += $"<link rel='stylesheet' href='{item}'>\r\n";
        //    }

        //    return new HtmlString(css);
        //}
    }
}
