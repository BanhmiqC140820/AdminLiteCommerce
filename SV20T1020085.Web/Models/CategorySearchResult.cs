using SV20T1020085.DomainModels;

namespace SV20T1020085.Web.Models
{
    public class CategorySearchResult : BasePaginationResult
    {
        public List<Category>? Data { get; set; }
    }
}
