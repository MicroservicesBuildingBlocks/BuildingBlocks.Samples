using Microsoft.AspNetCore.Mvc;

namespace Echo.WebApi.Features.Home
{
    public class HomeController : Controller
    {
        public IActionResult Index() =>
            new RedirectResult("~/swagger");
    }
}
