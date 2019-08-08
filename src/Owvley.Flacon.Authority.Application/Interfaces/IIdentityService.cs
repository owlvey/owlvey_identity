using System;
using System.Collections.Generic;
using System.Text;

namespace Owvley.Flacon.Authority.Application.Interfaces
{
    public interface IIdentityService
    {
        string GetClientId();
        string GetUserId();
        string GetUserName();
        string GetUserEmail();
    }
}
