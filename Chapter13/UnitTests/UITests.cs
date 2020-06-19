using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
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
              
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

                var btn = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Name("btnK")));

                Assert.NotNull(btn);

                btn.Click();
            }
        }
    }
}
