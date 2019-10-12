using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Owvley.Flacon.Application.Services.Interfaces;
using Owvley.Falcon.Authority.Domain.Core.Manager;
using Owvley.Flacon.Application.Models;

namespace Owlvey.Falcon.Authority.Presentation.Controllers.Api
{
    [Authorize(Roles = "globaladmin, authadmin")]
    [Route("api/users")]
    public class UserController : BaseController
    {
        readonly IUserService _userService;
        readonly IUserQueryService _userQueryService;
        public UserController(IDomainManagerService domainManagerService,
                              IUserService userService,
                              IUserQueryService userQueryService) : base(domainManagerService)
        {
            _userService = userService;
            _userQueryService = userQueryService;
        }

        [HttpGet]
        [Route("{userId}")]
        public async Task<IActionResult> GetUserById(string userId)
        {
            if (userId.Equals("me", StringComparison.InvariantCultureIgnoreCase)) {
                userId = this.Request.HttpContext.User.Identity.Name;
            }
            var userRp = await _userQueryService.GetUserById(userId.ToString());

            if (DomainManager.HasNotFounds())
            {
                return this.NotFound(DomainManager.GetNotFounds());
            }

            return this.Ok(userRp);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody]UserPostRp resource)
        {
            if (!ModelState.IsValid)
                return this.BadRequest(ModelState);

            await _userService.CreateUser(resource);

            if (DomainManager.HasConflicts())
            {
                return this.Conflict(DomainManager.GetConflicts());
            }

            return this.Ok(new { UserId = await DomainManager.GetResult<string>("UserId") });
        }

        [HttpPatch]
        [Route("{userId:guid}")]
        public async Task<IActionResult> PatchUser(Guid userId, [FromBody]UserPatchRp resource)
        {
            if (!ModelState.IsValid)
                return this.BadRequest(ModelState);

            await _userService.PatchUser(userId.ToString(), resource);

            if (DomainManager.HasNotFounds())
            {
                return this.Conflict(DomainManager.GetNotFounds());
            }

            if (DomainManager.HasConflicts())
            {
                return this.Conflict(DomainManager.GetConflicts());
            }

            return this.Ok(this.DefaultResponse);
        }
    }
}
