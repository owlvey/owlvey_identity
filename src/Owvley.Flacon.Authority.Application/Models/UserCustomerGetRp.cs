using System;
using System.Collections.Generic;
using System.Text;

namespace Owvley.Flacon.Application.Models
{
    public class UserCustomerListRp
    {
        public List<UserCustomerListItemRp> Items { get; set; }
    }
    
    public class UserCustomerListItemRp
    {
        public Guid CustomerId { get; set; }
        public string Name { get; set; }
        public string AdminName { get; set; }
        public string AdminEmail { get; set; }
    }
}
