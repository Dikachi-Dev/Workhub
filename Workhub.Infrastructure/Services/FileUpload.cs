using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Workhub.Application.Interfaces.Logger;
using Workhub.Application.Interfaces.Services;

namespace Workhub.Infrastructure.Services
{
    public class FileUpload : IFileUpload
    {
        private readonly Cloudinary cloudinary;
        private readonly ISeriLogger logger;

        public FileUpload(Cloudinary cloudinary, ISeriLogger logger)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            Account account = new Account(
             configuration.GetSection("Cloudinary:Cloud").Value,
              configuration.GetSection("Cloudinary:Apikey").Value,
             configuration.GetSection("Cloudinary:ApiSecret").Value);

            this.cloudinary = new Cloudinary(account);
            this.cloudinary = cloudinary;
            this.logger = logger;
        }

        public async Task<DeletionResult> DeleteImageAsync(string publicId)
        {
            var result = new DeletionResult();
            try
            {

                var img = new DeletionParams(publicId);
                result = await cloudinary.DestroyAsync(img);
                logger.LogInfo($"Delete complete {result.StatusCode}", DateTime.UtcNow);
            }
            catch (Exception ex)
            {
                logger.LogExceptions($"{ex}: {result.StatusCode}", DateTime.UtcNow);
            }
            return result;

        }

        public async Task<ImageUploadResult> UploadImageAsync(IFormFile file)
        {
            var result = new ImageUploadResult();
            if (file.Length > 0)
            {
                try
                {
                    var stream = file.OpenReadStream();
                    var upload = new ImageUploadParams
                    {
                        File = new FileDescription(file.FileName, stream),
                        Transformation = new Transformation().Height(500).Width(500).Crop("fill")
                    };
                    result = await cloudinary.UploadAsync(upload);
                    logger.LogInfo($"Photo upload result: {result.StatusCode}", DateTime.UtcNow);
                }
                catch (Exception ex)
                {
                    logger.LogExceptions($"{ex}Error uploading photo: {file.FileName}", DateTime.UtcNow);
                }

            }
            return result;
        }
    }
}