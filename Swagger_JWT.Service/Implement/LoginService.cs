using Swagger_JWT.Common;
using Swagger_JWT.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Swagger_JWT.Service.Implement
{
    public class LoginService : ILoginService
    {
        [Caching(AbsoluteExpiration = 10)]
        public async  Task<string> Station(string para)
        {
            string aa = "ssss";

            return aa;
;        }
    }
}
