using Owlvey.Falcon.Authority.Infra.Data.SqlServer.Models;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using static TheRoostt.Authority.Domain.DomainConstants;
using Owvley.Falcon.Authority.Domain.Models;
using TheRoostt.Authority.Domain.Interfaces;

namespace Owlvey.Falcon.Authority.Infra.Data.SqlServer.Profiles
{
    public class FalconProfileService : IProfileService
    {
        private readonly IUserClaimsPrincipalFactory<User> _claimsFactory;
        private readonly UserManager<User> _userManager;
        private readonly ICustomerRepository _customerRepository;
        public FalconProfileService(IUserClaimsPrincipalFactory<User> claimsFactory, 
                                   UserManager<User> userManager,
                                   ICustomerRepository customerRepository)
        {
            _claimsFactory = claimsFactory;
            _userManager = userManager;
            _customerRepository = customerRepository;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();

            var user = await _userManager.FindByIdAsync(sub);
            var principal = await _claimsFactory.CreateAsync(user);
            
            var cs = principal.Claims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();
            
            if (principal.IsInRole(Roles.Guest))
            {
                string upCourier = principal.Claims.First(x => x.Type.Equals("up_customer", StringComparison.InvariantCultureIgnoreCase)).Value;
                string customerId = string.Empty;
                string customerName = string.Empty;
                if (upCourier.Equals("lastsignin", StringComparison.InvariantCultureIgnoreCase))
                {
                    var lastSignInActivity = user.GetLastAccessAccountActivity();
                    if (lastSignInActivity == null)
                    {
                        var lastAssignedTeam = user.GetLastAssignedCustomer();
                        customerId = lastAssignedTeam.CustomerId.ToString();
                        var team = await _customerRepository.GetCustomerById(Guid.Parse(customerId));
                        customerId = team.CustomerId.ToString();
                        customerName = team.Name;
                    }
                    else
                    {
                        var lastSignInActivityModel = JsonConvert.DeserializeObject<CustomerModel>(lastSignInActivity.Data);
                        customerId = lastSignInActivityModel.CustomerId;
                        customerName = lastSignInActivityModel.CustomerName;
                    }
                }
                else
                {
                    customerId = upCourier;
                }

                cs.Add(new System.Security.Claims.Claim("customer_id", customerId));
                cs.Add(new System.Security.Claims.Claim("customer_name", customerName));
                
                var teamModel = await _customerRepository.GetCustomerById(Guid.Parse(customerId));
                var teamMember = teamModel.GetTeamMemberById(user.Id);
                cs.Add(new System.Security.Claims.Claim("customer_role", teamMember.Role.ToString()));
            }
            
            context.IssuedClaims = cs;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            context.IsActive = user != null;
        }
    }
}
