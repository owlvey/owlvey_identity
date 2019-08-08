using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Owvley.Flacon.Application.Models
{
    public class UserPostRp
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string GivenName { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "El campo {0} debe tener al menos {2} and {1} como máximo de caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
