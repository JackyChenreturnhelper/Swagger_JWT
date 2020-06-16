using Autofac;
using Autofac.Extras.DynamicProxy;
using AutoMapper;
using Swagger_JWT.Common.AOP;
using Swagger_JWT.Common.Helper;
using Swagger_JWT.Common.LoginUser;
using Swagger_JWT.Repository.Implement;
using Swagger_JWT.Repository.Interface;
using Swagger_JWT.Service.Implement;
using Swagger_JWT.Service.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Swagger_JWT.Infrastructure.Dependency
{
    public class AutofacModuleRegister : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var basePath = AppContext.BaseDirectory;
         

           

            var servicesDllFile = Path.Combine(basePath, "Swagger_JWT.Service.dll");
            var repositoryDllFile = Path.Combine(basePath, "Swagger_JWT.Repository.dll");

            var cacheType = new List<Type>();
            if (Appsettings.app(new string[] { "AppSettings", "RedisCachingAOP", "Enabled" }).ObjToBool())
            {
                builder.RegisterType<RedisCacheAOP>();
                cacheType.Add(typeof(RedisCacheAOP));
            }
            else
            {
                builder.RegisterType<CacheAOP>();
                cacheType.Add(typeof(CacheAOP));
            }
          
            // 獲取 Service.dll 程序集服務，並注冊
            var assemblysServices = Assembly.LoadFrom(servicesDllFile);
            builder.RegisterAssemblyTypes(assemblysServices)
                      .AsImplementedInterfaces()
                      .InstancePerDependency()
                      .EnableInterfaceInterceptors()//引用Autofac.Extras.DynamicProxy;
                      .InterceptedBy(cacheType.ToArray());//允許將攔截器服務的列表分配給注冊。
             // 獲取 Repository.dll 程序集服務，並注冊
            var assemblysRepository = Assembly.LoadFrom(repositoryDllFile);
            builder.RegisterAssemblyTypes(assemblysRepository)
                   .AsImplementedInterfaces()
                   .InstancePerDependency();

            builder.Register(c => new LoginService("jacky",
                c.Resolve<IApiClaimsRepository>(),
                c.Resolve<IMapper>(),
                c.Resolve<ILoginUser>()
                ))
             .As<ILoginService>()
              .AsImplementedInterfaces()
              .EnableInterfaceInterceptors()
             .InterceptedBy(cacheType.ToArray());
        }
    }
}
