using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swagger_JWT.Infrastructure.Dependency
{
    public static class DBSetup
    {
          public static void AddDBSetup(this IServiceCollection services , IConfiguration Configuration)
        {
            var SQL = Configuration.GetValue<string>("ConnectionStrings:SQL");
            var MySQL = Configuration.GetValue<string>("ConnectionStrings:MySQL");
        }
    }
}
