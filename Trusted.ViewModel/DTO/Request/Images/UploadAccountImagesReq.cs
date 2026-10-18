
using Microsoft.AspNetCore.Http;

namespace Trusted.ViewModel.DTO.Request.Images;

public class UploadAccountImagesReq
{
  
    public IFormFile Image { get; set; }
    public bool IsDefault { get; set; }
}
