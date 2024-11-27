using Diana.Mvc.Areas.Admin.ViewModels;
using Diana.Mvc.ViewModels.CommonVM;
using Diana.Mvc.ViewModels.ProductVM;
using Diana.Mvc.ViewModels.SliderVM;

namespace Diana.Mvc.ViewModels.HomeVM;
public class HomeVM
{
    //public IEnumerable<SliderListItemVM> Sliders { get; set; } bunu silirik cunki viewcomponentden gelir
    public IEnumerable<ProductListItemVMTEST> Products { get; set; } //ListItem admin ucun ayri user ucun yari olmalidi problem yasanarsa admini getir ama yasanmazda bunu islet getsin 
    public PaginationVM<IEnumerable<ProductListItemVMTEST>> PaginatedProducts { get; set; }
}
