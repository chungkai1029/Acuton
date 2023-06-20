namespace Acuton.Models
{
    public class MailServer
    {
        public string account { get; set; }
        public string password { get; set; }
        public string mailTo { get; set; }
        public string smtpHost { get; set; }
        public int smtpPort { get; set; }
        public string subject { get; set; }
        public ContactUs contactUs { get; set; }
    }
}
