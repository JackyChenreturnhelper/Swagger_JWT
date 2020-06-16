using Microsoft.Extensions.DependencyInjection;
using Swagger_JWT.Common.LoginUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swagger_JWT.Infrastructure.Dependency
{
    public static class PrePareLoginUserSetup
    {
        public static  IServiceCollection AddPrePareLoginUserSetup(this IServiceCollection services)
        {
            services.AddScoped<LoginUser>();
            services.AddTransient<ILoginUser>((sp) => { return sp.GetService<LoginUser>(); });

            return services;
        }
    }
}
