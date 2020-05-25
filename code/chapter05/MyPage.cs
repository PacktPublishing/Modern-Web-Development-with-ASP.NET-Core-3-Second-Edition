using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chapter05
{
    public partial class MyPage : RazorPage<dynamic>, ICustomInitializable
    {
        public MyPage()
        {

        }
        public override Task ExecuteAsync()
        {
            return Task.CompletedTask;
        }

        public void Init(ViewContext context)
        {
            
        }
    }
}
