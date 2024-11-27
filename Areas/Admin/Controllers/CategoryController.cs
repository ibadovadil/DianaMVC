using Diana.Mvc.Contexts;
using Diana.Mvc.Models;
using Diana.Mvc.ViewModels.CatgeoryVM;
using Diana.Mvc.ViewModels.SliderVM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Diana.Mvc.Areas.Admin.Controllers
{

    [Area("Admin")]

    public class CategoryController : Controller
    {
        DianaDbContext _db { get; }

        public CategoryController(DianaDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            var items = await _db.Categories.Select(s => new CategoryListItemVM
            {
                Name = s.Name,
                Id = s.Id,
            }).ToListAsync();

            return View(items);
        }


        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            if (await _db.Categories.AnyAsync(x=>x.Name == vm.Name))
            {
                ModelState.AddModelError("Name",  vm.Name + " Already exist");
                return View(vm);
            }
            Category category = new Category{Name = vm.Name};
            await _db.Categories.AddAsync(category);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        public async Task<IActionResult> Delete(int id)
        {
            TempData["Response"] = false;
            if (id == null) return BadRequest();
            var data = await _db.Categories.FindAsync(id);
            if (data == null) return RedirectToAction(nameof(Index));
            _db.Categories.Remove(data);
            await _db.SaveChangesAsync();
            TempData["Response"] = true;
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null || id <= 0) return BadRequest();
            var data = await _db.Categories.FindAsync(id);
            if (data == null) return NotFound();
            return View(new CategoryUpdateVM
            {
                Name = data.Name,
               
            });
        }
        [HttpPost]

        public async Task<IActionResult> Update(int? id,CategoryUpdateVM vm)
        {
            if (id == null || id <= 0) return BadRequest();

            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var data = await _db.Categories.FindAsync(id);
            if (data == null) return NotFound();
            data.Name = vm.Name;
           
            await _db.SaveChangesAsync();
            TempData["Response"] = true;
            return RedirectToAction(nameof(Index));
        }
    }
}
