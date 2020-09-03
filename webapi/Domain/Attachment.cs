namespace webapi.Domain
{
    public class Attachment
    {
        public int Id { get; set; }
        public string Name { get; set; }       
        public byte[] Content { get; set; }
        public Email Email { get; set; }
        public Attachment()
        {
            
        }

       
    }
}