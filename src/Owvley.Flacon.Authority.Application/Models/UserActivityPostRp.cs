using Owvley.Falcon.Authority.Domain.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Owvley.Flacon.Application.Models
{
    public class UserActivityPostRp
    {
        [Required]
        public ActivityType Type { get; set; }
        [Required]
        public string Data { get; set; }
    }
}
