using SV20T1020085.DomainModels;

namespace SV20T1020085.Web.Models
{
    public class ProductSearchResult:BasePaginationResult
    {
        public List<Product>? data {get; set;}
    }
}
