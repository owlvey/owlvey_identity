using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Owvley.Flacon.Application.Models;

namespace Owvley.Flacon.Application.Services.Interfaces
{
    public interface ICustomerMemberService
    {
        Task CreateCustomerMember(CustomerMemberPostRp resource);
    }
}
