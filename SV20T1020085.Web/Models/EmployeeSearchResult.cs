using SV20T1020085.DomainModels;

namespace SV20T1020085.Web.Models
{
    public class EmployeeSearchResult : BasePaginationResult
    {
        public List<Employee>? Data { get; set; }
    }
}
