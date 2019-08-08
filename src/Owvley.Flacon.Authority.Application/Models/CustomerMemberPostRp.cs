using System;
using System.ComponentModel.DataAnnotations;
using Owvley.Falcon.Authority.Domain.Models.Enums;

namespace Owvley.Flacon.Application.Models
{
    public class CustomerMemberPostRp
    {
        public Guid CustomerMemberId { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public CustomerMemberRole Role { get; set; }
        
        public Guid CustomerId { get; set; }
    }

}
