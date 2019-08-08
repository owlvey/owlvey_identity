using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Owvley.Flacon.Application.Services.Interfaces;
using Owvley.Falcon.Authority.Domain.Core.Manager;

namespace Owlvey.Falcon.Authority.Presentation.Controllers.Api
{
    [Authorize]
    [Route("api/users")]
    public class UserCustomerController : BaseController
    {
        readonly IUserQueryService _userQueryService;
        public UserCustomerController(IDomainManagerService domainManagerService,
                                      IUserQueryService userQueryService) : base(domainManagerService)
        {
            _userQueryService = userQueryService;
        }
        
        [HttpGet]
        [Route("{userId}/customers")]
        public async Task<IActionResult> GetUserCustomers(string userId, [FromQuery(Name = "name")]string name)
        {
            var Customers = await _userQueryService.GetUserCustomers(userId, name);

            if (DomainManager.HasNotFounds())
            {
                return this.Conflict(DomainManager.GetNotFounds());
            }

            return this.Ok(Customers);
        }

    }
}
