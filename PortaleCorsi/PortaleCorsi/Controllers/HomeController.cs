using Microsoft.AspNetCore.Mvc;
using PortaleCorsi.Models;
using PortaleCorsi.Repositories;
using System.Diagnostics;

namespace PortaleCorsi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        readonly ICorsoRepository _corsoRepository;
        public HomeController(ILogger<HomeController> logger,ICorsoRepository corso)
        {
            _logger = logger;
            _corsoRepository = corso;
        }

        public IActionResult Index()
        {
            //_corsoRepository.AddAsync
            return View();
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
