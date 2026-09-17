using System;
using System.Collections.Generic;
using System.Text;

namespace Trusted.Core.Abstraction;

public record Error(string Code, string Message, int? StatusCode)
{
    public static readonly Error None = new(string.Empty, string.Empty, null);
}
