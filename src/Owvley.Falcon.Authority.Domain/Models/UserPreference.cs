using Owvley.Falcon.Authority.Domain.Core.Validators.ValidatorManagers;
using System;
using System.ComponentModel.DataAnnotations;

namespace Owvley.Falcon.Authority.Domain.Models
{
    public class UserPreference : BaseEntity
    {
        public Guid UserPreferenceId { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual User User { get; set; }

        public void SetValue(string value)
        {
            this.Value = value;
        }

        public static class Factory
        {
            public static UserPreference Create(string name, string value, string userId, string createdBy)
            {
                var entity = new UserPreference()
                {
                    Name = name,
                    Value = value,
                    Status = EntityStatus.Active,
                    UserId = userId,
                    CreatedBy = createdBy
                };

                var validationResult = new DataValidatorManager<UserPreference>().Build().Validate(entity);
                if (!validationResult.IsValid)
                    throw new ApplicationException(validationResult.Errors);

                return entity;
            }
        }
    }
}
