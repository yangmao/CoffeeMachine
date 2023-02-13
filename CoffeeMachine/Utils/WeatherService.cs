using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace CoffeeMachine.Utils
{
    [ExcludeFromCodeCoverage]
    public class WeatherService: IWeatherService
    {
        public async Task<double> GetCurrentTemperature(IHttpClientFactory httpClientFactory,IConfiguration configuration, ILogger logger)
        {
            var url =configuration["WeatherApi"] + configuration["AppId"];
            var httpClient = httpClientFactory.CreateClient();
            var httpResponseMessage = await httpClient.GetAsync(url);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var jsonContent = httpResponseMessage.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(jsonContent);
                var main = data.Where(x=>x.Key =="main").FirstOrDefault().Value;
                var maindata = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(Convert.ToString(main));
                var temp = (double)maindata["temp"] - Constants.Kelvin;// to Celsius
                return Math.Round(temp,1);
            }
            else
            {
                logger.LogError("Weather Service Unavailible.");
                return Constants.WeatherServiceUnavailible;
            }
        }
}
}
