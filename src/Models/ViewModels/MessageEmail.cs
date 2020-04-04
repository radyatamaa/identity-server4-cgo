using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityServer4.Models.ViewModels
{
   public class MessageEmail
    {
        public string Subject { get; set; }
        public string Message { get; set; }
        public string From{ get; set; }
        public string To { get; set; }
    }
}
