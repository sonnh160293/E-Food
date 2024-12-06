using FoodOnline.Domain.IService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace FoodOnline.Infrastructure.Service
{
    public class ImageService : IImageService
    {

        private readonly IWebHostEnvironment _webHostEnvironment;

        public ImageService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<bool> SaveImagesAsync(List<IFormFile> images, string path, string? defaultName)
        {
            try
            {
                if (images == null || images.Count == 0 || string.IsNullOrEmpty(path))
                {
                    return false;
                }

                // Combine the current directory with the specified path
                string pathImage = Path.Combine(_webHostEnvironment.WebRootPath, path);

                // Create the directory if it doesn't exist
                if (!Directory.Exists(pathImage))
                {
                    Directory.CreateDirectory(pathImage);
                }

                // Iterate over each image in the list
                foreach (var image in images)
                {
                    if (image != null)
                    {
                        // Determine the file path, using the default name if provided
                        string fileName = string.IsNullOrEmpty(defaultName) ? image.FileName : defaultName;
                        string originalPath = Path.Combine(pathImage, fileName);

                        // Use FileStream to save the image asynchronously
                        using (var fileStream = new FileStream(originalPath, FileMode.Create))
                        {
                            await image.CopyToAsync(fileStream);
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Optionally, log the exception here
                return false;
            }

            return true;
        }

    }
}
