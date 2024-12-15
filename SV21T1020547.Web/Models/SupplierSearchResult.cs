using SV21T1020547.DomainModels;

namespace SV21T1020547.Web.Models
{
    public class SupplierSearchResult : PaginationSearchResult
    {
        public required List<Supplier> Data { get; set; }
    }
}
