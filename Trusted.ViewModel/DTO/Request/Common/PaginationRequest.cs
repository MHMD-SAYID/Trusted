using System;
using System.Collections.Generic;
using System.Text;

namespace Trusted.ViewModel.DTO.Request.Common
{
    public class PaginationRequest
    {
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 3;
    }
}
