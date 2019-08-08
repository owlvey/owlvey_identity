using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Owvley.Falcon.Authority.Domain.Core.Validators.ValidatorManagers;
using Owvley.Falcon.Authority.Domain.Models.Enums;
using TheRoostt.Authority.Domain.Models;

namespace Owvley.Falcon.Authority.Domain.Models
{
    public class Customer : BaseEntity
    {
        public Guid CustomerId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string AdminName { get; set; }
        
        [Required]
        [EmailAddress]
        public string AdminEmail { get; set; }
        
        public virtual List<CustomerMember> Members { get; set; }

        public CustomerMember GetTeamMemberById(string userId)
        {
            if (this.Members == null)
                this.Members = new List<CustomerMember>();

            return this.Members.FirstOrDefault(x => !string.IsNullOrEmpty(x.UserId) && x.UserId.Equals(userId, StringComparison.InvariantCultureIgnoreCase));
        }

        public CustomerMember AddMember(Guid teamMemberId, string userId, string email, CustomerMemberRole role, string createdBy, bool activate = false)
        {
            if (this.Members == null)
                this.Members = new List<CustomerMember>();

            var newMember = CustomerMember.Factory.Create(teamMemberId, userId, email, role, createdBy);
            if (activate)
                newMember.Activate();

            this.Members.Add(newMember);

            return newMember;
        }

        public CustomerMember GetMemberByEmail(string email)
        {
            if (Members == null)
                Members = new List<CustomerMember>();

            return Members.FirstOrDefault(x => x.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase));
        }

        public static class Factory
        {
            public static Customer Create(Guid customerId, string name, string userId, string adminName, string adminEmail, string createdBy)
            {
                var entity = new Customer()
                {
                    CustomerId = customerId,
                    Name = name,
                    AdminName = adminName,
                    AdminEmail = adminEmail,
                    CreatedBy = createdBy
                };

                var validationResult = new DataValidatorManager<Customer>().Build().Validate(entity);
                if (!validationResult.IsValid)
                    throw new ApplicationException(validationResult.Errors);

                entity.AddMember(Guid.NewGuid(), userId, adminEmail, CustomerMemberRole.Admin, createdBy, true);
                
                return entity;
            }
        }
    }
}
