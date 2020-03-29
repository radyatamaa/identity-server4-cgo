using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityServer4.Models.ViewModels
{
    public class UsersDto
    {
        public UsersDto(Users users)
        {
            Username = users.Username;
            Name = users.Name;
            GivenName = users.GivenName;
            FamilyName = users.FamilyName;
            Email = users.Email;
            EmailVerified = users.EmailVerified;
            WebSite = users.WebSite;
            Address = users.Address;
        }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string Email { get; set; }
        public bool EmailVerified { get; set; }
        public string WebSite { get; set; }
        public string Address { get; set; }
    }
}
