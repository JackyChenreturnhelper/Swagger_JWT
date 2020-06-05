using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Swagger_JWT.Common.Helper;
using Swagger_JWT.Infrastructure.Dependency;
using Swagger_JWT.Infrastructure.Filter;
using Swashbuckle.AspNetCore.Filters;

namespace Swagger_JWT
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private string ApiName { get; set; } = "jacky";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<JwtHelper>();
            services.AddAuthorizationSetup(Configuration);
            services.AddControllers(options =>
            {
                options.Filters.Add<ActionResultFilter>();
               
            });

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

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "");
            });
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

          

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
