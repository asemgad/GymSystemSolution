using System.Diagnostics;
using System.Threading.Tasks;
using GymSystem.BLL.Services.Interfaces;
using GymSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAnalytices analyticsService;

        public HomeController(ILogger<HomeController> logger, IAnalytices analyticsService)
        {
            _logger = logger;
            this.analyticsService = analyticsService;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var analytics = await analyticsService.GetAnalyticsAsync(ct);

            return View(analytics);
        }

        public IActionResult Privacy()
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
