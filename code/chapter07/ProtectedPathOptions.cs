using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chapter07
{
    public class ProtectedPathOptions
    {
        public PathString Path { get; set; }
        public string PolicyName { get; set; }
    }
}
