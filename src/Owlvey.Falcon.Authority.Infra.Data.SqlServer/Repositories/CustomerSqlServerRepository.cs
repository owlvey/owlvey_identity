using Owlvey.Falcon.Authority.Infra.Data.SqlServer.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Owvley.Falcon.Authority.Domain.Models;
using TheRoostt.Authority.Domain.Interfaces;

namespace Owlvey.Falcon.Authority.Infra.Data.SqlServer.Repositories
{
    public class CustomerSqlServerRepository : Repository<Customer>, ICustomerRepository
    {
        readonly FalconAuthDbContext _context;
        public CustomerSqlServerRepository(FalconAuthDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Customer> GetCustomerById(Guid customerId)
        {
            return await _context.Customers.FirstOrDefaultAsync(x => x.CustomerId == customerId);
        }

        public async Task<Customer> GetCustomerByName(string name)
        {
            return await _context.Customers.FirstOrDefaultAsync(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
