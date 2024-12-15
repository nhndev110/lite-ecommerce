using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace SV21T1020547.Web
{
    /// <summary>
    /// Những thông tin cần dùng để biểu diễn/mô tả "danh tính" của người dùng
    /// </summary>
    public class WebUserData
    {
        public string UserID { get; set; } = "";
        public string UserName { get; set; } = "";
        public string DisplayName { get; set; } = "";
        public string Photo { get; set; } = "";
        public List<string> Roles { get; set; } = [];

        /// <summary>
        /// Tạo ra chứng nhận để ghi nhận danh tính của người dùng
        /// </summary>
        /// <returns></returns>
        public ClaimsPrincipal CreatePrincipal()
        {
            // Tạo danh sách các Claim, mỗi Claim lưu giữ 1 thông tin trong danh tính của người dùng
            List<Claim> claims = [
                new Claim(nameof(UserID), UserID),
                new Claim(nameof(UserName), UserName),
                new Claim(nameof(DisplayName), DisplayName),
                new Claim(nameof(Photo), Photo),
            ];
            if (Roles != null)
            {
                foreach (var role in Roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }

            // Tạo Identity
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // Tạo Principal (giấy chứng nhân)
            var principal = new ClaimsPrincipal(identity);

            return principal;
        }
    }
}
