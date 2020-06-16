using AutoMapper;
using Swagger_JWT.Common;
using Swagger_JWT.Common.HttpContextUser;
using Swagger_JWT.Common.LoginUser;
using Swagger_JWT.Repository.Interface;
using Swagger_JWT.Repository.Model;
using Swagger_JWT.Service.Dto;
using Swagger_JWT.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swagger_JWT.Service.Implement
{
    public class LoginService : ILoginService
    {
        private readonly string _name;
        private readonly IApiClaimsRepository _apiClaimsRepository;
        private IMapper _mapper;
        private readonly ILoginUser _loginUser;

        public LoginService(string name, IApiClaimsRepository apiClaimsRepository, IMapper mappe, ILoginUser loginUser)
        {
            this._name = name;
            this._apiClaimsRepository = apiClaimsRepository;
            this._mapper = mappe;
            this._loginUser = loginUser;
        }

        public async Task<IEnumerable<ApiClaimsDto>> ApiClaims()
        {

            var user = _loginUser.DepartmentId;
            var data= await _apiClaimsRepository.ApiClaims();
            var result = this._mapper.Map<IEnumerable<ApiClaims>, IEnumerable<ApiClaimsDto>>(data);

            return result;

        }

        [Caching(AbsoluteExpiration = 10)]
        public async  Task<string> Station(string para)
        {

           var bb= await _apiClaimsRepository.ApiClaims();
            string aa = "ssss" + _name + "1111" + bb.FirstOrDefault().Type;

            return aa;
;        }
    }
}
