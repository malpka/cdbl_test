namespace webapi
{
    public class AppSettings
    {
        public string SmtpHost { get; set; }
        public int? SmtpPort { get; set; }
        public int? AttachmentMaxLength { get; set; }
    }
}