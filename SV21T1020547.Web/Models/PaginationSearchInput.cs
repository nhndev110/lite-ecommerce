namespace SV21T1020547.Web.Models
{
    /// <summary>
    ///     <para>
    ///         Page: trang hiện tại
    ///     </para>
    ///     <para>
    ///         PageSize: số lượng kết quả trong 1 trang
    ///     </para>
    ///     <para>
    ///         SearchValue: giá trị tìm kiếm
    ///     </para>
    /// </summary>
    public class PaginationSearchInput
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; }
        public string SearchValue { get; set; } = "";
    }
}
