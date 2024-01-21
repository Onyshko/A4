using A4.Models;
using A4.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace A4.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHomeService _homeService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            ILogger<HomeController> logger,
            IHomeService homeService)
        {
            _logger = logger;
            _homeService = homeService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var homeVMJson = HttpContext.Session.GetString("HomeVM");
            HomeVM homeVM;

            if (!string.IsNullOrEmpty(homeVMJson))
            {
                homeVM = JsonConvert.DeserializeObject<HomeVM>(homeVMJson);
            }
            else
            {
                homeVM = InitializeHomeVM();
            }

            return View(homeVM);
        }

        private HomeVM InitializeHomeVM()
        {
            var n = TempData["N"] as int? ?? 0;
            var m = TempData["M"] as int? ?? 0;
            var homeVM = new HomeVM { N = n, M = m };

            for (int i = 0; i < n; i++)
            {
                var tmpList = new List<Cell>();
                for (int j = 0; j < m; j++)
                {
                    tmpList.Add(new Cell { Row = i, Col = j, IsSelected = false });
                }

                homeVM.Cells.Add(tmpList);
            }

            var homeVMJson = JsonConvert.SerializeObject(homeVM);
            HttpContext.Session.SetString("HomeVM", homeVMJson);

            return homeVM;
        }

        [HttpPost]
        public IActionResult CalculateSheets()
        {
            var homeVMJson = HttpContext.Session.GetString("HomeVM");
            var homeVM = JsonConvert.DeserializeObject<HomeVM>(homeVMJson);

            homeVM.SheetsCounter = _homeService.CounterOfSheets(homeVM);

            homeVMJson = JsonConvert.SerializeObject(homeVM);
            HttpContext.Session.SetString("HomeVM", homeVMJson);

            return View("Index", homeVM);
        }

        [HttpPost]
        public IActionResult UpdateCellSelection(int row, int col)
        {
            var homeVMJson = HttpContext.Session.GetString("HomeVM");
            var homeVM = JsonConvert.DeserializeObject<HomeVM>(homeVMJson);

            var cell = homeVM.Cells[row][col];
            if (cell != null)
            {
                cell.IsSelected = !cell.IsSelected;
            }

            homeVMJson = JsonConvert.SerializeObject(homeVM);
            HttpContext.Session.SetString("HomeVM", homeVMJson);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult CreateGrid()
        {
            var gridVM = new GridVM();
            return View(gridVM);
        }

        [HttpPost]
        public IActionResult CreateGrid(GridVM gridVM)
        {
            if (ModelState.IsValid)
            {
                TempData["N"] = gridVM.N;
                TempData["M"] = gridVM.M;

                HttpContext.Session.Remove("HomeVM");

                return RedirectToAction(nameof(Index));
            }

            return View(gridVM);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}