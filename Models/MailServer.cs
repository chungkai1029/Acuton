namespace Acuton.Models
{
    public class MailServer
    {
        public string account { get; }
        public string password { get; }
        public string mailTo { get; }
        public string smtpHost { get; }
        public string smtpPort { get; }
        public string subject { get; set; }
        public string content { get; set; }
    }
}
