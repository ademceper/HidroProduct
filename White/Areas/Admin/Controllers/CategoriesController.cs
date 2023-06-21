using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HidroProduct;
using White.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace HidroProduct.Areas.Admin.Controllers
{
    [Area("Admin")]
   
    public class CategoriesController : Controller
    {
        private readonly AppDbContext context;

        public CategoriesController(AppDbContext context ,RoleManager<AppRole>roleManager)
        {
            this.context = context;
        }
        [Authorize(Roles = "Administrators,Siliciler,Görüntüleyiciler,Kaydediciler,Güncelleyiciler")]

        public IActionResult Index()
        {
            var model = context.Categories.OrderBy(p => p.Name).ToList();
            return View(model);
        }
        public async Task<IActionResult> TableData(DataTableParameters parameters)
        {
            var query = await context.Categories.ToListAsync();

            var result = new DataTableResult
            {
                data =  query.Skip(parameters.Start).Take(parameters.Length).Select(p => new { p.Name, p.Id }).ToList(),
                draw = parameters.Draw,
                recordsFiltered =  query.Count(),
                recordsTotal =  query.Count()
            };

            return Json(result);
        }
        [Authorize(Roles = "Administrators")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrators")]
        public IActionResult Create(Category model)
        {
            context.Categories.Add(model);
            try
            {
                context.SaveChanges();
                TempData["success"] = "Kategori ekleme işlemi başarıyla tamamlanmıştır";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                TempData["error"] = "Aynı isimli bir başka kayıt olduğundan kayıt işlemi tamamlanamıyor!";
                return View(model);
            }
        }
        [Authorize(Roles = "Administrators")]
        public IActionResult Edit(Guid id)
        {
            var model = context.Categories.Find(id);
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Administrators")]
        public IActionResult Edit(Category model)
        {
            context.Categories.Update(model);
            try
            {
                context.SaveChanges();
                TempData["success"] = "Kategori güncelleme işlemi başarıyla tamamlanmıştır";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                TempData["error"] = "Aynı isimli bir başka kayıt olduğundan kayıt işlemi tamamlanamıyor!";
                return View(model);
            }
        }
        [Authorize(Roles = "Administrators")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var model = await context.Categories.FindAsync(id);
            if (model != null)
            {
                context.Categories.Remove(model);
            }
            try
            {
                context.SaveChanges();
                TempData["success"] = "Kategori silme işlemi başarıyla tamamlanmıştır";
            }
            catch (DbUpdateException)
            {
                TempData["error"] = "Bir yada daha fazla kayıt ile ilişkili olduğundan silme işlemi yapılamıyor!";

            }
            return RedirectToAction(nameof(Index));
        }
    }
}
