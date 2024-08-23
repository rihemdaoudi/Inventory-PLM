using Microsoft.AspNetCore.Mvc;

namespace Inventory_PLM.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
