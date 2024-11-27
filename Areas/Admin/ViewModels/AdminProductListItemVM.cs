using Diana.Mvc.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Diana.Mvc.Areas.Admin.ViewModels;
public class AdminProductListItemVM
{
    public int Id { get; set; }
    [MaxLength(64)]
    public string Name { get; set; }
    [Column(TypeName = "smallmoney")]
    public decimal SellPrice { get; set; } // satis qiymeti
    [Column(TypeName = "smallmoney")]
    public decimal CostPrice { get; set; } //Maya qiymeti
    [Range(0, 100)]
    public float Discout { get; set; }
    public ushort Quantity { get; set; }
    public string ImageUrl { get; set; }
    //public int CategoryId { get; set; } // Category adi lazimdir id si ehtiyac deyil tablede
    public Category? Category { get; set; } // bu olduqda sql de avtomatik foreign key yaradir
    public bool IsDeleted { get; set; }
    public IEnumerable<Color> Colors { get; set; } // add kimi isler gorulmeyecek deye enumerable yazdiq
}