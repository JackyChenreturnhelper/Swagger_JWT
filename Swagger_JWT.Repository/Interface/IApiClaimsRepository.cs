using Swagger_JWT.Repository.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Swagger_JWT.Repository.Interface
{
  public  interface IApiClaimsRepository
    {
        Task<IEnumerable<ApiClaims>> ApiClaims();
    }
}
