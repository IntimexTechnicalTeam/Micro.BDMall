using Microsoft.AspNetCore.Mvc.Filters;

namespace BDMall.MiniApi.Filters
{
    public class UserAuthorizeAttribute : IActionFilter
    {
        public UserAuthorizeAttribute()
        {

        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            throw new NotImplementedException();
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            throw new NotImplementedException();
        }
    }
}
