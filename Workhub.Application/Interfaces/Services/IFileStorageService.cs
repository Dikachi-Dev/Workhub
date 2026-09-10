using Microsoft.AspNetCore.Http;

namespace Workhub.Application.Interfaces.Services;

public interface IFileStorageService
{
    Task<string?> SaveImageAsync(IFormFile? file, string filename, int? resizeWidth = null);
    string GetImageUrl(string? filename);
}
