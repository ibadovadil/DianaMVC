 using Diana.Mvc.Contexts;
using Diana.Mvc.ViewModels.CommonVM;
using Diana.Mvc.ViewModels.HomeVM;
using Diana.Mvc.ViewModels.ProductVM;
using Diana.Mvc.ViewModels.SliderVM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Diana.Mvc.Controllers
{
    public class HomeController : Controller
    {
        DianaDbContext _db { get; }

        public HomeController(DianaDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            int take = 4;
            var items = _db.Products.Where(p => !p.IsDeleted).Take(take).Select(s => new ProductListItemVMTEST
            {
                Id = s.Id,
                Name = s.Name,
                ImageUrl = s.ImageUrl,
                Category = s.Category,
                //CostPrice = s.CostPrice,
                //IsDeleted= s.IsDeleted,
                Discout = s.Discout,
                Quantity = s.Quantity,
                SellPrice = s.SellPrice
            });

            int count = await _db.Products.CountAsync(x=>!x.IsDeleted);
            PaginationVM<IEnumerable<ProductListItemVMTEST>> pag = new (count , 1, (int)Math.Ceiling((decimal)count/take) , items);
            
            HomeVM vm = new HomeVM
            {
                //Sliders = await _db.Sliders.Select(s => new SliderListItemVM
                //{
                //    Id = s.Id,
                //    ImageUrl = s.ImageUrl,
                //    IsLeft = s.IsLeft,
                //    Text = s.Text,
                //    Title = s.Title,
                //}).ToListAsync(), silirk cunki viewcomponentdne gelir 
                Products = await _db.Products.Where(p => !p.IsDeleted).Select(s => new ProductListItemVMTEST
                {
                    Id = s.Id,
                    Name = s.Name,
                    ImageUrl = s.ImageUrl,
                    Category = s.Category,
                    //CostPrice = s.CostPrice,
                    //IsDeleted= s.IsDeleted,
                    Discout = s.Discout,
                    Quantity = s.Quantity,
                    SellPrice = s.SellPrice
                }).ToListAsync(),

                PaginatedProducts = pag
            };
            return View(vm);
        }


        public async Task<IActionResult> ProductPagination(int page = 1, int count = 4)
        {
            var items = _db.Products.Where(p => !p.IsDeleted).Skip((page-1)*count).Take(count).Select(s => new ProductListItemVMTEST
            {
                Id = s.Id,
                Name = s.Name,
                ImageUrl = s.ImageUrl,
                //Category = s.Category,
                //CostPrice = s.CostPrice,
                //IsDeleted= s.IsDeleted,
                Discout = s.Discout,
                Quantity = s.Quantity,
                SellPrice = s.SellPrice
            });

            int totalCount = await _db.Products.CountAsync(x => !x.IsDeleted);
            PaginationVM<IEnumerable<ProductListItemVMTEST>> pag = new(totalCount, page, (int)Math.Ceiling((decimal)totalCount / count), items);
            var datas = await _db.Products.Where(p => !p.IsDeleted).Skip((page-1)*count).Take(count).ToListAsync();
            return PartialView("_ProductPaginationPartial",pag);
            //return Json(datas);
        }
    }
}
