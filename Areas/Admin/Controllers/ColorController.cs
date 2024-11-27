using Diana.Mvc.Contexts;
using Diana.Mvc.Models;
using Diana.Mvc.ViewModels.ColorVM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Diana.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ColorController : Controller
    {
        DianaDbContext _db { get; set; }

        public ColorController(DianaDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _db.Colors.Select(x => new ColorListItemVM
            {
                Id = x.Id,
                Name = x.Name,
                Hexcode = x.Hexcode,
            }).ToListAsync());
        }


        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ColorCreateVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            await _db.Colors.AddAsync(new Color
            {
                Name = vm.Name,
                Hexcode = vm.Hexcode.Substring(1)
            });
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
