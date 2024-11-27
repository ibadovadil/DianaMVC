using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Diana.Mvc.Models;
public class Product
{
    public int Id { get; set; }
    [MaxLength(64)]
    public string Name { get; set; }
    [MaxLength(128)]
    public string? Description { get; set; }
    [MaxLength(128)]
    public string? About { get; set; }
    [Column(TypeName="smallmoney")]
    public decimal SellPrice { get; set; } // satis qiymeti
    [Column(TypeName = "smallmoney")]
    public decimal CostPrice { get; set; } //Maya qiymeti
    [Range(0, 100)]
    public float Discout { get; set; }
    public ushort Quantity { get; set; }
    public string ImageUrl { get; set; }
    public int CategoryId { get; set; }
    public Category? Category { get; set; } // bu olduqda sql de avtomatik foreign key yaradir
    public  bool  IsDeleted { get; set; } = false;
    public ICollection<ProductColor>? ProductColors { get; set; } // .add isi gore bilerik deye collection yazdiq olmasa enumerablede ola biler
    public ICollection<ProductImage>? ProductImages { get; set; }

}
