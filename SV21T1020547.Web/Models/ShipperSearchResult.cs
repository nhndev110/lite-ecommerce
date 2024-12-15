using SV21T1020547.DomainModels;

namespace SV21T1020547.Web.Models
{
    public class ShipperSearchResult : PaginationSearchResult
    {
        public required List<Shipper> Data { get; set; }
    }
}
