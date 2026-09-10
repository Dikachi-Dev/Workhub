using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Workhub.Application.Interfaces.Services;

namespace Workhub.Infrastructure.Services;

public class FileStorageService : IFileStorageService
{
    private readonly string _basePath;

    public FileStorageService(IConfiguration configuration)
    {
        _basePath = configuration["ImagePath"] ?? Path.Combine(AppContext.BaseDirectory, "images");
        if (!Directory.Exists(_basePath))
        {
            try
            {
                Directory.CreateDirectory(_basePath);
            }
            catch
            {
                // Fallback for directory creation permissions
            }
        }
    }

    public async Task<string?> SaveImageAsync(IFormFile? file, string filename, int? resizeWidth = null)
    {
        if (file == null || file.Length == 0)
            return null;

        if (!Directory.Exists(_basePath))
        {
            Directory.CreateDirectory(_basePath);
        }

        string filePath = Path.Combine(_basePath, filename);

        if (resizeWidth.HasValue && resizeWidth.Value > 0)
        {
            using var inputStream = file.OpenReadStream();
            using var image = await Image.LoadAsync(inputStream);

            if (image.Width > resizeWidth.Value)
            {
                int newHeight = (int)((double)image.Height * resizeWidth.Value / image.Width);
                image.Mutate(x => x.Resize(resizeWidth.Value, newHeight));
            }

            await image.SaveAsPngAsync(filePath);
        }
        else
        {
            using var fileStream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(fileStream);
        }

        return GetImageUrl(filename);
    }

    public string GetImageUrl(string? filename)
    {
        if (string.IsNullOrWhiteSpace(filename))
            return string.Empty;

        if (filename.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
            filename.StartsWith("https://", StringComparison.OrdinalIgnoreCase) ||
            filename.StartsWith("/images/", StringComparison.OrdinalIgnoreCase))
        {
            return filename;
        }

        return $"/images/{Path.GetFileName(filename)}";
    }
}
