using Diana.Mvc.Contexts;
using Diana.Mvc.Models;
using Diana.Mvc.ViewModels.SliderVM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Diana.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController : Controller
    {
        DianaDbContext _db { get; }

        public SliderController(DianaDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            //var sliders = await db.Sliders.ToListAsync();
            //List<SliderListItemVM> items = new List<SliderListItemVM>();
            //foreach (var item in sliders)
            //{
            //    items.Add(new SliderListItemVM
            //    {
            //        Id = item.Id,
            //        ImageUrl = item.ImageUrl,
            //        IsLeft = item.IsLeft,
            //        Text = item.Text,
            //        Title=item.Title,
            //    });
            //}
            //yuxaridaki kodla asagidaki eyni isi gorur//
            var items = await _db.Sliders.Select(s => new SliderListItemVM
            {
                Title = s.Title,
                Text = s.Text,
                IsLeft = s.IsLeft,
                Id = s.Id,
                ImageUrl = s.ImageUrl,
            }).ToListAsync();

            return View(items);
        }


        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(SliderCreateVM vm)
        {
            if (vm.Position < -1 || vm.Position > 1)
            {
                ModelState.AddModelError("Position", "Wrong Input!");
            }
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            Slider slider = new Slider
            {
                ImageUrl = vm.ImageUrl,
                Text = vm.Text,
                Title = vm.Title,
                IsLeft = vm.Position switch
                {
                    0 => null,
                    -1 => true,
                    1 => false,
                    // added for me
                    _ => throw new NotImplementedException(),
                }
            };
            await _db.Sliders.AddAsync(slider);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            TempData["Response"] = false;
            if (id == null) return BadRequest();
            //var data = await db.Sliders.SingleOrDefaultAsync();
            var data = await _db.Sliders.FindAsync(id);
            //if (data == null) return NotFound();
            if (data == null) return RedirectToAction(nameof(Index));
            _db.Sliders.Remove(data);
            await _db.SaveChangesAsync();
            TempData["Response"] = true;
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Update(int id)
        {
            if (id == null || id <= 0) return BadRequest();
            //var data = await db.Sliders.SingleOrDefaultAsync();
            var data = await _db.Sliders.FindAsync(id);
            if (data == null) return NotFound();
            //if (data == null) return RedirectToAction(nameof(Index)); 
            return View(new SliderUpdateVM
            {
                ImageUrl = data.ImageUrl,
                Text = data.Text,
                Title = data.Title,
                Position = data.IsLeft switch
                {
                    true => -1,
                    null => -1,
                    false => 1
                }
            });
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id, SliderUpdateVM vm)
        {
            if (id == null || id <= 0) return BadRequest();

            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            if (vm.Position < -1 || vm.Position > 1)
            {
                ModelState.AddModelError("Position", "Wrong Input!");
            }
            var data = await _db.Sliders.FindAsync(id);
            if (data == null) return NotFound();
            data.Text = vm.Text;
            data.Title = vm.Title;
            data.ImageUrl = vm.ImageUrl;
            data.IsLeft = vm.Position switch
            {
                0 => null,
                -1 => true,
                1 => false,
            };
            await _db.SaveChangesAsync();
            TempData["Response"] = true;
            return RedirectToAction(nameof(Index));
        }
    }
}