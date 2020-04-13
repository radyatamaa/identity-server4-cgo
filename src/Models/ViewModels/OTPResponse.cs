using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityServer4.Models.ViewModels
{
    public class OTPResponse
    {
        public string OTP { get; set; }
        public DateTime ExpiredDate { get; set; }
    }
}
