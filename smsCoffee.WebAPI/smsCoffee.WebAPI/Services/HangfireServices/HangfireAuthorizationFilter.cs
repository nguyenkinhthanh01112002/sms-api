using Hangfire.Dashboard;

namespace smsCoffee.WebAPI.Services.HangfireServices
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();
            return httpContext.User.IsInRole("Admin");
        }
    }
}
