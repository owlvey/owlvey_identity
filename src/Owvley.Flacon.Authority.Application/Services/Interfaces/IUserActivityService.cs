using Owvley.Flacon.Application.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Owvley.Flacon.Application.Services.Interfaces
{
    public interface IUserActivityService
    {
        Task CreateActivity(string userId, UserActivityPostRp resource);
    }
}
