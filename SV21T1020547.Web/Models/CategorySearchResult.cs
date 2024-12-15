using SV21T1020547.DomainModels;

namespace SV21T1020547.Web.Models
{
    public class CategorySearchResult : PaginationSearchResult
    {
        public required List<Category> Data { get; set; }
    }
}
