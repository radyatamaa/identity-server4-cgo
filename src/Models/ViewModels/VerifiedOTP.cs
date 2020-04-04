using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityServer4.Models.ViewModels
{
    public class VerifiedOTP
    {
        public string Email { get; set; }
        public string CodeOTP { get; set; }
    }
}
