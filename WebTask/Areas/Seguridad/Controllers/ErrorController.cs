using Microsoft.AspNetCore.Mvc;

namespace WebTask.Web.Areas.Seguridad.Controllers
{
    public class ErrorController : Controller
    {
        [HttpGet]
        public ActionResult Denied()
        {
            return View();
        }
    }
}
