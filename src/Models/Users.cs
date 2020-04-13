using IdentityServer4.Models.ViewModels;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.Models
{
    public class Users : EntitySoftDelete
    {
        public Users(Guid id) : base(id)
        {
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
        public string CurrentOTPCode { get; set; }
        public int? UserType { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? ExpiredOTP { get; set; }
    }
}
