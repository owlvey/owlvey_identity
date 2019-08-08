using Owvley.Falcon.Authority.Domain.Core.Manager;
using Owvley.Falcon.Authority.Domain.Models;
using Owvley.Flacon.Application.Interfaces;
using Owvley.Flacon.Application.Models;
using Owvley.Flacon.Application.Services.Interfaces;
using Owvley.Flacon.Authority.Application.Interfaces;
using System.Threading.Tasks;
using TheRoostt.Authority.Domain.Interfaces;

namespace Owvley.Flacon.Application.Services
{
    public class CustomerService : ICustomerService
    {
        readonly IDomainManagerService _domainManagerService;
        readonly IIdentityService _identityService;
        readonly ICustomerRepository _customerRepository;
        readonly IUserRepository _userRepository;
        public CustomerService(IDomainManagerService domainManagerService,
                              IIdentityService identityService,
                              ICustomerRepository customerRepository,
                              IUserRepository userRepository)
        {
            _domainManagerService = domainManagerService;
            _customerRepository = customerRepository;
            _identityService = identityService;
            _userRepository = userRepository;
        }

        public async Task CreateCustomer(CustomerPostRp resource)
        {
            string createdBy = _identityService.GetClientId();

            User existingUser = await _userRepository.GetUserById(resource.AdminId.ToString());
            if (existingUser == null)
            {
                await _domainManagerService.AddConflict($"The user with id {resource.AdminId} does not exists.");
                return;
            }

            Customer existingCustomer = await _customerRepository.GetCustomerByName(resource.Name);
            if (existingCustomer != null)
            {
                await _domainManagerService.AddConflict($"The team with name {resource.Name} has already been taken.");
                return;
            }

            Customer newCustomer = Customer.Factory.Create(resource.CustomerId, resource.Name, resource.AdminId.ToString(),
                                               resource.AdminName, resource.AdminEmail, createdBy);

            _customerRepository.Add(newCustomer);
            await _customerRepository.SaveChanges();
        }
    }
}
