using SV20T1020085.DomainModels;

namespace SV20T1020085.Web.Models
{
    public class SupplierSearchResult:BasePaginationResult
    {
        public List<Supplier>? Data { get; set; }
    }
}
