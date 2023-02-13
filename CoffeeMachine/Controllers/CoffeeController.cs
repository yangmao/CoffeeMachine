using CoffeeMachine.Models;
using CoffeeMachine.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CoffeeMachine.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CoffeeController : ControllerBase
    {
        private readonly static string RequestCount = "REQUEST_COUNT";
        private int? _count;
        private readonly IWeatherService _weatherService;
        private readonly IUtilsService _utilsService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CoffeeController> _logger;

        public CoffeeController(IWeatherService weatherService,IUtilsService utilsService, IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<CoffeeController> logger)
        {
            _weatherService = weatherService;
            _utilsService = utilsService;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet]
        [Route("brew-coffee")]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                _count = _utilsService.CountRequests(HttpContext, RequestCount);
                if (_utilsService.GetToday() == Constants.FoolsDay)
                {
                    return new ObjectResult(new EmptyResult())
                    {
                        StatusCode = 418,
                    };
                }

                if (_count % 5 != 0)
                {
                    var weather = await _weatherService.GetCurrentTemperature(_httpClientFactory, _configuration);
                    var result = new CoffeeResult()
                    {
                        message = weather >= 30 ? "Your refreshing iced coffee is ready" : "Your piping hot coffee is ready",
                        prepared = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzzz")
                    };
                    return Ok(result);
                }
                else
                {
                    return new ObjectResult(new EmptyResult())
                    {
                        StatusCode = (int)HttpStatusCode.ServiceUnavailable
                    };
                }
            }
            catch (Exception e)
            {
                if (e is WeatherServiceException)
                {
                    _logger.Log(LogLevel.Error, Constants.WeatherServiceExceptionMessage);
                }
                return StatusCode(500);
            }
        }
      
    }
}