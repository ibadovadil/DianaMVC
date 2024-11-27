using Diana.Mvc.Contexts;
using Diana.Mvc.ViewModels.ProductVM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Diana.Mvc.Controllers
{
    public class ProductController : Controller
    {
        DianaDbContext _db { get; }

        public ProductController(DianaDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Details(int id)
        {
            if (id == null || id <= 0) return BadRequest();
            var data = (await _db.Products.Select(x => new ProductDetailVM
            {
                Id = x.Id,
                Discout = x.Discout,
                Category = x.Category,
                Colors = x.ProductColors.Select(p => p.Color),
                SellPrice=(x.SellPrice-(x.SellPrice * (decimal)x.Discout)/100).ToString("C2"),
                ImageUrl = x.ImageUrl,
                Name = x.Name,
                Quantity = x.Quantity,
                Description = x.Description,
                About = x.About,
                ImageUrls = x.ProductImages.Select(p => p.ImageUrl),
            }).SingleOrDefaultAsync(p => p.Id == id));
            if (data == null) return NotFound();
            return View(data);


            //if (id == null || id <= 0) return BadRequest();
            //var data = await _db.Products.Where(x => x.Id == id).Select(x => new ProductDetailVM
            //{
            // ....
            //}).FirstOrDefaultAsync();
            //if (data == null) return NotFound();
            //return View(data);
        }
    }
}
