using Diana.Mvc.Models;

namespace Diana.Mvc.ViewModels.ProductVM;
public class ProductDetailVM
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string SellPrice { get; set; } //DECIMAL OLMALIDIR AMA QIYMETI YUVARLAQ ETMEK UCUN TOSTRING METHODU ISITFADE ETDIM CONTROLLERDE ONA GORE HELELEIK STRING SAXLADIQ 
    public float Discout { get; set; }
    public ushort Quantity { get; set; }
    public string ImageUrl { get; set; }
    public IEnumerable<string> ImageUrls { get; set; }
    public Category? Category { get; set; }
    public string Description { get; set; }
    public string About { get; set; }
    public IEnumerable<Color> Colors { get; set; }  // yalniz adi lazim olarsa ienum<string> colors bize hex coduda lazmdir deye bu cur yazdiq
}
