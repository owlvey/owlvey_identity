using System;
using System.Collections.Generic;
using System.Text;

namespace Owvley.Flacon.Application.Models
{
    public class CustomerGetRp
    {
        public Guid TeamId { get; set; }
        public string Name { get; set; }
        public string AdminName { get; set; }
        public string AdminEmail { get; set; }
    }
}
