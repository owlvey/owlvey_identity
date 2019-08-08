using Owvley.Flacon.Application.Interfaces;
using Owvley.Flacon.Application.Models;
using Owvley.Flacon.Application.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Owvley.Falcon.Authority.Domain.Core.Manager;
using TheRoostt.Authority.Domain.Interfaces;
using Owvley.Falcon.Authority.Domain.Models;
using Owvley.Flacon.Authority.Application.Interfaces;

namespace Owvley.Flacon.Application.Services
{
    public class UserPreferenceService : IUserPreferenceService
    {
        readonly UserManager<User> _userManager;
        readonly IDomainManagerService _domainManagerService;
        readonly IUserRepository _userRepository;
        readonly IIdentityService _identityService;
        public UserPreferenceService(UserManager<User> userManager,
                                     IDomainManagerService domainManagerService,
                                     IUserRepository userRepository,
                                     IIdentityService identityService)
        {
            _userManager = userManager;
            _domainManagerService = domainManagerService;
            _userRepository = userRepository;
            _identityService = identityService;
        }

        public async Task UpdatePreferenceBulk(string userId, UserPreferenceBulkPostRp resource)
        {
            var user = await _userRepository.GetUserById(_identityService.GetUserId());
            if (user == null)
            {
                await _domainManagerService.AddNotFound($"The user with id {userId} does not exists.");
                return;
            }

            var claims = await _userManager.GetClaimsAsync(user);

            foreach (var item in resource.Items)
            {
                var preference = user.GetPreference(item.Name);
                if(preference == null)
                {
                    await _domainManagerService.AddConflict($"The preference with name {item.Name} does not exists.");
                    return;
                }

                user.UpdatePreference(item.Name, item.Value);

                if(item.Name.Equals("Theme", StringComparison.InvariantCultureIgnoreCase))
                {
                    var upThemeClaim = claims.FirstOrDefault(x => x.Type.Equals("up_theme"));
                    await _userManager.RemoveClaimAsync(user, upThemeClaim);
                    await _userManager.AddClaimAsync(user, new Claim("up_theme", item.Value));
                }

                if (item.Name.Equals("Culture", StringComparison.InvariantCultureIgnoreCase))
                {
                    var upCultureClaim = claims.FirstOrDefault(x => x.Type.Equals("up_culture"));
                    await _userManager.RemoveClaimAsync(user, upCultureClaim);
                    await _userManager.AddClaimAsync(user, new Claim("up_culture", item.Value));
                }

                if (item.Name.Equals("TimeZone", StringComparison.InvariantCultureIgnoreCase))
                {
                    var upTimezoneClaim = claims.FirstOrDefault(x => x.Type.Equals("up_timezone"));
                    await _userManager.RemoveClaimAsync(user, upTimezoneClaim);
                    await _userManager.AddClaimAsync(user, new Claim("up_timezone", item.Value));
                }

            }

            _userRepository.Update(user);
            await _userRepository.SaveChanges();

        }
    }
}
