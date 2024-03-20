using SV20T1020085.DomainModels;

namespace SV20T1020085.Web.Models
{
    public class ProductSelected
    {
        public Product? product { get; set; }
        public List<ProductPhoto>? productPhotos { get; set; }
        public List<ProductAttribute>? attributes { get; set; }
    }
}
