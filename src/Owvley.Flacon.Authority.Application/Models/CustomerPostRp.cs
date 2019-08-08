using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Owvley.Flacon.Application.Models
{
    public class CustomerPostRp
    {
        [Required]
        public Guid CustomerId { get; set; }
        
        [Required]
        public string Name { get; set; }

        [Required]
        public string AdminId { get; set; }
        
        [Required]
        public string AdminName { get; set; }
        
        [Required]
        [EmailAddress]
        public string AdminEmail { get; set; }
        
    }
}
