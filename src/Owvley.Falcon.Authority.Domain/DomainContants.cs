using System;
using System.Collections.Generic;
using System.Text;

namespace TheRoostt.Authority.Domain
{
    public class DomainConstants
    {
        public static class Preferences
        {
            public static class Theme
            {
                public const string Name = "Theme";
                public const string ClaimName = "up_theme";
                public const string DefaultValue = "Standard";
            }

            public static class TimeZone
            {
                public const string Name = "TimeZone";
                public const string ClaimName = "up_timezone";
                public const string DefaultValue = "Eastern Standard Time";
            }

            public static class DateFormat
            {
                public const string Name = "DateFormat";
                public const string ClaimName = "up_dateformat";
                public const string DefaultValue = "dd/MM/yyyy";
            }

            public static class TimeFormat
            {
                public const string Name = "TimeFormat";
                public const string ClaimName = "up_timeformat";
                public const string DefaultValue = "hh:mm tt";
            }

            public static class Customer
            {
                public const string Name = "Customer";
                public const string ClaimName = "up_customer";
                public const string DefaultValue = "lastsignin";
            }

        }
        
        public static class Roles
        {
            public const string Admin = "admin";
            public const string BasicUser = "basicuser";
        }
    }
}
