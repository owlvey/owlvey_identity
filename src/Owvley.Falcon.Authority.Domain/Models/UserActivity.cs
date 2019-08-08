using Owvley.Falcon.Authority.Domain.Core.Validators.ValidatorManagers;
using Owvley.Falcon.Authority.Domain.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Owvley.Falcon.Authority.Domain.Models
{
    public class UserActivity : BaseEntity
    {
        public Guid UserActivityId { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual User User { get; set; }

        [Required]
        public ActivityType Type { get; set; }

        [Required]
        public string Data { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public static class Factory
        {
            public static UserActivity Create(ActivityType type, string data, string userId, string createdBy)
            {
                var entity = new UserActivity()
                {
                    Type = type,
                    Data = data,
                    Status = EntityStatus.Active,
                    Date = DateTime.UtcNow,
                    UserId = userId,
                    CreatedBy = createdBy
                };

                var validationResult = new DataValidatorManager<UserActivity>().Build().Validate(entity);
                if (!validationResult.IsValid)
                    throw new ApplicationException(validationResult.Errors);

                return entity;
            }
        }
    }
}
