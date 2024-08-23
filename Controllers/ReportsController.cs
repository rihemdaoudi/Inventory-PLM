using Microsoft.AspNetCore.Mvc;

namespace Inventory_PLM.Controllers
{
    public class ReportsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
