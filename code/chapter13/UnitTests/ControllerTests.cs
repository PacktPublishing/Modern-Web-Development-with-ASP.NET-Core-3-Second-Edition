using chapter13.Controllers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using Xunit;

namespace UnitTests
{
    public class ControllerTests
    {
        private static T GetController<T>() where T : Controller, new()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IMyService, MyServiceImplementation>();

            var serviceProvider = services.BuildServiceProvider();

            var controller = new T();

            controller.HttpContext.RequestServices = serviceProvider;

            var values = new Dictionary<string, string> { { "username", "rjperes" }, { "email", "rjperes@hotmail.com" }, { "name", "Ricardo Peres" } };
            var cookies = new RequestCookieCollection(values);

            var request = new Dictionary<string, StringValues>
            {
                { "email", "rjperes@hotmail.com" },
                { "name", "Ricardo Peres" }
            };

            var features = new FeatureCollection();
            features.Set<IMyFeature>(new MyFeatureImplementation());
            features.Set<IRequestCookiesFeature>(new RequestCookiesFeature(cookies));
            features.Set<IFormFeature>(new FormFeature(new FormCollection(request)));

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext(features)
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, "username"),
                        new Claim(ClaimTypes.Role, "Admin")
                    }))
                }
            };

            return controller;
        }

        [Fact]
        public void CanExecuteIndex()
        {
            var controller = new HomeController();
            var result = controller.Index();

            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void CanMock()
        {
            var mock = new Mock<ILogger<HomeController>>();
            //setup an implementation for the Log method
            mock.Setup(x => x.Log(LogLevel.Critical, new EventId(), "", null, null));

            //get the mock
            ILogger<HomeController> logger = mock.Object;
            //call the mocked method with some parameters
            logger.Log(LogLevel.Critical, new EventId(2), "Hello, Moq!", null, null);
        }

        [Fact]
        public void CanMockUser()
        {
            var mock = new Mock<HttpContext>();
            mock.Setup(x => x.User).Returns(new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, "username"), new Claim(ClaimTypes.Role, "Admin") }, CookieAuthenticationDefaults.AuthenticationScheme)));

            var context = mock.Object;
            var user = context.User;

            Assert.NotNull(user);
            Assert.True(user.Identity.IsAuthenticated);
            Assert.True(user.HasClaim(ClaimTypes.Name, "username"));
        }        
    }
}
