using System;
using System.Collections.Generic;
using System.Text;

namespace Owvley.Falcon.Authority.Domain.Core.Manager.Models
{
    public enum DomainManagerMessageType
    {
        NotFound,
        Conflict,
        Result,
        BadRequest,
        Unauthorized
    }
}
