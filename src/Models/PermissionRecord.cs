using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityServer4.Models
{
    public class PermissionRecord
    {
        public int Id { get; set; }
        public string ActivityCode { get; set; }
        public string ActivityName { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
    }
}
