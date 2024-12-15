namespace SV21T1020547.Web
{
    public static class StaticResources
    {
        public const string NO_AVT_PATH = "/images/users/no-avatar.jpg";

        public const string NO_IMG_PATH = "/images/no-image.jpg";

        /// <summary>
        /// Lấy đường dẫn đến ảnh đại diện của người dùng
        /// </summary>
        /// <param name="imageName">Tên ảnh</param>
        /// <returns></returns>
        public static string GetUserAvatarPath(string imageName)
        {
            string imagePath = $"\\images\\users\\{imageName}";
            return string.IsNullOrWhiteSpace(imagePath) || !File.Exists($"{ApplicationContext.WebRootPath}{imagePath}")
                ? NO_AVT_PATH : imagePath;
        }

        /// <summary>
        /// Lấy đường dẫn đến ảnh nếu ảnh không tồn tại sẽ trả về ảnh no-image
        /// </summary>
        /// <param name="imageName">Tên ảnh</param>
        /// <param name="folder">Thư mục chưa ảnh</param>
        /// <returns></returns>
        public static string GetImagePath(string imageName, string folder)
        {
            string imagePath = $"\\images\\{folder}\\{imageName}";
            return string.IsNullOrWhiteSpace(imageName) || !File.Exists($"{ApplicationContext.WebRootPath}{imagePath}")
                ? NO_IMG_PATH : imagePath;
        }
    }
}
