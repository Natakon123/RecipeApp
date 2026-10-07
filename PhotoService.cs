namespace RecipeApp
{
    /// <summary>Saves / deletes recipe photos under wwwroot/uploads</summary>
    public class PhotoService
    {
        public const long MaxBytes = 5 * 1024 * 1024; // 5 MB
        static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

        readonly IWebHostEnvironment _env;

        public PhotoService(IWebHostEnvironment env)
        {
            _env = env;
        }

        /// <summary>Returns an error message, or null when the file is acceptable</summary>
        public static string? Validate(IFormFile? file)
        {
            if (file is null || file.Length == 0) return null;
            if (file.Length > MaxBytes) return "Photo must be 5 MB or smaller.";
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(ext)) return "Photo must be a .jpg, .png, .gif or .webp image.";
            if (!file.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase)) return "File is not an image.";
            return null;
        }

        /// <returns>Relative web path such as /uploads/xxxx.jpg</returns>
        public async Task<string> SaveAsync(IFormFile file)
        {
            var error = Validate(file);
            if (error != null) throw new InvalidOperationException(error);

            var folder = Path.Combine(_env.WebRootPath, "uploads");
            Directory.CreateDirectory(folder);

            var fileName = Guid.NewGuid().ToString("N") + Path.GetExtension(file.FileName).ToLowerInvariant();
            await using var stream = File.Create(Path.Combine(folder, fileName));
            await file.CopyToAsync(stream);
            return "/uploads/" + fileName;
        }

        public void Delete(string? photoPath)
        {
            if (string.IsNullOrEmpty(photoPath) || !photoPath.StartsWith("/uploads/")) return;
            var fullPath = Path.Combine(_env.WebRootPath, "uploads", Path.GetFileName(photoPath));
            if (File.Exists(fullPath)) File.Delete(fullPath);
        }
    }
}
