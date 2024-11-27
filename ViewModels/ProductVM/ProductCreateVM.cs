using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Diana.Mvc.ViewModels.ProductVM;

public class ProductCreateVM
{
    [MaxLength(64)]
    public string Name { get; set; }
    [MaxLength(128)]
    public string? Description { get; set; }
    [MaxLength(128)]
    public string? About { get; set; }
    [Column(TypeName = "smallmoney")]
    public decimal SellPrice { get; set; } // satis qiymeti
    [Column(TypeName = "smallmoney")]
    public decimal CostPrice { get; set; } //Maya qiymeti
    [Range(0, 100)]
    public float Discout { get; set; }
    public ushort Quantity { get; set; }
    public IFormFile ImageFile { get; set; } //single image
    public IEnumerable<IFormFile>? Images { get; set; } //multi image  //public IFormFileCollection Images { get; set; } da olar oz methodlari var bu methodlari bu projectde istifade etmeyeceyik deye ehtiyac yoxdu ienumearble daha yunguldur istifade etmeliyiikk!!
    public int CategoryId { get; set; }
    public IEnumerable<int> ColorIds { get; set; }
}
