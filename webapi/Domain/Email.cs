using System.Collections.Generic;

namespace webapi.Domain
{
    public class Email
    {
        public int Id { get; set; }
        public string Body { get; set; } 
        public string Subject { get; set; } 
        public string Recipients { get; set; } 
        public string Sender { get; set; } 
        public int Priority { get; set; }
        public EmailStatus Status { get; set; }
        public ICollection<Attachment> Attachments { get; set; }
        public Email()
        {
            
        }

       
    }
}