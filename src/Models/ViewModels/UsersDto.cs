using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IdentityServer4.Models.ViewModels
{
    public class UsersDto : IdentityUser
    {
        public UsersDto(Users users,List<Roles> roles)
        {
            Username = users.Username;
            Name = users.Name;
            GivenName = users.GivenName;
            FamilyName = users.FamilyName;
            Email = users.Email;
            EmailVerified = users.EmailVerified;
            WebSite = users.WebSite;
            Address = users.Address;
            PhoneNumber = users.PhoneNumber;
            if (roles != null)
            {
                Roles = roles.Select(o => new UserRolesDto
                {
                    RoleId = o.Id.ToString(),
                    RoleName = o.RoleName
                }).ToList();
            }
        }
        public UsersDto(Users users, List<Roles> roles,bool isDetail)
        {
            Username = users.Username;
            Password = users.Password;
            Name = users.Name;
            GivenName = users.GivenName;
            FamilyName = users.FamilyName;
            Email = users.Email;
            EmailVerified = users.EmailVerified;
            WebSite = users.WebSite;
            Address = users.Address;
            PhoneNumber = users.PhoneNumber;
            if (roles != null)
            {
                Roles = roles.Select(o => new UserRolesDto
                {
                    RoleId = o.Id.ToString(),
                    RoleName = o.RoleName
                }).ToList();
            }
        }
        public UsersDto(UsersForm users)
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
        public string PhoneNumber { get; set; }
        public List<UserRolesDto> Roles { get; set; }
    }
}
