using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Owvley.Falcon.Authority.Domain.Models
{
    public abstract class BaseEntity
    {
        public BaseEntity()
        {
            CreationDate = DateTime.UtcNow;
            Status = EntityStatus.New;
        }

        [Required]
        public DateTime CreationDate { get; set; }

        [Required]
        [StringLength(450)]
        public string CreatedBy { get; set; }

        public DateTime? LastModificationDate { get; set; }

        [StringLength(450)]
        public string LastModifiedBy { get; set; }

        public DateTime? LastDeactivationDate { get; set; }

        [StringLength(450)]
        public string LastDeactivatedBy { get; set; }

        public DateTime? DeletionDate { get; set; }

        [StringLength(450)]
        public string DeletedBy { get; set; }

        [Required]
        public EntityStatus Status { get; set; }

        public void Activate()
        {
            if (this.Status != EntityStatus.New)
            {
                throw new ApplicationException("The entity only can be activated when previous status is new");
            }

            this.Status = EntityStatus.Active;
        }

        public void Reactivate()
        {
            if (this.Status != EntityStatus.Inactive)
            {
                throw new ApplicationException("The entity only can be reactivated when previous status is inactive");
            }

            this.Status = EntityStatus.Active;
        }

        public void Deactivate(string userId)
        {
            if (this.Status != EntityStatus.Active)
            {
                throw new ApplicationException("The entity only can be deactivated when previous status is active");
            }

            this.LastDeactivatedBy = userId;
            this.LastDeactivationDate = DateTime.UtcNow;
            this.Status = EntityStatus.Inactive;
        }

        public void Delete(string userId)
        {
            if (this.Status == EntityStatus.Active || this.Status == EntityStatus.Inactive)
            {
                throw new ApplicationException("The entity only can be deleted when previous status is active/inactive");
            }

            this.DeletedBy = userId;
            this.DeletionDate = DateTime.UtcNow;
            this.Status = EntityStatus.Deleted;
        }
    }

    public enum EntityStatus
    {
        New = 0,
        Active = 1,
        Inactive = 2,
        Deleted = 3
    }
}
