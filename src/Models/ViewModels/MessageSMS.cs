using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityServer4.Models.ViewModels
{
    public class MessageSMS
    {
        public string Source { get; set; }
        public string Destination { get; set; }
        public string Text { get; set; }
        public string Encoding { get; set; }
    }
}
