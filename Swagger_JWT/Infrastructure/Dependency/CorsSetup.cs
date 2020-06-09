using Microsoft.Extensions.DependencyInjection;
using Swagger_JWT.Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swagger_JWT.Infrastructure.Dependency
{
    public static class CorsSetup
    {
        public static void AddCorsSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            /////不推薦
            //services.AddCors(cors =>
            //{
            //    cors.AddPolicy("AllRequests", policy =>
            //    {
            //        policy.AllowAnyOrigin()//任何來源
            //        .AllowAnyMethod()//任何方式
            //        .AllowAnyHeader()//任何表投
            //        .AllowCredentials();//任何COOKIE
            //    });
            //});

            services.AddCors(c =>
            {
                //一般采用這種方法
                c.AddPolicy("LimitRequests", policy =>
                {
                    // 支持多個域名端口，註意端口號後不要帶/斜桿：比如localhost:8000/，是錯的
                    // 註意，http: 和 http://localhost:540 是不一樣的，盡量寫兩個
                    policy
                    .WithOrigins(Appsettings.app(new string[] { "Cors", "IPs" }).Split(','))
                    .AllowAnyHeader()//Ensures that the policy allows any header.
                    .AllowAnyMethod();
                });
            });
        }
    }
}
