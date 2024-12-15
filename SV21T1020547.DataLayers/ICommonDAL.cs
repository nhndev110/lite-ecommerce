namespace SV21T1020547.DataLayers
{
    /// <summary>
    /// Định nghĩa các phép xử lý dữ liệu "chung chung"
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICommonDAL<T> where T : class
    {
        /// <summary>
        /// Tìm kiếm và lấy danh sách dữ liệu phân trang
        /// </summary>
        /// <param name="page">Trang cần hiển thị</param>
        /// <param name="pageSize">Số dòng cần hiển thị trên mỗi trang (bằng 0 tức là không phân trang)</param>
        /// <param name="searchValue">Giá trị cần tìm kiếm (chuỗi rỗng tức là lấy toàn bộ dữ liệu)</param>
        /// <returns></returns>
        List<T> List(int page = 1, int pageSize = 0, string searchValue = "");

        /// <summary>
        /// Đếm số lượng dòng dữ liệu tìm được
        /// </summary>
        /// <param name="searchValue">Giá trị cần tìm (chuỗi rỗng nếu đếm toàn bộ dữ liệu)</param>
        /// <returns></returns>
        int Count(string searchValue = "");

        /// <summary>
        /// Lấy 1 dòng dữ liệu dựa vào khóa chính/id (trả về null nếu dữ liệu không tồn tại)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T? Get(int id);

        /// <summary>
        /// Bổ sung một bản ghi vào CSDL. Hàm trả về ID của dữ liệu vừa được bổ sung
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        int Add(T data);

        /// <summary>
        /// Cập nhật dữ liệu
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Update(T data);

        /// <summary>
        /// Xóa một dòng dữ liệu dựa vào giá trị của khóa chính/id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Delete(int id);

        /// <summary>
        /// Kiểm tra xem một dòng dữ liệu có khóa là id hiện có dữ liệu tham chiếu hay không ?
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool InUsed(int id);
    }
}
