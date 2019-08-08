using Owvley.Falcon.Authority.Domain.Core.Manager;
using Owvley.Flacon.Application.Models;
using Owvley.Flacon.Application.Services.Interfaces;
using System.Threading.Tasks;
using TheRoostt.Authority.Domain.Interfaces;

namespace Owvley.Flacon.Application.Services
{
    public class UserActivityService : IUserActivityService
    {
        readonly IDomainManagerService _domainManagerService;
        readonly IUserRepository _userRepository;
        public UserActivityService(IDomainManagerService domainManagerService,
                                   IUserRepository userRepository)
        {
            _domainManagerService = domainManagerService;
            _userRepository = userRepository;
        }

        public async Task CreateActivity(string userId, UserActivityPostRp resource)
        {
            var user = await _userRepository.GetUserById(userId);
            if (user == null)
            {
                await _domainManagerService.AddNotFound($"The user with id {userId} does not exists.");
                return;
            }

            user.AddActivity(resource.Type, resource.Data);

            _userRepository.Update(user);
            await _userRepository.SaveChanges();
        }
    }
}
