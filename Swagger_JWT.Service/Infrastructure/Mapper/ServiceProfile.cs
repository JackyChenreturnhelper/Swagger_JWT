using AutoMapper;
using Swagger_JWT.Repository.Model;
using Swagger_JWT.Service.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Swagger_JWT.Service.Infrastructure
{
    public class ServiceProfile : Profile
    {
        public ServiceProfile()
        {
            this.CreateMap<ApiClaims, ApiClaimsDto>();
        }
    }
}
