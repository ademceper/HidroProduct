using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace White.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrators,Siliciler,Görüntüleyiciler,Kaydediciler,Güncelleyiciler")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
