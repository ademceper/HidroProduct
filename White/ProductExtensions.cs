using X.PagedList.Web.Common;

namespace HidroProduct
{
    public static class ProductExtensions
    {
        public static PagedListRenderOptions PagedListOptions => new PagedListRenderOptions
        {
            ActiveLiElementClass = "active",
            LiElementClasses = new[] { "page-item" },
            PageClasses = new[] { "page-link" }
        };
    }
}
