using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace chapter07
{
    public sealed class LocalIpRequirement : IAuthorizationRequirement
    {
        public const string Name = "LocalIp";
    }
}
