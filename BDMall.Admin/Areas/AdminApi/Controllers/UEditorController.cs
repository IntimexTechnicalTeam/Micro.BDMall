using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UEditorNetCore;

namespace BDMall.Admin.Areas.AdminApi.Controllers
{
    [Area("AdminApi")]
    [Route("AdminApi/[controller]")]
    //[ApiController]
    public class UEditorController : Controller
    {
        private UEditorService ue;
        public UEditorController(UEditorService ue)
        {
            this.ue = ue;
        }

        public void Upload()
        {
            ue.DoAction(HttpContext);
        }
    }
}
