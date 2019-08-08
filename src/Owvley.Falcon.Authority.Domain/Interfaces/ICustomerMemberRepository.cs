using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Owvley.Falcon.Authority.Domain.Models;
using TheRoostt.Authority.Domain.Models;

namespace Owvley.Falcon.Authority.Domain.Interfaces
{
    public interface ICustomerMemberRepository : IRepository<CustomerMember>
    {
        Task<CustomerMember> GetCustomerMemberById(Guid customerMemberId);
    }
}
