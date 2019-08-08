using Owvley.Falcon.Authority.Domain.Interfaces;
using Owvley.Falcon.Authority.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TheRoostt.Authority.Domain.Models;

namespace TheRoostt.Authority.Domain.Interfaces
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<Customer> GetCustomerById(Guid customerId);
        Task<Customer> GetCustomerByName(string name);
    }
}
