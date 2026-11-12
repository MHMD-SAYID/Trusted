using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using Trusted.Core.Abstraction;
using Trusted.Core.Constants;
using Trusted.Core.Entities.Games;
using Trusted.Core.IRepositories;
using Trusted.Core.IServices;
using Trusted.ViewModel.DTO.Request.Images;

namespace Trusted.Application.Services
{
    public class ImageService : IImageService
    {
        private readonly string _imagesPath;
        private readonly IImageRepository _imageRepository;
        public ImageService(IWebHostEnvironment env, IImageRepository imageRepository)
        {
            // WebRootPath can be null if web root isn't set; fall back to {ContentRoot}/wwwroot
            var webRoot = env.WebRootPath;
            if (string.IsNullOrWhiteSpace(webRoot))
            {
                webRoot = Path.Combine(env.ContentRootPath, "wwwroot");
            }

            _imagesPath = Path.Combine(webRoot, "Images");
            Directory.CreateDirectory(_imagesPath);
            _imageRepository = imageRepository;

        }

        public static bool IsAllowedExtension(IFormFile image)
        {
            var extension = Path.GetExtension(image.FileName.ToLower());
            return ImageValidationConstants.AllowedExtensions.Contains(extension);
        }

        public static bool IsAllowedSize(IFormFile image)
        {
            return image.Length <= ImageValidationConstants.MaxFileSizeInBytes;
        }

        public static bool HasSafeSignature(IFormFile image)
        {
            using var reader = new BinaryReader(image.OpenReadStream());
            var bytes = reader.ReadBytes(2);
            var signature = BitConverter.ToString(bytes);
            return !ImageValidationConstants.BlockedSignatures.Contains(signature);
        }

     

        public async Task<Result<bool>> UploadManyAsync(List<UploadAccountImagesReq> images, long AccountId, CancellationToken cancellationToken = default)

        {
            List<Image> uploadedFiles = [];

            if (images.Count(i => i.IsDefault) > 1)
            {
                return new Result<bool>
                {
                    IsSuccess = false,
                    EnRes = "Only one image can be set as default.",
                    ArRes = "يمكن تعيين صورة واحدة فقط كافتراضية.",
                    Data = false,
                    StatusCode = 400,
                    Errors = new List<string> { "Multiple default images found." }
                };
            }
            if (images.Count(i => i.IsDefault) < 1)
            {
                return new Result<bool>
                {
                    IsSuccess = false,
                    EnRes = "At least one image must be set as default.",
                    ArRes = "يجب تعيين صورة واحدة على الأقل كافتراضية.",
                    Data = false,
                    StatusCode = 400,
                    Errors = new List<string> { "No default image found." }
                };
            }



            foreach (var image in images)
            {

                if (!IsAllowedExtension(image.Image))
                {

                    return new Result<bool>
                    {
                        IsSuccess = false,
                        EnRes = "File extension is not allowed.",
                        ArRes = "امتداد الملف غير مسموح به.",
                        Data = false,
                        StatusCode = 400,
                        Errors = new List<string> { "Invalid file extension." }
                    };
                }
                //Validate Size
                if (!IsAllowedSize(image.Image))
                {
                    return new Result<bool>
                    {
                        IsSuccess = false,
                        EnRes = $"File size exceeds the allowed limit of {ImageValidationConstants.MaxFileSizeInMB}MB.",
                        ArRes = $"يتجاوز حجم الملف الحد المسموح به وهو {ImageValidationConstants.MaxFileSizeInMB} ميجابايت.",
                        Data = false,
                        StatusCode = 400,
                        Errors = new List<string> { "File is too large." }
                    };
                }
                //validate signature
                if (!HasSafeSignature(image.Image))
                {
                    return new Result<bool>
                    {
                        IsSuccess = false,
                        EnRes = "This file type is blocked for security reasons.",
                        ArRes = "تم حظر نوع الملف هذا لأسباب أمنية.",
                        Data = false,
                        StatusCode = 400,
                        Errors = new List<string> { "Blocked file content." }
                    };
                }
                var uploadedFile = await SaveFile(image.Image, AccountId, image.IsDefault, cancellationToken);
                uploadedFiles.Add(uploadedFile);
            }

            await _imageRepository.AddRangeAsync(uploadedFiles);
            await _imageRepository.CommitAsync();

            return new Result<bool>
            {
                IsSuccess = true

            };
        }

        private async Task<string> SavePhysicalFile(IFormFile image, string folderName, long entityId, CancellationToken cancellationToken = default)
        {
            var imageName = string.Concat(Guid.NewGuid(), Path.GetExtension(image.FileName));

            var folderPath = Path.Combine(_imagesPath, folderName, entityId.ToString());

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var physicalPath = Path.Combine(folderPath, imageName);

            using var stream = File.Create(physicalPath);
            await image.CopyToAsync(stream, cancellationToken);

            return $"/Images/{folderName}/{entityId}/{imageName}";
        }

        private async Task<Image> SaveFile(IFormFile image, long AccountId, bool IsDefault, CancellationToken cancellationToken = default, string folderName = "Account")
        {
            var path = await SavePhysicalFile(image, folderName, AccountId, cancellationToken);

            var accountImage = new Image
            {
                AccountId = AccountId,
                Path = path,
                IsDefault = IsDefault
            };

            return accountImage;
        }

        private async Task<string> SaveUserFile(IFormFile image, long UserId, CancellationToken cancellationToken = default, string folderName = "ProfilePics")
        {
            var folderPath = Path.Combine(_imagesPath, folderName, UserId.ToString());
            if (Directory.Exists(folderPath))
            {
                var existingFiles = Directory.GetFiles(folderPath);
                foreach (var file in existingFiles)
                {
                    try { File.Delete(file); } catch { /* ignore */ }
                }
            }

            return await SavePhysicalFile(image, folderName, UserId, cancellationToken);
        }

        public async Task<Result<string>> UploadProfilePicAsync(IFormFile image, long UserId, CancellationToken cancellationToken = default)
        {
            if (!IsAllowedExtension(image))
            {
                return new Result<string>
                {
                    IsSuccess = false,
                    EnRes = "File extension is not allowed.",
                    ArRes = "امتداد الملف غير مسموح به.",
                    Data = null,
                    StatusCode = 400,
                    Errors = new List<string> { "Invalid file extension." }
                };
            }

            // Validate Size
            if (!IsAllowedSize(image))
            {
                return new Result<string>
                {
                    IsSuccess = false,
                    EnRes = $"File size exceeds the allowed limit of {ImageValidationConstants.MaxFileSizeInMB}MB.",
                    ArRes = $"يتجاوز حجم الملف الحد المسموح به وهو {ImageValidationConstants.MaxFileSizeInMB} ميجابايت.",
                    Data = null,
                    StatusCode = 400,
                    Errors = new List<string> { "File is too large." }
                };
            }

            // Validate signature
            if (!HasSafeSignature(image))
            {
                return new Result<string>
                {
                    IsSuccess = false,
                    EnRes = "This file type is blocked for security reasons.",
                    ArRes = "تم حظر نوع الملف هذا لأسباب أمنية.",
                    Data = null,
                    StatusCode = 400,
                    Errors = new List<string> { "Blocked file content." }
                };
            }

            var profilePicPath = await SaveUserFile(image, UserId, cancellationToken, "ProfilePics");

            return new Result<string>
            {
                IsSuccess = true,
                StatusCode = 200,
                ArRes = "تم رفع الصورة بنجاح",
                EnRes = "Image uploaded successfully",
                Data = profilePicPath
            };
        }
    }
}

