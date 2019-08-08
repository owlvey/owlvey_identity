using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Owvley.Flacon.Application.Services.Interfaces;
using Owvley.Falcon.Authority.Domain.Core.Manager;
using Owvley.Flacon.Application.Models;

namespace Owlvey.Falcon.Authority.Presentation.Controllers.Api
{
    [Authorize(Roles = "globaladmin, authadmin")]
    [Route("api/members")]
    public class MemberController : BaseController
    {
        readonly ICustomerMemberService _CustomerMemberService;
        public MemberController(IDomainManagerService domainManagerService,
                                ICustomerMemberService CustomerMemberService) : base(domainManagerService)
        {
            _CustomerMemberService = CustomerMemberService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomerMember([FromBody]CustomerMemberPostRp resource)
        {
            if (!ModelState.IsValid)
                return this.BadRequest(ModelState);

            await _CustomerMemberService.CreateCustomerMember(resource);

            if (DomainManager.HasConflicts())
            {
                return this.Conflict(DomainManager.GetConflicts());
            }

            return this.Ok(base.DefaultResponse);
        }
    }
}
