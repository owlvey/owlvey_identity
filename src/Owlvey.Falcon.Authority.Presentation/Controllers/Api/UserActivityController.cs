using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Owvley.Flacon.Application.Services.Interfaces;
using Owvley.Falcon.Authority.Domain.Core.Manager;
using Owvley.Flacon.Application.Models;

namespace Owlvey.Falcon.Authority.Presentation.Controllers.Api
{
    [Authorize(Roles = "globaladmin, authadmin, portaladmin")]
    [Authorize]
    [Route("api/users")]
    public class UserActivityController : BaseController
    {
        readonly IUserActivityService _userActivityService;
        readonly IUserActivityQueryService _userActivityQueryService;
        public UserActivityController(IDomainManagerService domainManagerService,
                                      IUserActivityService userActivityService,
                                      IUserActivityQueryService userActivityQueryService) : base(domainManagerService)
        {
            _userActivityService = userActivityService;
            _userActivityQueryService = userActivityQueryService;
        }
        
        [HttpPost]
        [Route("{userId}/activities")]
        public async Task<IActionResult> CreateActivity(string userId, [FromBody]UserActivityPostRp resource)
        {
            if (!ModelState.IsValid)
                return this.BadRequest(ModelState);

            await _userActivityService.CreateActivity(userId, resource);

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

        [HttpGet]
        [Route("{userId}/activities")]
        public async Task<IActionResult> GetActivities(string userId)
        {
            if (!ModelState.IsValid)
                return this.BadRequest(ModelState);

            var activities = await _userActivityQueryService.GetUserActivities(userId);

            if (DomainManager.HasNotFounds())
            {
                return this.Conflict(DomainManager.GetNotFounds());
            }

            return this.Ok(activities);
        }

    }
}
