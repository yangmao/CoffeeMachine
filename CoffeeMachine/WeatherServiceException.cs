namespace CoffeeMachine
{
    public class WeatherServiceException:Exception
    {
        public WeatherServiceException()
        {
        }
        public WeatherServiceException(string message)
            : base(message)
        {
        }
    }
}
