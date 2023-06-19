using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Acuton.Models;

namespace Acuton.Controllers
{
    [Route("/")]
    [ApiController]
    public class ContactUsController : ControllerBase
    {
        MailServer? mailServer;
        private string json;
        private string configFile = "./Configs/mail_server.json";

        [HttpPost("contact_us")]
        public async Task<IActionResult> contact_us([FromForm] ContactUs contactUs)
        {
            Console.WriteLine("Recieve from form submit");

            if(System.IO.File.Exists(configFile))
            {
                using (FileStream fs = System.IO.File.OpenRead(configFile))
                {
                    // Deserialize json from a mail_server.json
                    mailServer = await JsonSerializer.DeserializeAsync<MailServer>(fs);

                    // Change the json with data from form submit
                    mailServer.subject = "Customer Service";
                    mailServer.content = $"公司:{contactUs.company}<br>姓名:{contactUs.name}<br>電子信箱:{contactUs.email}<br>聯絡電話:{contactUs.phone}<br>內容:{contactUs.content}";

                    // Serialize new json string from form submit
                    JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
                    json = JsonSerializer.Serialize(mailServer, options);
                    Console.WriteLine(json);

                    fs.Close();
                    fs.Dispose();
                }

                // Write new json into mail_server.json
                using (StreamWriter writer = new StreamWriter(configFile))
                {
                    writer.Write(json);
                }
            }

            return Ok();
        }
    }
}
