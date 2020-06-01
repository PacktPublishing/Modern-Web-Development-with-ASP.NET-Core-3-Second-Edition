using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace UnitTests
{
    public class UITests
    {
        [Fact]
        public void CanFindSearchButton()
        {
            using (var driver = (IWebDriver)new ChromeDriver(Environment.CurrentDirectory))
            {
                driver
                    .Navigate()
                    .GoToUrl("http://www.google.com");

                var elm = driver.FindElement(By.Name("q"));

                Assert.NotNull(elm);

                elm.SendKeys("asp.net");

                var btn = driver.FindElement(By.Name("btnK"));

                Assert.NotNull(btn);

                btn.Click();

            }
        }
    }
}
