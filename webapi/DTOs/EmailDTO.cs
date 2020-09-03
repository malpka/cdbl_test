using System;
using webapi.Domain;

namespace webapi.DTOs
{
    public class EmailDTO
    {
        public int Id { get; set; }
        public string Description { get; set; } 
        public string Recipients { get; set; } 
        public string Sender { get; set; } 
        public int Priority { get; set; }
        public EmailStatus Status { get; set; }

    }
}