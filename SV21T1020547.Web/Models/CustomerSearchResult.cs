using SV21T1020547.DomainModels;

namespace SV21T1020547.Web.Models
{
    public class CustomerSearchResult : PaginationSearchResult
    {
        public required List<Customer> Data { get; set; }
    }
}
