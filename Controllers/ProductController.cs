using System.Diagnostics.CodeAnalysis;
using HelmonyCornDog.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HelmonyCornDog.Controllers
{
    public class ProductController : Controller
    {
        CornDogContext _context;

        public ProductController(CornDogContext context)
        {
            _context = context;
        }


        [Route("Group/{id}/{name}")]
        [SuppressMessage("ReSharper.DPA", "DPA0011: High execution time of MVC action", MessageId = "time: 4937ms")]
        public IActionResult ShowProductByGroupId(int id,string name)
        {
            ViewData["GroupName"] = name;
            
            var products = _context.CategoryToProducts.Where(i => i.CategoryId == id).Include(p => p.Product).Include(i=>i.Product.Item)
                .Select(p => p.Product).ToList();

            foreach (var product in products)
            {
                product.ImagePath = GetImagePath(product.Id);
            }

            ViewData["Products"] = products;

            return View(products);
        }

        private string GetImagePath(int productId)
        {
            var imageExtensions = new[] { "jpg", "png", "jpeg", "gif" };
            foreach (var ext in imageExtensions)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", $"{productId}.{ext}");
                if (System.IO.File.Exists(path))
                {
                    return $"/images/{productId}.{ext}";
                }
            }
            return null; // Fallback if no image is found
        }
    }
}
