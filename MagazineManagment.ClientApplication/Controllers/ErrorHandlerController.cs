using Microsoft.AspNetCore.Mvc;

namespace MagazineManagment.ClientApplication.Controllers
{
    public class ErrorHandlerController : Controller
    {
        [Route("Error/{statusCode}")]
        public IActionResult Index(int statusCode)
        {
            switch (statusCode)
            {
                case 404: return View("NotFound", statusCode); break;
                case 400: return View("BadRequest", statusCode); break;
            }
            return NoContent();
        }
    }
}
