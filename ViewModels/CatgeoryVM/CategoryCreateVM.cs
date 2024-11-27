using System.ComponentModel.DataAnnotations;

namespace Diana.Mvc.ViewModels.CatgeoryVM;
public class CategoryCreateVM
{
    [MaxLength(16)]
    public string Name { get; set; }
}
