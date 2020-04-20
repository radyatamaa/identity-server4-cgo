using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityServer4.Models
{
    public class Roles : EntitySoftDelete
    {
        public Roles(Guid id) : base(id)
        {

        }
        public string RoleName { get; set; }
        public string Description { get; set; }
        public int? RoleType { get; set; }
    }
}
