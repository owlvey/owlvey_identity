using Owlvey.Falcon.Authority.Infra.Data.SqlServer.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using Owvley.Falcon.Authority.Domain.Models;
using TheRoostt.Authority.Domain.Interfaces;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Owlvey.Falcon.Authority.Infra.Data.SqlServer.Repositories
{
    public class UserSqlServerRepository : Repository<User>, IUserRepository
    {
        readonly FalconAuthDbContext _context;
        readonly IConfiguration _configuration;
        public UserSqlServerRepository(FalconAuthDbContext context,
                                       IConfiguration configuration) : base(context)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<User> GetUserById(string userId)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id.Equals(userId, StringComparison.InvariantCultureIgnoreCase));
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase));
        }

        public async Task<List<Customer>> GetUserCustomersByQuery(string userId, string customerName)
        {
            List<Customer> userCustomerList = new List<Customer>();

            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                sqlConnection.Open();
                using (SqlCommand sqlCommand = new SqlCommand("[dbo].[usp_UserCustomer_GetByQuery]", sqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@pUserId", userId);
                    sqlCommand.Parameters.AddWithValue("@pCustomerName", customerName);
                    
                    using (var reader = await sqlCommand.ExecuteReaderAsync())
                    {
                        Customer customer = null;
                        while (await reader.ReadAsync())
                        {
                            customer = new Customer();
                            customer.CustomerId = reader.GetGuid(0);
                            customer.Name = reader.GetString(1);
                            customer.AdminName = reader.GetString(2);
                            customer.AdminEmail = reader.GetString(3);
                            userCustomerList.Add(customer);
                        }
                    }
                }
            }

            return userCustomerList;
        }

    }
}
