using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityServer4.Models.ViewModels
{
    public class RolesDto
    {
        public RolesDto(Roles roles,List<PermissionRecord> permission)
        {
            if (roles != null)
            {
                this.Id = roles.Id.ToString();
                this.RoleName = roles.RoleName;
                this.RoleType = roles.RoleType;
                this.Description = roles.Description;
                this.Permissions = permission;
            }
        }
        public string Id { get; set; }
        public string RoleName { get; set; }
        public int? RoleType { get; set; }
        public string Description { get; set; }
        public List<PermissionRecord> Permissions { get; set; }
    }
}
