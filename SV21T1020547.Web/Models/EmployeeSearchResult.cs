using SV21T1020547.DomainModels;

namespace SV21T1020547.Web.Models
{
    public class EmployeeSearchResult : PaginationSearchResult
    {
        public required List<Employee> Data { get; set; }
    }
}
