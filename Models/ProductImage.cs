namespace Diana.Mvc.Models;
public class ProductImage
{
    public int Id { get; set; }
    public string ImageUrl { get; set; }
    public int ProductId { get; set; }
    public Product? Product { get; set; } // butun reference typelerin default qiymeti nulldur ona gore nullable qoyuruq(product tipinden data gondere bilmrk deye model statede ilisir problem yaradir isvalid olur)
    //Prdocutun mutleq 1 sekli olmalidi deye o qalir productuin icerisindeki image propunda ama diger sekilleri ise burada olacaq
}
