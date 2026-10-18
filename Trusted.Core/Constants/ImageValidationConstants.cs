using System;
using System.Collections.Generic;
using System.Text;

namespace Trusted.Core.Constants
{
    public static class ImageValidationConstants
    {
        public const int MaxFileSizeInMB = 6;
        public const int MaxFileSizeInBytes = MaxFileSizeInMB * 1024 * 1024;

        public static readonly string[] AllowedExtensions = [".jpg", ".jpeg", ".png"];
        public static readonly string[] BlockedSignatures = ["4D-5A", "2F-2A", "D0-CF"];
    }
}
