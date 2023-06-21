using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using White.Data;
using White.Models;
using X.PagedList;

namespace White.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext context;
        public HomeController(
            ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            this.context = context;
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.MostViews = await context.Products.OrderByDescending(p => p.Price).Take(15).ToListAsync();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }
        public async Task<IActionResult> Product(Guid id)
        {
            var product = await context.Products.SingleOrDefaultAsync(p => p.id == id);


            return View(product);

        }
        public async Task<IActionResult> Category(Guid id, int? page)
        {
            var category = await context.Categories.SingleOrDefaultAsync(p => p.Id == id);
            ViewBag.Category = category;
            var model = category.Products.ToPagedList(page ?? 1, 15);
            return View(model);


        }
        public async Task<IActionResult> Search(string keyword, int? page)
        {
           
            if(keyword == null)
            {
                return RedirectToAction("Index");

            }
            ViewBag.Keyword = keyword;
            var model = (await context.Products.Where(p => p.Name.Contains(keyword)).ToListAsync()).ToPagedList(page ?? 1, 12);
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}