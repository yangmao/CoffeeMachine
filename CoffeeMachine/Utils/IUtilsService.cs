namespace CoffeeMachine.Utils
{
    public interface IUtilsService
    {
        string GetToday();
        int? CountRequests(HttpContext context, string RequestCount);
    }
}
