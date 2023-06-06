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
        [HttpPost("contact_us")]
        public async Task<IActionResult> contact_us([FromForm] ContactUs contactUs)
        {
            Console.WriteLine("Recieve from form submit");

            // Console what recieve from form submit
            Console.WriteLine($"Company={contactUs.company}");
            Console.WriteLine($"Name={contactUs.name}");
            Console.WriteLine($"Email={contactUs.email}");
            Console.WriteLine($"Phone={contactUs.phone}");
            Console.WriteLine($"Content={contactUs.content}");

            return Ok();
        }
    }
}
