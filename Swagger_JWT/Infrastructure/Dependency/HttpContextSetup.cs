using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Swagger_JWT.Common.HttpContextUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swagger_JWT.Infrastructure.Dependency
{
    public static class HttpContextSetup
    {
        public static void AddHttpContextSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUser, AspNetUser>();
        }
    }
}
