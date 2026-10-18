using System;
using System.Collections.Generic;
using System.Text;

namespace Trusted.Core.Constants
{
    public static class SystemRoles
    {

        
        public const string Mediator = nameof(Mediator);
        public const string Admin = nameof(Admin);
        public const string User = nameof(User);

        public static readonly Dictionary<string, int> Hierarchy = new()
    {
        { User,        0 },
        { Mediator,         1 },
        { Admin,     2 }
    };
    }
}
