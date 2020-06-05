using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Swagger_JWT.Infrastructure.Dependency
{
    public static class SwaggerSetup
    {
      
        public static void AddSwaggerSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

             var ApiName  = "jacky";
        services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Swagger_JWT  Sample",
                    Version = "v1",
                    Description = $"{ApiName} HTTP API V1",
                    Contact = new OpenApiContact { Name = ApiName, Email = "jacky6632004", Url = new Uri("https://www.youtube.com/") },
                    License = new OpenApiLicense { Name = ApiName, Url = new Uri("https://www.youtube.com/") }
                });
                options.DescribeAllEnumsAsStrings();
                options.IgnoreObsoleteActions();


                // Set the comments path for the Swagger JSON and UI.
                var basePath = AppContext.BaseDirectory;
                var xmlFiles = Directory.EnumerateFiles(basePath, "*.xml", SearchOption.TopDirectoryOnly);
                var xmlFileo = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFileo);
                options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
                foreach (var xmlFile in xmlFiles)
                {
                    options.IncludeXmlComments(xmlFile);
                }

                options.OperationFilter<AddResponseHeadersFilter>();
                options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();

                options.OperationFilter<SecurityRequirementsOperationFilter>();
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "JWT授權(數據將在請求頭中進行傳輸) 直接在下框中輸入Bearer {token}（注意兩者之間是一個空格）\"",
                    Name = "Authorization",//jwt默認的參數名稱
                    In = ParameterLocation.Header,//jwt默認存放Authorization信息的位置(請求頭中)
                    Type = SecuritySchemeType.ApiKey
                });
            });
        }
    }
}
