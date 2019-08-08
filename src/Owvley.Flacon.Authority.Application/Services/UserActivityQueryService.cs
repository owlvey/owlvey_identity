using Owvley.Falcon.Authority.Domain.Core.Manager;
using Owvley.Falcon.Authority.Domain.Models;
using Owvley.Flacon.Application.Models;
using Owvley.Flacon.Application.Services.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using TheRoostt.Authority.Domain.Interfaces;

namespace Owvley.Flacon.Application.Services
{
    public class UserActivityQueryService : IUserActivityQueryService
    {
        readonly IDomainManagerService _domainManagerService;
        readonly IUserRepository _userRepository;
        public UserActivityQueryService(IDomainManagerService domainManagerService,
                                        IUserRepository userRepository)
        {
            _domainManagerService = domainManagerService;
            _userRepository = userRepository;
        }

        public async Task<UserActivityListRp> GetUserActivities(string userId)
        {
            User user = await _userRepository.GetUserById(userId);
            if (user == null)
            {
                await _domainManagerService.AddNotFound($"The user with id {userId} does not exists.");
                return null;
            }

            var activities = user.GetUserActivities();

            UserActivityListRp userActivityListRp = new UserActivityListRp()
            {
                Items = activities.Select(x => new UserActivityListItemRp()
                {
                    Type = x.Type,
                    Data = x.Data
                }).ToList()
            };

            return userActivityListRp;
        }
    }
}
