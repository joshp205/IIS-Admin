using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IISAdministration.Controllers
{
    /// <summary>
    /// Controller for the homepage of the application.
    /// </summary>
    [AllowAnonymous]
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            return View("Index");
        }
    }
}
