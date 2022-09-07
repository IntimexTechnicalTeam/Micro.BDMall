﻿namespace Web.Framework
{
    /// <summary>
    /// 想要用Globals,必须做config.Build()
    /// </summary>
    public static class Globals
    {
        public static IServiceProvider Services { get; set; }

        public static IConfiguration Configuration { get; set; }
    }
}
