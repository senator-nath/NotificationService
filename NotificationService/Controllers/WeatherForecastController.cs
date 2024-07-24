using Microsoft.AspNetCore.Mvc;
using NotificationService.Application.Contract;
using NotificationService.Domain.Entity;
using NotificationService.Domain.Enums;

namespace NotificationService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly IMongoDbLogRepository _logRepository;

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IMongoDbLogRepository logRepository)
        {
            _logger = logger;
            _logRepository = logRepository;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            var result = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();

            var log = new NotificationActivity
            {
                SentTo= "ieosahon@gmail.com",
                HasAttachment = false,
                NotificationType = NotificationType.Email,
                Purpose = "Testing",
                Status = true
            };
            await _logRepository.CreateLog(log);
            return result;
        }
    }
}
