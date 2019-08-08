using Owvley.Falcon.Authority.Domain.Models.Enums;
using System.Collections.Generic;

namespace Owvley.Flacon.Application.Models
{
    public class UserActivityListRp
    {
        public UserActivityListRp()
        {
            this.Items = new List<UserActivityListItemRp>();
        }

        public IReadOnlyList<UserActivityListItemRp> Items { get; set; }
    }

    public class UserActivityListItemRp
    {
        public ActivityType Type { get; set; }
        public string Data { get; set; }
    }

}
