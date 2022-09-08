using Microsoft.AspNetCore.Authorization;

namespace BDMall.MiniApi.Filters
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var httpContext = context.Resource as DefaultHttpContext;
            var questPath = httpContext?.Request?.Path;
            var method = httpContext?.Request?.Method;
            var isAuthenticated = context?.User?.Identity?.IsAuthenticated;

            var headers = httpContext.Request.Headers;
 

        }
    }


    public class PermissionRequirement : IAuthorizationRequirement
    {
    }
}
