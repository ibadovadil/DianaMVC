using Diana.Mvc.Contexts;
using Diana.Mvc.Models;
using Diana.Mvc.ViewModels.SliderVM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Diana.Mvc.ViewComponents;
public class SliderViewComponent : ViewComponent //invoke invikeasync ile cagrmak olr ancaq
{
    DianaDbContext _db { get; }

    public SliderViewComponent(DianaDbContext db)
    {
        _db = db;
    }

    public async Task<IViewComponentResult> InvokeAsync() 
    {
        return View(await _db.Sliders.Select(s => new SliderListItemVM
        {
            Id = s.Id,
            ImageUrl = s.ImageUrl,
            IsLeft = s.IsLeft,
            Text = s.Text,
            Title = s.Title,
        }).ToListAsync());
    }

}
