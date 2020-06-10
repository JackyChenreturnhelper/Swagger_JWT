using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swagger_JWT.Common.Helper;

namespace Swagger_JWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IHttpContextAccessor _accessor;
        public ValuesController(IHttpContextAccessor accessor)
        {
            this._accessor = accessor;
        }

        [HttpGet]
        [Route("test1")]
        public async Task<string> test()
        {
            var aa = _accessor.HttpContext;
            var tt = _accessor.HttpContext.Request.Headers.TryGetValue("Authorization", out var bb);
            var tt1 = _accessor.HttpContext.Request.Headers["Authorization"].ObjToString();

            var ba = User.Claims.Select(p => new { p.Type, p.Value });

            var aaa = User.Identity.Name;
            var jti = User.Claims.FirstOrDefault(p => p.Type == "jti");
            return "";
        }
    }
}
