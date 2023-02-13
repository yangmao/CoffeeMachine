namespace CoffeeMachine.Utils
{
    public interface IWeatherService
    {
        public Task<double> GetCurrentTemperature(IHttpClientFactory httpClientFactory, IConfiguration configuration);
    }
}
