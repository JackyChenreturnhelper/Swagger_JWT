using Swagger_JWT.Repository.DB;
using Swagger_JWT.Repository.Interface;
using Swagger_JWT.Repository.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace Swagger_JWT.Repository.Implement
{
   public class ApiClaimsRepository : IApiClaimsRepository
    {
        private readonly MyDbContext _myDbContext;

        public ApiClaimsRepository(MyDbContext myDbContext)
        {
            this._myDbContext = myDbContext;
        }

        public async Task<IEnumerable<ApiClaims>> ApiClaims()
        {
           return _myDbContext.ApiClaims;

        }
    }
}
