using System.ComponentModel.DataAnnotations;

namespace Diana.Mvc.ViewModels.CatgeoryVM;
public class CategoryUpdateVM
{
    [MaxLength(16)]

    public string Name { get; set; }
}
