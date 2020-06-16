using Autofac;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Swagger_JWT.Common.Cache.Redis;
using Swagger_JWT.Common.LoginUser;
using Swagger_JWT.Repository.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swagger_JWT.Infrastructure.Middlewares
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly MyDbContext _myDbContext;
        private readonly IRedisCacheManager _cache;

        public AuthenticationMiddleware(RequestDelegate next , MyDbContext myDbContext, IRedisCacheManager cache)
        {
            this._next = next;
            this._myDbContext = myDbContext;
            this._cache = cache;
        }

        public async Task Invoke(
            HttpContext context,
            LoginUser user
           )
        {
            if (context.Request.Path.StartsWithSegments("/swagger") ||
               context.Request.Path.StartsWithSegments("/health") ||
               context.Request.Path.StartsWithSegments("/ping") ||
               context.Request.Path.StartsWithSegments("/api/metrics"))
            {
                await this._next(context);

                return;
            }
         
            if (context.User?.Identity.IsAuthenticated == true)
            {
                var employeeId = context.User.Identity.Name;
                var cacheValue = _cache.GetValue(employeeId);
                if (cacheValue != null)
                {
                    user= JsonConvert.DeserializeObject<LoginUser>(cacheValue);
                }
                else
                {
                    user.EmployeeId = employeeId;
                    user.IsLogin = true;
                    var data = _myDbContext.AspNetUsers.Where(s => s.UserName == employeeId).FirstOrDefault();
                    user.Email = data.Email;
                    _cache.Set(employeeId, user, TimeSpan.FromMinutes(10));
                }
               
            }

                // 取得 Session ID
                var sessionId = context.User?.FindFirst(c => c.Type == "session_id")?.Value;


                if (!string.IsNullOrEmpty(context.Request.ContentType) &&
                    context.Request.ContentType.Contains("multipart/form-data"))
                {
                    await this._next(context);
                }
                else
                {
                    await this._next(context);
                }
            }
            
    }
}
