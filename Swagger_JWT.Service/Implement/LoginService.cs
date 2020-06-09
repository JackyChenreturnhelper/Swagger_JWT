using AutoMapper;
using Swagger_JWT.Common;
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
        public LoginService(string name, IApiClaimsRepository apiClaimsRepository, IMapper mappe)
        {
            this._name = name;
            this._apiClaimsRepository = apiClaimsRepository;
            this._mapper = mappe;
        }

        public async Task<IEnumerable<ApiClaimsDto>> ApiClaims()
        {
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
