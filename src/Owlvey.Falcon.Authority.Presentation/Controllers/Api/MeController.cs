using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Owvley.Flacon.Application.Services.Interfaces;
using Owvley.Falcon.Authority.Domain.Core.Manager;
using Owvley.Flacon.Application.Models;

namespace Owlvey.Falcon.Authority.Presentation.Controllers.Api
{
    [Route("api/account")]
    public class MeController : BaseController
    {
        readonly IUserService _userService;
        readonly IUserQueryService _userQueryService;
        public MeController(IDomainManagerService domainManagerService,
                              IUserService userService,
                              IUserQueryService userQueryService) : base(domainManagerService)
        {
            _userService = userService;
            _userQueryService = userQueryService;
        }

        [HttpGet]
        [Route("me")]
        public async Task<IActionResult> GetUserById()
        {
            var userId = this.Request.HttpContext.User.Identity.Name;
            var userRp = await _userQueryService.GetUserById(userId.ToString());

            if (DomainManager.HasNotFounds())
            {
                return this.NotFound(DomainManager.GetNotFounds());
            }

            return this.Ok(userRp);
        }
        
    }
}
