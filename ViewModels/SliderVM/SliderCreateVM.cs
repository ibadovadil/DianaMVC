using System.ComponentModel.DataAnnotations;

namespace Diana.Mvc.ViewModels.SliderVM;
public class SliderCreateVM
{
    [Required]
    public string ImageUrl { get; set; }
    [Required, MinLength(3), MaxLength(64, ErrorMessage = "Text length must be bewteen 3 to 64 symbol"), DataType("nvarchar")]
    public string Title { get; set; }
    [Required, MinLength(3), DataType("nvarchar"), MaxLength(32, ErrorMessage = "Text length must be between 3 to 32 symbol")]
    public string Text { get; set; }
    public sbyte Position { get; set; }
}
