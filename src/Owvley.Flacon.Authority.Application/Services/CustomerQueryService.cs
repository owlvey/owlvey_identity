using Owvley.Falcon.Authority.Domain.Core.Manager;
using Owvley.Falcon.Authority.Domain.Models;
using Owvley.Flacon.Application.Models;
using Owvley.Flacon.Application.Services.Interfaces;
using System;
using System.Threading.Tasks;
using TheRoostt.Authority.Domain.Interfaces;

namespace Owvley.Flacon.Application.Services
{
    public class CustomerQueryService : ICustomerQueryService
    {
        readonly IDomainManagerService _domainManagerService;
        readonly ICustomerRepository _customerRepository;
        public CustomerQueryService(IDomainManagerService domainManagerService,
                                    ICustomerRepository customerRepository)
        {
            _domainManagerService = domainManagerService;
            _customerRepository = customerRepository;
        }

        public async Task<CustomerGetRp> GetCustomerById(Guid teamId)
        {
            Customer customer = await _customerRepository.GetCustomerById(teamId); ;

            if (customer == null)
            {
                await _domainManagerService.AddNotFound($"The customer with id {teamId} does not exists or you don't have enough permissions to get it.");
                return null;
            }

            CustomerGetRp teamGetRp = new CustomerGetRp()
            {
                TeamId = customer.CustomerId,
                Name = customer.Name,
                AdminName = customer.AdminName,
                AdminEmail = customer.AdminEmail
            };

            return teamGetRp;
        }
    }
}
