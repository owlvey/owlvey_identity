using Owlvey.Falcon.Authority.Infra.Data.SqlServer.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using TheRoostt.Authority.Domain.Models;
using Owvley.Falcon.Authority.Domain.Interfaces;

namespace Owlvey.Falcon.Authority.Infra.Data.SqlServer.Repositories
{
    public class CustomerMemberSqlServerRepository : Repository<CustomerMember>, ICustomerMemberRepository
    {
        readonly FalconAuthDbContext _context;
        public CustomerMemberSqlServerRepository(FalconAuthDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<CustomerMember> GetCustomerMemberById(Guid customerMemberId)
        {
            return await _context.Members.FirstOrDefaultAsync(x => x.CustomerMemberId == customerMemberId);
        }
    }
}
