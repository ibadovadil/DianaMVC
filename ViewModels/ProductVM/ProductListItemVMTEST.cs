using Diana.Mvc.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Diana.Mvc.ViewModels.ProductVM
{
    public class ProductListItemVMTEST
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal SellPrice { get; set; } // satis qiymeti
        public decimal CostPrice { get; set; } //Maya qiymeti
        public float Discout { get; set; }
        public ushort Quantity { get; set; }
        public string ImageUrl { get; set; }
        public Category? Category { get; set; } 
        public bool IsDeleted { get; set; }
    }
}