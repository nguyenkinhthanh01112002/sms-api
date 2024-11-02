using Microsoft.AspNetCore.Authorization;
using smsCoffee.WebAPI.DTOs.Common;

namespace smsCoffee.WebAPI.Services.CommonService
{
    public class CustomAuthorizationHandler : IAuthorizationHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CustomAuthorizationHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task HandleAsync(AuthorizationHandlerContext context)
        {
            if (context.HasFailed)
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext != null)
                {
                    var response = new ApiResponse<object>
                    {
                        Status = false,
                        Path = httpContext.Request.Path,
                        Message = "Access denied",
                        StatusCode = StatusCodes.Status403Forbidden,
                        Timestamp = DateTime.UtcNow
                    };

                    httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                    httpContext.Response.ContentType = "application/json";
                    await httpContext.Response.WriteAsJsonAsync(response);
                }
            }
        }
    }
}
