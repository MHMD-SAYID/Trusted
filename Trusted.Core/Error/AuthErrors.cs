using System;
using System.Collections.Generic;
using System.Text;
using Trusted.Core.Abstraction;
using Trusted.Core.Entities;

namespace Trusted.Core
{
    public static class AuthErrors
    {
        public static readonly Error DuplicatedEmail = new("User.DuplicatedEmail", "A user with this email already exists.", 400);
    }
}
