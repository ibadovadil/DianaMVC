using Diana.Mvc.Areas.Admin.ViewModels;
using Diana.Mvc.Contexts;
using Diana.Mvc.Helpers;
using Diana.Mvc.Models;
using Diana.Mvc.ViewModels.ProductVM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;

namespace Diana.Mvc.Areas.Admin.Controllers;
[Area("Admin")]
public class ProductController : Controller
{
    DianaDbContext _db { get; }
    IWebHostEnvironment _env { get; }

    public ProductController(DianaDbContext db, IWebHostEnvironment env)
    {
        _db = db;
        _env = env;
    }

    public IActionResult Index()
    {
        //return View(_db.Products.Include(p=>p.Category).Select(p=>new AdminProductListItemVM { 
        return View(_db.Products.Select(p => new AdminProductListItemVM
        {
            Id = p.Id,
            Name = p.Name,
            CostPrice = p.CostPrice,
            Category = p.Category,
            Discout = p.Discout,
            ImageUrl = p.ImageUrl,
            Quantity = p.Quantity,
            SellPrice = p.SellPrice,
            IsDeleted = p.IsDeleted,
            Colors = p.ProductColors.Select(c => c.Color)
            //include ve mapper ilede olur  de mumkundur
        }));
    }
    public async Task<IActionResult> Create()
    {
        ViewBag.Categories = _db.Categories;
        //ViewBag.Colors = _db.Colors;
        ViewBag.Colors = new SelectList(_db.Colors, "Id", "Name");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductCreateVM vm)
    {
        if (vm.ImageFile != null)
        {

            if (!vm.ImageFile.IsCorrectType("image"))
            {
                ModelState.AddModelError("ImageFile", "Wrong File Type");
            }
            //if (!vm.ImageFile.IsValidSize()) 20kb
            if (!vm.ImageFile.IsValidSize(50f))
            {
                ModelState.AddModelError("ImageFile", "Files Must be less than " + 50f + "kb");
            }
        }
        if (vm.Images != null && vm.Images?.Count() > 0)
        {
            //string message = string.Empty;
            foreach (var img in vm.Images)
            {
                if (!img.IsCorrectType("image"))
                {
                    //message += "Wrong file type (" + img.FileName + ")";
                    ModelState.AddModelError("", "Wrong File Type(" + img.FileName + ")");
                }
                if (!img.IsValidSize(1050))
                {
                    //message += "Files lenght must be less than kb (" + img.FileName + ")";
                    ModelState.AddModelError("", "Files Must be less than " + 50f + "kb(" + img.FileName + ")");
                }
            }
            //if (!string.IsNullOrEmpty(message))
            //{
            //    ModelState.AddModelError("Images", message);
            //}
        }
        if (vm.CostPrice > vm.SellPrice)
        {
            ModelState.AddModelError("SellPrice", "Sell Price must be bigger than Cost Price");
        }
        if (!ModelState.IsValid)
        {
            ViewBag.Categories = _db.Categories;
            ViewBag.Colors = new SelectList(_db.Colors, "Id", "Name");
            return View(vm);
        }
        if (!await _db.Categories.AnyAsync(x => x.Id == vm.CategoryId))
        {
            ModelState.AddModelError("CategoryId", "Category doesnt exist");
            ViewBag.Categories = _db.Categories;
            ViewBag.Colors = new SelectList(_db.Colors, "Id", "Name");
            return View(vm);
        }


        //var a = await (from c in _db.Colors
        //               where vm.ColorIds.Contains(c.Id)
        //               select c.Id).CountAsync();  //LINQ
        // any ilede yaza bilerdik ama any db ni cox yorur
        //if (await _db.Colors.Where(x => vm.ColorIds.Contains(x.Id)).CountAsync() != vm.ColorIds.Count())
        if (await _db.Colors.Where(x => vm.ColorIds.Contains(x.Id)).Select(x => x.Id).CountAsync() != vm.ColorIds.Count())
        {
            ModelState.AddModelError("ColorIds", "Color doesnt exist");
            ViewBag.Colors = new SelectList(_db.Colors, "Id", "Name");
            return View(vm);
        }

        // FileExtenison.cs
        ////string fileName = Path.Combine("assets", "img", "products", vm.ImageFile.FileName); eyni adli sekillerde,sekiller bir birini ovveride edir deye bu usul problemlidir
        //string fileName = Path.Combine(PathConstants.Product, Path.GetRandomFileName()+ Path.GetExtension(vm.ImageFile.FileName)); //GUID de istifade oluna biler

        ////using (FileStream fs = System.IO.File.Create(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", fileName)))
        //using (FileStream fs = System.IO.File.Create(Path.Combine(_env.WebRootPath, fileName)))
        //{
        //    await vm.ImageFile.CopyToAsync(fs);
        //}

        Product product = new Product
        {
            Name = vm.Name,
            About = vm.About,
            Description = vm.Description,
            Quantity = vm.Quantity,
            SellPrice = vm.SellPrice,
            CostPrice = vm.CostPrice,
            CategoryId = vm.CategoryId,
            Discout = vm.Discout,
            ImageUrl = await vm.ImageFile.SaveAsync(PathConstants.Product),
            ProductColors = vm.ColorIds.Select(id => new ProductColor
            {
                ColorId = id
            }).ToList(),

            ProductImages = vm.Images.Select(i => new ProductImage
            {
                ImageUrl = i.SaveAsync(PathConstants.Product).Result
            }).ToList(),

            //ProductImages = (await Task.WhenAll(vm.Images.Select(async i => new ProductImage
            //{
            //    ImageUrl = await i.SaveAsync(PathConstants.Product)
            //}))).ToList()  basqa yolu


        };
        //product colur elave etmekdir
        //IList<ProductColor> list = new List<ProductColor>();
        //foreach (var id in vm.ColorIds)
        //{
        //    list.Add(new ProductColor
        //    {
        //        ColorId=id,
        //        Product=product,
        //    });
        //}

        await _db.Products.AddAsync(product);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }


    public async Task<IActionResult> Delete(int id)
    {
        TempData["Response"] = false;
        if (id == null) return BadRequest();
        var data = await _db.Products.FindAsync(id);
        if (data == null) return RedirectToAction(nameof(Index));
        _db.Products.Remove(data);
        await _db.SaveChangesAsync();
        TempData["Response"] = true;
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Update(int? id)
    {
        if (id == null || id <= 0) return BadRequest();
        ViewBag.Colors = new SelectList(_db.Colors, nameof(Color.Id), nameof(Color.Name));
        ViewBag.Categories = new SelectList(_db.Categories, nameof(Category.Id), nameof(Category.Name));
        var data = await _db.Products
            //.Include(p => p.ProductColors)
            //.ThenInclude(pc => pc.Color)
            //.Include(p => p.Category)
            .Include(p => p.ProductImages)
            .Include(p => p.ProductColors) // select yeni obyekt qaytarir inclde secir(select daxilinde . qoyduqlarimizi include edir)
            .SingleOrDefaultAsync(p => p.Id == id);
        if (data == null) return BadRequest();
        var vm = new ProductUpdateVM
        {
            About = data.About,
            CategoryId = data.CategoryId,
            ColorIds = data.ProductColors.Select(c => c.ColorId), // tolist elemeye gerek yoxdur cunki ienumerable ozu tolist edir
            CostPrice = data.CostPrice,
            Description = data.Description,
            Discout = data.Discout,
            Name = data.Name,
            Quantity = data.Quantity,
            SellPrice = data.SellPrice,
            ImageUrls = data.ProductImages.Select(pi => new ProductImageVM
            {
                Id = pi.Id,
                Url = pi.ImageUrl,
            }),// tolist elemeye gerek yoxdur cunki ienumerable ozu tolist edir
            CoverImageUrl = data.ImageUrl,
        };
        return View(vm);
    }
    [HttpPost]
    public async Task<IActionResult> Update(int id, ProductUpdateVM vm)
    {
        if (id == null || id <= 0) return BadRequest();
        ViewBag.Categories = new SelectList(_db.Categories, nameof(Category.Id), nameof(Category.Name));
        ViewBag.Colors = new SelectList(_db.Colors, nameof(Color.Id), nameof(Color.Name));
        if (!vm.ColorIds.Any()) // any ici bosdusa yeni false !=
        {
            ModelState.AddModelError("ColorIds", "You must add at lest 1 color");
        }

        if (vm.ImageFile != null)
        {

            if (!vm.ImageFile.IsCorrectType("image"))
            {
                ModelState.AddModelError("ImageFile", "Wrong File Type");
            }
            if (!vm.ImageFile.IsValidSize(50f))
            {
                ModelState.AddModelError("ImageFile", "Files Must be less than " + 50f + "kb");
            }
        }
        if (vm.Images != null && vm.Images?.Count() > 0)
        {
            foreach (var img in vm.Images)
            {
                if (!img.IsCorrectType("image"))
                {
                    ModelState.AddModelError("", "Wrong File Type(" + img.FileName + ")");
                }
                if (!img.IsValidSize(1050))
                {
                    ModelState.AddModelError("", "Files Must be less than " + 50f + "kb(" + img.FileName + ")");
                }
            }
        }
        if (vm.CostPrice > vm.SellPrice)
        {
            ModelState.AddModelError("SellPrice", "Sell Price must be bigger than Cost Price");
        }
        if (!ModelState.IsValid)
        {
            ViewBag.Categories = new SelectList(_db.Categories, nameof(Category.Id), nameof(Category.Name));

            ViewBag.Colors = new SelectList(_db.Colors, nameof(Color.Id), nameof(Color.Name));

            return View(vm);
        }
        if (!await _db.Categories.AnyAsync(x => x.Id == vm.CategoryId))
        {
            ModelState.AddModelError("CategoryId", "Category doesnt exist");
            ViewBag.Categories = new SelectList(_db.Categories, nameof(Category.Id), nameof(Category.Name));

            ViewBag.Colors = new SelectList(_db.Colors, nameof(Color.Id), nameof(Color.Name));

            return View(vm);
        }
        if (!ModelState.IsValid) return View(vm);
        var data = await _db.Products

            .Include(p => p.ProductImages)
            .Include(p => p.ProductColors)
            .SingleOrDefaultAsync(p => p.Id == id);

        data.Name = vm.Name;
        data.About = vm.About;
        data.Description = vm.Description;
        data.Quantity = vm.Quantity;
        data.SellPrice = vm.SellPrice;
        data.CostPrice = vm.CostPrice;
        data.CategoryId = vm.CategoryId;
        data.Discout = vm.Discout;
        if (data == null) return BadRequest();
        if (vm.ImageFile != null)
        {
            string filePath = Path.Combine(PathConstants.RootPath, data.ImageUrl);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            data.ImageUrl = await vm.ImageFile.SaveAsync(PathConstants.Product);
        }
        if (vm.Images != null)
        {

            var imgs = vm.Images.Select(i => new ProductImage
            {
                ImageUrl = i.SaveAsync(PathConstants.Product).Result,
                ProductId = data.Id
            });
            data.ProductImages.AddRange(imgs);
        }
        //if (Enumerable.SequenceEqual(data.ProductColors?.Select(p => p.ColorId), vm.ColorIds)) // bir nov compare kimidir buradaki method 2 arrayi muqayise edir
        //{
        //    data.ProductColors = vm.ColorIds.Select(c => new ProductColor { ColorId = c, ProductId = data.Id }).ToList();
        //}; // Mycode 
        if (data.ProductColors != null) //GPT CODE
        {
            // Eski renkleri temizle
            _db.ProductColors.RemoveRange(data.ProductColors);

            // Yeni renkleri ekle
            data.ProductColors = vm.ColorIds.Select(c => new ProductColor
            {
                ColorId = c,
                ProductId = data.Id
            }).ToList();
        }

        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> DeleteImageCSharp(int? Id)
    {
        if (Id == null) return BadRequest();
        var data = await _db.ProductImages.FindAsync(Id);
        if (data == null) return NotFound();
        _db.ProductImages.Remove(data);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index), new { Id = data.ProductId });
    }

    public async Task<IActionResult> DeleteImage(int? Id)
    {
        if (Id == null) return BadRequest();
        var data = await _db.ProductImages.FindAsync(Id);
        if (data == null) return NotFound();
        _db.ProductImages.Remove(data);
        await _db.SaveChangesAsync();
        return Ok();
    }
}
