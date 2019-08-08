using System;
using System.Collections.Generic;
using System.Text;

namespace Owlvey.Falcon.Authority.Infra.Settings
{
    public class AuthenticationSettings
    {
        public string Authority { get; set; }
        public string ApiName { get; set; }
        public string NameClaimType { get; set; }
        public string RoleClaimType { get; set; }
    }
}
