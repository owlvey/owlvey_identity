using Microsoft.AspNetCore.Identity;
using Owvley.Falcon.Authority.Domain.Core.Validators.ValidatorManagers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Owvley.Falcon.Authority.Domain.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Url]
        public string Avatar { get; set; }      

        public bool IsEmailConfirmed()
        {
            return this.EmailConfirmed;
        }

        public void ConfirmEmail()
        {
            if (this.EmailConfirmed)
                throw new ApplicationException("The email is already confirmed");

            this.EmailConfirmed = true;
        }

        public static class Factory
        {
            public static User Create(string firstName, string lastName, string email, string password)
            {
                var entity = new User()
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    PasswordHash = password,
                    UserName = email,
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    SecurityStamp = Guid.NewGuid().ToString(),
                    NormalizedEmail = email.ToUpper(),
                    NormalizedUserName = email.ToUpper()
                };

                var validationResult = new DataValidatorManager<User>().Build().Validate(entity);
                if (!validationResult.IsValid)
                    throw new ApplicationException(validationResult.Errors);

                return entity;
            }
        }
    }

}
