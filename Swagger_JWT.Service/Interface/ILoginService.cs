using Swagger_JWT.Service.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Swagger_JWT.Service.Interface
{
    public interface ILoginService
    {
        Task<string> Station(string para);

        Task<IEnumerable<ApiClaimsDto>> ApiClaims();
    }
}
