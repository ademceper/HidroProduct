using Microsoft.AspNetCore.Mvc;
using White.Data;

namespace HidroProduct.Components
{
    public class CategoryMenuViewComponent : ViewComponent
    {
        private readonly AppDbContext context;

        public CategoryMenuViewComponent(
            AppDbContext context
            )
        {
            this.context = context;
        }

        public IViewComponentResult Invoke()
        {
            var model = context.Categories.ToList();
            return View(model);
        }
    }
}
