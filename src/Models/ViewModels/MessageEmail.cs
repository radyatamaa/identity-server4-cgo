using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityServer4.Models.ViewModels
{
   public class MessageEmail
    {
        public string Subject { get; set; }
        public string Message { get; set; }
        public IList<AttachmentFile> Attachment { get; set; }
        public string From{ get; set; }
        public string To { get; set; }
    }

    public class AttachmentFile
    {
        public string AttachmentFileUrl { get; set; }
        public string FileName { get; set; }
    }
}
