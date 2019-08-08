using Owvley.Flacon.Application.Models;
using Owvley.Flacon.Application.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;
using Owvley.Falcon.Authority.Domain.Core.Manager;
using TheRoostt.Authority.Domain.Interfaces;
using Owvley.Falcon.Authority.Domain.Models;

namespace Owvley.Flacon.Application.Services
{
    public class UserService : IUserService
    {
        readonly IDomainManagerService _domainManagerService;
        readonly IUserRepository _userRepository;
        readonly UserManager<User> _userManager;

        public UserService(IDomainManagerService domainManagerService,
                           IUserRepository userRepository,
                           UserManager<User> userManager)
        {
            _domainManagerService = domainManagerService;
            _userRepository = userRepository;
            _userManager = userManager;
        }

        public async Task CreateUser(UserPostRp resource)
        {
            var user = new User { UserName = resource.Email, Email = resource.Email, FirstName = resource.GivenName, LastName = resource.Surname };
            var result = await _userManager.CreateAsync(user, resource.Password);
            if (!result.Succeeded)
            {
                await _domainManagerService.AddConflict(string.Join(", ", result.Errors));
                return;
            }

            await _userManager.AddClaimAsync(user, new Claim("givenname", resource.GivenName));
            await _userManager.AddClaimAsync(user, new Claim("fullname", $"{resource.GivenName} {resource.Surname}"));
            await _userManager.AddToRoleAsync(user, "agent");

            await _domainManagerService.AddResult("UserId", user.Id);
        }

        public async Task PatchUser(string userId, UserPatchRp resource)
        {
            User existingUser = await _userRepository.GetUserById(userId);
            if (existingUser == null)
            {
                await _domainManagerService.AddNotFound($"The user with id {userId} does not exist.");
                return;
            }

            if (resource.EmailConfirmed.HasValue)
            {
                if (existingUser.IsEmailConfirmed())
                {
                    await _domainManagerService.AddConflict($"The email is already confirmed.");
                    return;
                }
                existingUser.ConfirmEmail();

                _userRepository.Update(existingUser);
                await _userRepository.SaveChanges();
            }
        }
    }
}
