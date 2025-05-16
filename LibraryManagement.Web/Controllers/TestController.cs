using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Web.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Message = "If you can see this message, ASP.NET Core is working!";
            return View();
        }
    }
}