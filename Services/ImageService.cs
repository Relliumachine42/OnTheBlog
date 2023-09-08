using OnTheBlog.Enums;
using OnTheBlog.Services.Interfaces;

namespace OnTheBlog.Services
{
    public class ImageService : IImageService
    {

        private readonly string? _defaultBlogImage = "/img/believingintensifies.jpg";
        private readonly string? _defaultUserImage = "/img/DefaultContactImage.png";
        private readonly string? _defaultCategoryImage = "/img/DefaultContactImage.png";
        private readonly string? _blogAuthorImage = "/img/DefaultContactImage.png";




        public string? ConvertByteArrayToFile(byte[]? fileData, string? extension, DefaultImage defaultImage)
        {
            try
            {
                if (fileData == null || fileData.Length == 0)
                {
                    switch (defaultImage)
                    {
                        case DefaultImage.AuthorImage: return _blogAuthorImage;
                        case DefaultImage.BlogPostImage: return _defaultBlogImage;
                        case DefaultImage.CategoryImage: return _defaultCategoryImage;
                        case DefaultImage.BlogUserImage: return _defaultUserImage;
                    }
                }

                string? imageBase64Data = Convert.ToBase64String(fileData!);
                imageBase64Data = string.Format($"data:{extension};base64,{imageBase64Data}");

                return imageBase64Data;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<byte[]> ConvertFileToByteArrayAsync(IFormFile? file)
        {
            try
            {
                using MemoryStream memoryStream = new MemoryStream();
                await file!.CopyToAsync(memoryStream);
                byte[] byteFile = memoryStream.ToArray();
                memoryStream.Close();

                return byteFile;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
