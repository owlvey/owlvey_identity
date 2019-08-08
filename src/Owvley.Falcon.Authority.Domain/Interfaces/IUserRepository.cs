using Owvley.Falcon.Authority.Domain.Interfaces;
using Owvley.Falcon.Authority.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TheRoostt.Authority.Domain.Models;

namespace TheRoostt.Authority.Domain.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetUserById(string userId);
        Task<User> GetUserByEmail(string email);
        Task<List<Customer>> GetUserCustomersByQuery(string userId, string customerName);
    }
}
