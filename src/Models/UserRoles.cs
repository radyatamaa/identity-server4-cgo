using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityServer4.Models
{
   public class UserRoles
    {
        public Guid Id { get; set; }
        public virtual Users Users { get; set; }
        public Guid UserId { get; set; }
        public virtual Roles Roles { get; set; }
        public Guid RoleId { get; set; }
    }
}
