using Owvley.Falcon.Authority.Domain.Core.Manager;
using Owvley.Falcon.Authority.Domain.Models;
using Owvley.Flacon.Application.Interfaces;
using Owvley.Flacon.Application.Models;
using Owvley.Flacon.Application.Services.Interfaces;
using Owvley.Flacon.Authority.Application.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
using TheRoostt.Authority.Domain.Interfaces;

namespace Owvley.Flacon.Application.Services
{
    public class UserQueryService : IUserQueryService
    {
        readonly IDomainManagerService _domainManagerService;
        readonly IUserRepository _userRepository;
        readonly IIdentityService _identityService;
        public UserQueryService(IDomainManagerService domainManagerService,
                                IUserRepository userRepository,
                                IIdentityService identityService)
        {
            _domainManagerService = domainManagerService;
            _userRepository = userRepository;
            _identityService = identityService;
        }

        public async Task<UserGetRp> GetUserById(string userId)
        {
            User user = await _userRepository.GetUserById(userId);
            if (user == null)
            {
                await _domainManagerService.AddNotFound($"The user with id {userId} does not exists.");
                return null;
            }

            UserGetRp userGetRp = new UserGetRp()
            {
                UserId = Guid.Parse(user.Id),
                GivenName = user.FirstName,
                Surname = user.LastName,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed
            };

            return userGetRp;
        }
        
        public async Task<UserCustomerListRp> GetUserCustomers(string userId, string customerName)
        {
            User user = await _userRepository.GetUserById(_identityService.GetUserId());
            if (user == null)
            {
                await _domainManagerService.AddNotFound($"The user with id {userId} does not exists.");
                return null;
            }

            var customers = await _userRepository.GetUserCustomersByQuery(_identityService.GetUserId(), customerName);
            
            UserCustomerListRp userTeamListRp = new UserCustomerListRp()
            {
                Items = customers.Select(x=> new UserCustomerListItemRp()
                {
                    CustomerId = x.CustomerId,
                    Name = x.Name,
                    AdminEmail = x.AdminEmail,
                    AdminName = x.AdminName
                }).ToList()
            };

            return userTeamListRp;
        }
    }
}
