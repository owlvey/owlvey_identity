﻿using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

namespace Owlvey.Falcon.Authority.Infra.CrossCutting.Identity
{
    public interface IIdentityService
    {
        string GetClientId();
        string GetUserId();
        string GetUserName();
        string GetUserEmail();
    }
    public class IdentityService : IIdentityService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IdentityService(IHttpContextAccessor httpContextAccessor)
        {
            this._httpContextAccessor = httpContextAccessor;
        }

        public string GetClientId()
        {
            return this._httpContextAccessor.HttpContext.User.FindFirst(c => c.Type.Equals("client_id")).Value;
        }

        public string GetUserId()
        {
            return this._httpContextAccessor.HttpContext.User.FindFirst(c => c.Type.Equals(ClaimTypes.NameIdentifier, StringComparison.InvariantCultureIgnoreCase) || c.Type.Equals("sub", StringComparison.InvariantCultureIgnoreCase)).Value;
        }

        public string GetUserName()
        {
            return this._httpContextAccessor.HttpContext.User.FindFirst(c => c.Type == "email").Value;
        }

        public string GetUserEmail()
        {
            return this._httpContextAccessor.HttpContext.User.FindFirst(c => c.Type == "email").Value;
        }
    }
}
