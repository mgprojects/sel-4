using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace Sel4Training
{
    [TestFixture]
    public class OpenPage
    {
        IWebDriver driver;
        WebDriverWait wait;

        [SetUp]
        public void BeforeTest()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        }

        [Test]
        public void OpenPageTest()
        {
            driver.Navigate().GoToUrl("http://github.com");
            wait.Until(ExpectedConditions.TitleContains("GitHub"));
        }

        [TearDown]
        public void AfterTest()
        {
            driver.Quit();
            driver = null;
        }
    }
}
