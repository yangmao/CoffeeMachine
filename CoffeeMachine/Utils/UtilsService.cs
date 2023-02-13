using System.Diagnostics.CodeAnalysis;

namespace CoffeeMachine.Utils
{
    [ExcludeFromCodeCoverage]
    public class UtilsService : IUtilsService
    {
        public string GetToday()
        {
            return DateTime.Now.Date.ToString("MM-dd");
        }

        public int? CountRequests(HttpContext context, string RequestCount)
        {
            int? count = context.Session.GetInt32(RequestCount);
            if (count == null)
            {
                count = 1;
            }
            else
            {
                count++;
            }
            context.Session.SetInt32(RequestCount, count.Value);
            return count;
        }
    }
}
