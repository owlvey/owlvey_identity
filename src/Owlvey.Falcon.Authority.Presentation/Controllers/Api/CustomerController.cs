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
    [Route("api/customers")]
    public class CustomerController : BaseController
    {
        readonly ICustomerService _CustomerService;
        readonly ICustomerQueryService _CustomerQueryService;
        public CustomerController(IDomainManagerService domainManagerService,
                              ICustomerService CustomerService,
                              ICustomerQueryService CustomerQueryService) : base(domainManagerService)
        {
            _CustomerService = CustomerService;
            _CustomerQueryService = CustomerQueryService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody]CustomerPostRp resource)
        {
            if (!ModelState.IsValid)
                return this.BadRequest(ModelState);

            await _CustomerService.CreateCustomer(resource);

            if (DomainManager.HasConflicts())
            {
                return this.Conflict(DomainManager.GetConflicts());
            }

            return this.Ok(base.DefaultResponse);
        }

        [HttpGet]
        [Route("{customerId:guid}")]
        public async Task<IActionResult> GetCustomerById(Guid customerId)
        {
            var customerRp = await _CustomerQueryService.GetCustomerById(customerId);

            if (DomainManager.HasNotFounds())
            {
                return this.NotFound(DomainManager.GetNotFounds());
            }

            return this.Ok(customerRp);
        }
    }
}
