using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Owvley.Flacon.Application.Models;
using Owvley.Falcon.Authority.Domain.Core.Manager;
using Owvley.Flacon.Application.Services.Interfaces;

namespace Owlvey.Falcon.Authority.Presentation.Controllers.Api
{
    [Authorize]
    [Route("api/users")]
    public class UserPreferenceController : BaseController
    {
        readonly IUserPreferenceService _userPreferenceService;
        public UserPreferenceController(IDomainManagerService domainManagerService,
                                        IUserPreferenceService userPreferenceService) : base(domainManagerService)
        {
            _userPreferenceService = userPreferenceService;
        }
        
        [HttpPost]
        [Route("{userId}/preferences")]
        [ActionProfile(Name = "bulkupdate")]
        public async Task<IActionResult> UpdatePreferences(string userId, [FromBody]UserPreferenceBulkPostRp resource)
        {
            if (!ModelState.IsValid)
                return this.BadRequest(ModelState);

            await _userPreferenceService.UpdatePreferenceBulk(userId, resource);

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
