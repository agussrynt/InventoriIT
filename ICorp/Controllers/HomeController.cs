using InventoryIT.Helpers;
using InventoryIT.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace InventoryIT.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IOptions<Setting> _setting;
        private readonly ILogger<HomeController> _logger;
        private string _urlSHIP { get; set; }
        private string _aplicationId { get; set; }

        public HomeController(
            IOptions<Setting> setting,
            ILogger<HomeController> logger)
        {
            _setting = setting;
            _aplicationId = _setting.Value.ActiveSSO;
            _urlSHIP = _setting.Value.baseURL_SIHP;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Route("/access-sihp")]
        public RedirectResult accessSIHP()
        {
            string username = HttpContext.Session.GetString("username");
            string password = HttpContext.Session.GetString("password");
            var enc = Cryptografi.Encrypt(username);
            var pasenc = Cryptografi.Encrypt(password);
            var url = _urlSHIP + enc + "&IL=1&UIP=" + pasenc;

            return RedirectPermanent(url);
        }

        [Route("/Account/Profile")]
        public IActionResult Profile()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}