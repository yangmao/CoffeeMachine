namespace CoffeeMachine.Utils
{
    public interface IUtilsService
    {
        string GetTodayDate();
        string GetTodayLocalDateTime();
        int? CountRequests(HttpContext context, string RequestCount);
    }
}
