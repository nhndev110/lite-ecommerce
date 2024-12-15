using SV21T1020547.DomainModels;

namespace SV21T1020547.Web.Models
{
    public class OrderSearchResult : PaginationSearchResult
    {
        public int Status { get; set; } = 0;
        public string TimeRange { get; set; } = "";
        public required List<Order> Data { get; set; }
    }
}
