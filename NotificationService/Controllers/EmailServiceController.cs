using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Application.Contract.IEmailContract;
using NotificationService.Domain.Entity;

namespace NotificationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailServiceController : ControllerBase
    {
        private readonly IEmailService _emailService;
        public EmailServiceController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost]
        public IActionResult SendNotiication(EmailRequest request)
        {
            _emailService.SendEmail(request);
            return Ok();
        }
    }
}
