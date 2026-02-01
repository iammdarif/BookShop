using DI_Service_Lifetime.Models;
using DI_Service_Lifetime.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;

namespace DI_Service_Lifetime.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISingletonService singletonService1;
        private readonly ISingletonService singletonService2;
        private readonly IScopedService scopedService1;
        private readonly IScopedService scopedService2;
        private readonly ITransientService transientService1;
        private readonly ITransientService transientService2;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            ISingletonService singletonService1,
            ISingletonService singletonService2,
            IScopedService scopedService1,
            IScopedService scopedService2,
            ITransientService transientService1,
            ITransientService transientService2,
            ILogger<HomeController> logger)
        {
            this.singletonService1 = singletonService1;
            this.singletonService2 = singletonService2;
            this.scopedService1 = scopedService1;
            this.scopedService2 = scopedService2;
            this.transientService1 = transientService1;
            this.transientService2 = transientService2;
            _logger = logger;
        }

        public IActionResult Index()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"Transient 1: {transientService1.GetGuid()}\n");
            sb.Append($"Transient 2: {transientService2.GetGuid()}\n\n\n");

            sb.Append($"Scoped 1: {scopedService1.GetGuid()}\n");
            sb.Append($"Scoped 2: {scopedService2.GetGuid()}\n\n\n");

            sb.Append($"Singleton 1: {singletonService1.GetGuid()}\n");
            sb.Append($"Singleton 2: {singletonService2.GetGuid()}\n\n\n");

            return Ok(sb.ToString());
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
