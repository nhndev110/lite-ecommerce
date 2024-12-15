using SV21T1020547.DomainModels;

namespace SV21T1020547.DataLayers
{
    public interface IUserAccountDAL
    {
        /// <summary>
        /// Kiểm tra xem tên đăng nhập và mật khẩu có đúng hay không ?
        /// Nếu đúng trả về thông tin của User, nếu sai trả về null
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        UserAccount? Authorize(string username, string password);

        /// <summary>
        /// Đổi mật khẩu
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        bool ChangePassword(string username, string password);
    }
}
