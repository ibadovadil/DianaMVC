using System.ComponentModel.DataAnnotations;

namespace Diana.Mvc.ViewModels.ColorVM;
public class ColorCreateVM
{
    [Required , MaxLength(32)]
    public string Name { get; set; }
    //[DataType(DataType.)]
    [Required , MaxLength(7) , MinLength(7)]
    public string Hexcode { get; set; }
}
