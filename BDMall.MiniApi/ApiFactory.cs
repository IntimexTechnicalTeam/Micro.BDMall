using BDMall.BLL;

namespace BDMall.MiniApi
{
    public static class ApiFactory
    {       
        public static void InitApi(WebApplication app)
        {
            var service = app.Services;
            
            app.MapGet("api/Test/{id}",(HttpContext context,string id) => {

                var testservice = service.Resolve<ITestService>();

                 testservice.Hello(id);
            }).RequireAuthorization("Permission");



        }
    }
}
