using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Acuton.Models;
using System.Net.Mail;
using System.Net;
using System.Text;

namespace Acuton.Controllers
{
    [Route("/")]
    [ApiController]
    public class ContactUsController : ControllerBase
    {
        MailServer? mailServer;
        private string json;
        private string configFile = "./Configs/mail_server.json";
        private bool isApiWriteComplete = false;

        [HttpPost("contact_us")]
        public async Task<IActionResult> contact_us([FromForm] ContactUs contactUs)
        {
            Console.WriteLine("Recieve from form submit");


            if (System.IO.File.Exists(configFile))
            {
                try
                {
                    using (FileStream fs = System.IO.File.OpenRead(configFile))
                    {
                        // Deserialize json from a mail_server.json to overwrite.
                        mailServer = await JsonSerializer.DeserializeAsync<MailServer>(fs);

                        // Change the json with data from form submit.
                        mailServer.subject = "Customer Service";
                        mailServer.contactUs.company = contactUs.company;
                        mailServer.contactUs.name = contactUs.name;
                        mailServer.contactUs.email = contactUs.email;
                        mailServer.contactUs.phone = contactUs.phone;
                        mailServer.contactUs.content = contactUs.content;

                        // Serialize new json string from form submit.
                        JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
                        json = JsonSerializer.Serialize(mailServer, options);
                        Console.WriteLine(json);

                        fs.Close();
                        fs.Dispose();
                    }

                    // Write new json into mail_server.json.
                    using (StreamWriter writer = new StreamWriter(configFile))
                    {
                        writer.Write(json);
                        isApiWriteComplete = true;
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Recieve from HTML form exception:{ex}");
                }

                if (isApiWriteComplete == true)
                {
                    SendMail();
                    isApiWriteComplete = false;
                }
            }

            return Ok();
        }

        internal async void SendMail()
        {
            try
            {
                using (FileStream fs = System.IO.File.OpenRead(configFile))
                {
                    // Deserialize json to send customer service from a mail_server.json.
                    mailServer = await JsonSerializer.DeserializeAsync<MailServer>(fs);

                    // Build a SmtpClient to send customer service.
                    SmtpClient smtpClient = new SmtpClient(mailServer.smtpHost, mailServer.smtpPort);
                    Console.WriteLine("SMTP client is built success.");

                    // Get the credential to verify mail sender.
                    smtpClient.Credentials = new NetworkCredential(mailServer.account, mailServer.password);
                    smtpClient.EnableSsl = false;

                    // Build the mail to send.
                    MailMessage mailMessage = new MailMessage();
                    Console.WriteLine("Mail is built success.");

                    mailMessage.From = new MailAddress(mailServer.account, "Acuton Customer Service");  // Set the sender and display on sender block.
                    mailMessage.To.Add(mailServer.mailTo);  // Set the destination for the email message.

                    // Set the content of the customer service mail.
                    mailMessage.Subject = mailServer.subject;   // The title of the mail.
                    mailMessage.SubjectEncoding = Encoding.UTF8;

                    mailMessage.Body = $"公司:{mailServer.contactUs.company}\r\n" +
                        $"姓名:{mailServer.contactUs.name}\r\n" +
                        $"電子信箱:{mailServer.contactUs.email}\r\n" +
                        $"聯絡電話:{mailServer.contactUs.phone} \r\n" +
                        $"訊息內容:{mailServer.contactUs.content}";    // The content of the mail.
                    mailMessage.BodyEncoding = Encoding.UTF8;

                    smtpClient.Send(mailMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Send email exception:{ex}");
            }
        }
    }
}
