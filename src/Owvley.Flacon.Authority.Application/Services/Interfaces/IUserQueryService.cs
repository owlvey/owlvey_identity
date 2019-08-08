using Owvley.Flacon.Application.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Owvley.Flacon.Application.Services.Interfaces
{
    public interface IUserQueryService
    {
        Task<UserGetRp> GetUserById(string userId);
        Task<UserCustomerListRp> GetUserCustomers(string userId, string customerName);
    }
}
