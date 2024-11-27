using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Diana.Mvc.Models;
public class Category
{
    public int Id { get; set; }
    [MaxLength(16)]
    public string Name { get; set; }
    //[NotMapped] SQL DE CEDVELDE YARATMIR BUNU YAZDIQDA AMA SQLDE PRODUCT ADLI OLMADGINA GORE BURDA VACIB DEYL STRING OLSAYDI VE.S ONDA VACIBDIR
    public IEnumerable<Product>? Products { get; set; }
}
