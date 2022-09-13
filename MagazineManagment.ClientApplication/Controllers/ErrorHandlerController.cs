using Microsoft.AspNetCore.Mvc;

namespace MagazineManagment.ClientApplication.Controllers
{
    public class ErrorHandlerController : Controller
    {
        [Route("Error/{statusCode}")]
        public IActionResult Index(int statusCode)
        {
            return statusCode switch
            {
                404 => View("NotFound", statusCode),
                400 => View("BadRequest", statusCode),
                _ => NoContent()
            };
        }
    }
}