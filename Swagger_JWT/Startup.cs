using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Swagger_JWT.Common.Helper;
using Swagger_JWT.Infrastructure.Dependency;
using Swagger_JWT.Infrastructure.Filter;
using Swagger_JWT.Infrastructure.Mapper;
using Swagger_JWT.Infrastructure.Middlewares;
using Swagger_JWT.Repository.DB;
using Swagger_JWT.Service.Infrastructure;
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



        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

     

            services.AddSingleton<JwtHelper>();

            services.AddSingleton(new Appsettings(Configuration));

            services.AddSingleton(new MyDbContext());

            services.AddAutoMapper
          (
              typeof(ServiceProfile).Assembly,
              typeof(ControllerProfile).Assembly
          );

            services.AddPrePareLoginUserSetup();

            services.AddCacheSetup();

            services.AddAuthorizationSetup(Configuration);

            services.AddSwaggerSetup();

            services.AddCorsSetup();

            services.AddHttpContextSetup();

            services.AddIpPolicyRateLimitSetup(Configuration);

            

            services.Configure<KestrelServerOptions>(x => x.AllowSynchronousIO = true)
                    .Configure<IISServerOptions>(x => x.AllowSynchronousIO = true);
            services.AddControllers(options =>
            {
                options.Filters.Add<ActionResultFilter>();
               
            })  //全局配置Json序列化處理
            .AddNewtonsoftJson(options =>
            {
                //忽略循環引用
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                //不使用駝峰樣式的key
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                //設置時間格式
                //options.SerializerSettings.DateFormatString = "yyyy-MM-dd";
            });


        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new AutofacModuleRegister());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("LimitRequests");

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "");
            });
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();
            app.UseMiddleware<AuthenticationMiddleware>();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
