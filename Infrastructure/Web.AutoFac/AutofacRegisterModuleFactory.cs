using Autofac;
using Autofac.Extras.DynamicProxy;
using BDMall.BLL;
using BDMall.Repository;
using MediatR;
using System.Linq;
using System.Reflection;
using Web.Framework;

namespace Web.AutoFac
{
    /// <summary>
    /// autofac注入工厂类
    /// </summary>
    public class AutofacRegisterModuleFactory : Autofac.Module
    {
        //重写Autofac管道Load方法，在这里注册注入
        protected override void Load(ContainerBuilder builder)
        {
            //注入上下文
            builder.RegisterType(typeof(MallDbContext)).As(typeof(MallDbContext)).InstancePerLifetimeScope().AsImplementedInterfaces();

            var assemblys = RuntimeHelper.Discovery();

            //////只要继承IDependency就自动注册
            builder.RegisterAssemblyTypes(assemblys)
                      .Where(t => !t.GetTypeInfo().IsAbstract && typeof(IDependency).IsAssignableFrom(t))
                      .EnableInterfaceInterceptors()//AOP拦截
                       .AsImplementedInterfaces()            //构造函数注入                     
                      .PropertiesAutowired()                    //属性注入
                      .InstancePerLifetimeScope();           ////自动注册    

            //注入SqlSugarDbContext,泛型注入
            //builder.RegisterGeneric(typeof(SugarDbContext<>)).As(typeof(ISugarDbContext<>)).InstancePerLifetimeScope().AsImplementedInterfaces();

            //注入MediatR中介者服务类           
            var mediatrList = RuntimeHelper.Discovery().FirstOrDefault(type => type.GetName().Name == "BDMall.BLL")?
                    .GetTypes()?.Where(type => type.IsDefined(typeof(DependencyAttribute), true))?.ToArray();
            builder.RegisterTypes(mediatrList)
                      .AsClosedTypesOf(typeof(INotificationHandler<>)).AsImplementedInterfaces()
                      .PropertiesAutowired().InstancePerLifetimeScope();

            var preHeatList = RuntimeHelper.Discovery().FirstOrDefault(type => type.GetName().Name == "BDMall.BLL")?
                    .GetTypes()?.Where(type => type.Name.StartsWith("PreHeat"))?.ToArray();

            foreach (var item in preHeatList)
            {
                //builder.RegisterType(item).Named<IBasePreHeatService>(item.Name).EnableInterfaceInterceptors().AsImplementedInterfaces().InstancePerLifetimeScope();
                builder.RegisterType(item).InstancePerLifetimeScope();
            }
            builder.RegisterType(typeof(IdGenerator)).As(typeof(IdGenerator)).SingleInstance();   //注入雪花算法的的唯一序列号
        }
    }
}
