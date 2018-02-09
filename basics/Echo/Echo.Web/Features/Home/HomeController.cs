using System.Threading.Tasks;
using Echo.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Echo.Web.Features.Home
{
    public class HomeController : Controller
    {
        private readonly IEchoService _echoService;

        public HomeController(IEchoService echoService)
        {
            _echoService = echoService;
        }

        public IActionResult Index() => View();
        [Authorize]
        public IActionResult Secret() => View();
        

        [HttpGet]
        [Authorize]
        public IActionResult Echo()
        {
            return View(new EchoViewModel());
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Echo(EchoViewModel model)
        {
            model.Result = await _echoService.Echo(model.Message);
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Tokens()
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            return View();
        }

        public async Task Logout()
        {
            await HttpContext.SignOutAsync("Cookies");
            await HttpContext.SignOutAsync("oidc");
        }

        
    }
}
