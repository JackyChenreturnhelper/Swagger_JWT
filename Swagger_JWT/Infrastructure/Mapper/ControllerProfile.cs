using AutoMapper;
using Swagger_JWT.Service.Dto;
using Swagger_JWT.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swagger_JWT.Infrastructure.Mapper
{
    public class ControllerProfile : Profile
    {
        public ControllerProfile()
        {
            this.CreateMap<ApiClaimsDto, ApiClaimsViewModel>();
        }
    }
}
