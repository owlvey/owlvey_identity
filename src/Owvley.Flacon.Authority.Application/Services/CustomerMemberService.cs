using System.Threading.Tasks;
using Owvley.Falcon.Authority.Domain.Core.Manager;
using Owvley.Falcon.Authority.Domain.Models;
using Owvley.Flacon.Application.Interfaces;
using Owvley.Flacon.Application.Models;
using Owvley.Flacon.Application.Services.Interfaces;
using Owvley.Flacon.Authority.Application.Interfaces;
using TheRoostt.Authority.Domain.Interfaces;

namespace Owvley.Flacon.Application.Services
{
    public class CustomerMemberService : ICustomerMemberService
    {
        readonly IDomainManagerService _domainManagerService;
        readonly IIdentityService _identityService;
        readonly IUserRepository _userRepository;
        readonly ICustomerRepository _customerRepository;
        readonly IActivityMonitorService _activityMonitorService;

        public CustomerMemberService(IDomainManagerService domainManagerService,
                                 IIdentityService identityService,
                                 IUserRepository userRepository,
                                 ICustomerRepository customerRepository,
                                 IActivityMonitorService activityMonitorService)
        {
            _domainManagerService = domainManagerService;
            _identityService = identityService;
            _userRepository = userRepository;
            _customerRepository = customerRepository;
            _activityMonitorService = activityMonitorService;
        }

        public async Task CreateCustomerMember(CustomerMemberPostRp resource)
        {
            string createdBy = _identityService.GetClientId();

            User existingUser = await _userRepository.GetUserByEmail(resource.Email);

            Customer customer = await _customerRepository.GetCustomerById(resource.CustomerId);
            if (customer == null)
            {
                await _domainManagerService.AddConflict($"The customer with id {resource.CustomerId} does not exists.");
                return;
            }

            var existingMember = customer.GetMemberByEmail(resource.Email);
            if (existingMember != null)
            {
                await _domainManagerService.AddConflict($"The member with email {resource.Email} has already been added.");
                return;
            }

            var newMember = customer.AddMember(resource.CustomerMemberId, existingUser?.Id , resource.Email, resource.Role, createdBy);

            _customerRepository.Update(customer);
            await _customerRepository.SaveChanges();
        }
    }
}
