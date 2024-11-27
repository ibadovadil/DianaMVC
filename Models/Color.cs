using System.ComponentModel.DataAnnotations;

namespace Diana.Mvc.Models;
public class Color
{
    public int Id { get; set; }
    [MaxLength(32)]
    public string Name { get; set; }
    [MinLength(3) , MaxLength(6)]
    public string Hexcode { get; set; }
    public ICollection<ProductColor>? ProductColor { get; set; } // add isi gore bilerik deye collection yazdiq olmasa enumerablede ola biler
}
