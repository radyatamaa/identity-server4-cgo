using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityServer4.Models
{
    public class PermissionRoleMapping
    {

        public Guid Id { get; set; }
        public virtual PermissionRecord Permission { get; set; }
        public int PermissionId { get; set; }
        public virtual Roles Roles { get; set; }
        public Guid RoleId { get; set; }
    }
}
