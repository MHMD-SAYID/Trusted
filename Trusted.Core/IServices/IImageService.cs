using System;
using System.Collections.Generic;
using System.Text;
using Trusted.Core.Abstraction;
using Trusted.ViewModel.DTO.Request.Images;

namespace Trusted.Core.IServices
{
    public interface IImageService
    {
        Task<Result<bool>> UploadManyAsync(List<UploadAccountImagesReq> images, long AccountId, CancellationToken cancellationToken = default);
    }
}
