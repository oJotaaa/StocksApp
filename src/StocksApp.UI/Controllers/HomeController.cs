using Microsoft.AspNetCore.Mvc;

namespace StocksApp.Controllers
{
    [Route("[controller]")]
    public class HomeController : Controller
    {
        [Route("[action]")]
        public IActionResult Error()
        {
            return View();
        }
    }
}
