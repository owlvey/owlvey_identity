using System;
using System.Collections.Generic;
using System.Text;

namespace Owvley.Flacon.Application.Models
{
    public class UserPreferenceBulkPostRp
    {
        public List<UserPreferenceBulkItemPostRp> Items { get; set; }
    }

    public class UserPreferenceBulkItemPostRp
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
