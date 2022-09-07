namespace Web.AutoFac
{
    /// <summary>
    /// 属性注入
    /// </summary>
    public  class ControllerRegisterModuleFactory : Autofac.Module
    {
        ////重写Autofac管道Load方法，在这里注册注入
        protected override void Load(ContainerBuilder builder)
        {
            var controllerBaseType = typeof(ControllerBase);            //控制器属性注入
            var assemly = RuntimeHelper.Discovery();
            builder.RegisterAssemblyTypes(assemly).Where(t => controllerBaseType.IsAssignableFrom(t) && t != controllerBaseType).PropertiesAutowired();
 
        }
    }
}
