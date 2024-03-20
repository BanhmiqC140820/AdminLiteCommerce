using SV20T1020085.DomainModels;

namespace SV20T1020085.Web.Models
{
    public class ShipperSearchResult:BasePaginationResult
    {
        public List<Shipper>? data { get; set; }
    }
}
