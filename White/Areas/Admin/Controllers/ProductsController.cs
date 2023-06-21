
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using X.PagedList;
using White.Data;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;

namespace HidroProduct.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly AppDbContext context;

        public ProductsController(AppDbContext context)
        {
            this.context = context;
        }
        [Authorize(Roles = "Administrators,Siliciler,Görüntüleyiciler,Kaydediciler")]
        public IActionResult Index(int? page)
        {
            var model = context.Products.OrderBy(p => p.Name).ToPagedList(page ?? 1, 10);
            
            return View(model);
        }
        public async Task<IActionResult> TableData(Guid? id, DataTableParameters parameters)
        {
            var query = context.Products;

            var result = new DataTableResult
            {
                data = await query
                    .Skip(parameters.Start)
                    .Take(parameters.Length)
                    .Select(p => new
                    {
                        p.id,
                        p.Name,
                        p.Image,
                        p.Piece,
                        p.Price,
                        p.SupplierName,
                        categoryName = p.Category!.Name,
                        UserName = p.CreatorUser!.Name,
                      
                   
                    }).ToListAsync(),
                draw = parameters.Draw,
                recordsFiltered = await query.CountAsync(),
                recordsTotal = await query.CountAsync()
            };

            return Json(result);
        }
        [Authorize(Roles = "Administrators,Kaydedici")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = new SelectList(await context.Categories.OrderBy(p => p.Name).ToListAsync(), "Id", "Name");
            
            return View();
        }

        
        [HttpPost]
        [Authorize(Roles = "Administrators,Kaydedici")]
        public async Task<IActionResult> Create(Product model)
        {
            ViewBag.Categories = new SelectList(await context.Categories.OrderBy(p => p.Name).ToListAsync(), "Id", "Name");


            if (model.ImageFile is not null)
            {
                using var image = await Image.LoadAsync(model.ImageFile.OpenReadStream());
                //using var ms = new MemoryStream();
                //BlobClient blobClient = new BlobClient("sdvsv", "Container1", "Films");

                image.Mutate(p => p.Resize(new ResizeOptions
                {
                    Size = new Size(500, 740),
                    Mode = ResizeMode.Crop
                }));
                //image.SaveAsJpeg(ms);
                //var response = await blobClient.UploadAsync(ms);
                //model.Image = response.Value.BlobSequenceNumber.ToString();
                model.Image = image.ToBase64String(JpegFormat.Instance);

            }
            model.Price = decimal.Parse(model.PriceText, CultureInfo.CreateSpecificCulture("tr-TR"));

            
            

            try
            {
                context.Products.Add(model);
                context.SaveChanges();
                TempData["success"] = "Ürün ekleme işlemi başarıyla tamamlanmıştır";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                TempData["error"] = "Aynı isimli bir başka kayıt olduğundan kayıt işlemi tamamlanamıyor!";
                return View(model);
            }

        }
        [Authorize(Roles = "Administrators,Güncelleyiciler")]
        public async Task<IActionResult> Edit(Guid id)
        {
            ViewBag.Categories = new SelectList(await context.Categories.OrderBy(p => p.Name).ToListAsync(), "Id", "Name");
            
            var model = context.Products.Find(id);
            return View(model);
        }
        [Authorize(Roles = "Administrators,Güncelleyiciler")]
        [HttpPost]
        public async Task<IActionResult> Edit(Product model)
        {
            ViewBag.Categories = new SelectList(await context.Categories.OrderBy(p => p.Name).ToListAsync(), "Id", "Name");


            if (model.ImageFile is not null)
            {
                using var image = await Image.LoadAsync(model.ImageFile.OpenReadStream());
                

                image.Mutate(p => p.Resize(new ResizeOptions
                {
                    Size = new Size(500, 740),
                    Mode = ResizeMode.Crop
                }));
               
                model.Image = image.ToBase64String(JpegFormat.Instance);

            }

            model.Price = decimal.Parse(model.PriceText, CultureInfo.CreateSpecificCulture("tr-TR"));

            context.Products.Add(model);
            try
            {
                context.Products.Update(model);
                context.SaveChanges();
                TempData["success"] = "Ürün güncelleme işlemi başarıyla tamamlanmıştır";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                TempData["error"] = "Aynı isimli bir başka kayıt olduğundan kayıt işlemi tamamlanamıyor!";
                return View(model);
            }

            


        }
        [Authorize(Roles = "Administrators,Siliciler")]
        public IActionResult Delete(Guid id)
        {
            var model = context.Products.Find(id);
            context.Products.Remove(model);
            context.SaveChanges();
            TempData["success"] = "Ürün silme işlemi başarıyla tamamlanmıştır";
            return RedirectToAction(nameof(Index));
        }
    }
}
