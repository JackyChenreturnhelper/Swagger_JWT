﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swagger_JWT.Common.Helper;
using Swagger_JWT.Common.HttpContextUser;
using Swagger_JWT.Service.Dto;
using Swagger_JWT.Service.Interface;
using Swagger_JWT.ViewModel;

namespace Swagger_JWT.Controllers
{

    /// <summary>
    /// 登入
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly JwtHelper _jwt;

        private readonly ILoginService _loginService;
        private readonly IMapper _mapper;
        private readonly IUser _user;
        public LoginController(JwtHelper jwt, ILoginService loginService, IMapper mapper, IUser user)
        {
            this._jwt = jwt;
            this._loginService = loginService;
            this._mapper = mapper;
            this._user = user;
        }

        [HttpGet]
        [Route("GetJwtStr")]
        public async Task<object> GetJwtStr(string name, string pass)
        {
            string jwtStr = string.Empty;
            bool suc = false;

            // 獲取用戶的角色名，請暫時忽略其內部是如何獲取的，可以直接用 var userRole="Admin"; 來代替更好理解。
            var userRole = "Admin";
            if (userRole != null)
            {
                // 將用戶id和角色名，作為單獨的自定義變量封裝進 token 字符串中。
                TokenModelJwt tokenModel = new TokenModelJwt { Uid = 1, Role = userRole,Name= name };
                jwtStr = _jwt.IssueJwt(tokenModel);//登錄，獲取到一定規則的 Token 令牌
                suc = true;
            }
            else
            {
                jwtStr = "login fail!!!";
            }

            return Ok(new
            {
                success = suc,
                token = jwtStr
            });
        }

        [HttpGet]
        [Route("test")]
        public async Task<string> test(string para)
        {

            return await _loginService.Station(para);

        }

        [HttpGet]
        [Route("ApiClaims")]
        public async Task<IEnumerable<ApiClaimsViewModel>> ApiClaims()
        {
            var data = await _loginService.ApiClaims();
            var result = _mapper.Map<IEnumerable<ApiClaimsDto>, IEnumerable<ApiClaimsViewModel>>(data);
            return result;

        }

        [HttpGet]
        [Route("UserInfo")]
        public async Task<string> GetUserInfo(string ClaimType = "jti")
        {
            var getUserInfoByToken = _user.GetUserInfoFromToken(ClaimType);

            var IsAuthenticated = _user.IsAuthenticated();
            var Name = _user.Name;
            var GetClaimValueByType = _user.GetClaimValueByType(ClaimType);

            return Name;
        }
    }
}
