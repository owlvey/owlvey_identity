using System;
using System.Collections.Generic;
using System.Text;

namespace Owvley.Flacon.Application.Models
{
    public class UserListRp
    {
        public UserListRp()
        {
            this.Items = new List<UserListItemRp>();
        }

        public IReadOnlyList<UserListItemRp> Items { get; set; }

        public int TotalItems { get; set; }
    }

    public class UserListItemRp
    {
        public Guid UserId { get; set; }
        public string GivenName { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public DateTime CreationDate { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
    }

    public class UserGetRp
    {
        public Guid UserId { get; set; }
        public string GivenName { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public DateTime CreationDate { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
    }
}
