using Microsoft.AspNetCore.Identity;
using Owvley.Falcon.Authority.Domain.Core.Validators.ValidatorManagers;
using Owvley.Falcon.Authority.Domain.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TheRoostt.Authority.Domain.Models;

namespace Owvley.Falcon.Authority.Domain.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Url]
        public string Avatar { get; set; }

        public virtual List<CustomerMember> Customers { get; set; }

        public virtual List<UserPreference> Preferences { get; set; }

        public virtual List<UserActivity> Activities { get; set; }

        public CustomerMember GetLastAssignedCustomer()
        {
            if (Customers == null)
                Customers = new List<CustomerMember>();

            return Customers.Where(x => x.Status == EntityStatus.Active).OrderByDescending(x => x.CreationDate).FirstOrDefault();
        }

        public List<UserActivity> GetUserActivities()
        {
            if (Activities == null)
                Activities = new List<UserActivity>();

            return Activities.ToList();
        }

        public UserActivity GetLastAccessAccountActivity()
        {
            if (Activities == null)
                Activities = new List<UserActivity>();

            return Activities.Where(x => x.Type == Enums.ActivityType.SignInCustomer ||
                                         x.Type == Enums.ActivityType.SwitchCustomer)
                             .OrderByDescending(x => x.Date)
                             .FirstOrDefault();
        }

        public void AddActivity(ActivityType type, string data)
        {
            if (Activities == null)
                Activities = new List<UserActivity>();

            var activity = UserActivity.Factory.Create(type, data, this.Id, this.Id);
            Activities.Add(activity);
        }

        public void AddPreference(string name, string value)
        {
            if (Preferences == null)
                Preferences = new List<UserPreference>();

            var preference = UserPreference.Factory.Create(name, value, this.Id, this.Id);
            Preferences.Add(preference);
        }

        public string GetPreferenceValue(string name)
        {
            if (Preferences == null)
                Preferences = new List<UserPreference>();

            return Preferences.First(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)).Value;
        }

        public UserPreference GetPreference(string name)
        {
            if (Preferences == null)
                Preferences = new List<UserPreference>();

            return Preferences.First(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }

        public void UpdatePreference(string name, string value)
        {
            if (Preferences == null)
                Preferences = new List<UserPreference>();

            var preference = Preferences.First(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
            if (preference == null)
                throw new NullReferenceException($"The preference with name {name} does not exists.");

            preference.SetValue(value);
        }

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
