using Microsoft.AspNetCore.Mvc;

namespace MagazineManagment.ClientApplication.Controllers
{
    public class CategoryApiCallController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
