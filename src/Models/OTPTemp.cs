using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityServer4.Models
{
    public class OTPTemp
    {
        public Guid Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string OTP { get; set; }
        public DateTime Expired { get; set; }
    }
}
