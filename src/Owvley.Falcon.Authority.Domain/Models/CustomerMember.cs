using Owvley.Falcon.Authority.Domain.Core.Validators.ValidatorManagers;
using Owvley.Falcon.Authority.Domain.Models;
using Owvley.Falcon.Authority.Domain.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace TheRoostt.Authority.Domain.Models
{
    public class CustomerMember : BaseEntity
    {
        [Required]
        public Guid CustomerMemberId { get; set; }

        [Required]
        public Guid CustomerId { get; set; }

        public virtual Customer Customer { get; set; }

        public string UserId { get; set; }

        public virtual User User { get; set; }

        [Required]
        public string Email { get; set; }

        public string Phone { get; set; }

        [Required]
        public CustomerMemberRole Role { get; set; }

        public static class Factory
        {
            public static CustomerMember Create(Guid teamMemberId, string userId, string email, CustomerMemberRole role, string createdBy)
            {
                var entity = new CustomerMember()
                {
                    CustomerMemberId = teamMemberId,
                    UserId = userId,
                    Email = email,
                    Role = role,
                    CreatedBy = createdBy
                };

                var validationResult = new DataValidatorManager<CustomerMember>().Build().Validate(entity);
                if (!validationResult.IsValid)
                    throw new ApplicationException(validationResult.Errors);

                return entity;
            }
        }
    }
}
