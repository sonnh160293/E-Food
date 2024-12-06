using Microsoft.AspNetCore.Http;

namespace FoodOnline.Domain.IService
{
    public interface IImageService
    {
        Task<bool> SaveImagesAsync(List<IFormFile> images, string path, string? defaultName);
    }
}
