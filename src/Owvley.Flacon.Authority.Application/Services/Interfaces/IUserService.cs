using Owvley.Flacon.Application.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Owvley.Flacon.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task CreateUser(UserPostRp resource);
        Task PatchUser(string userId, UserPatchRp resource);
    }
}
