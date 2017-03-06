using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace Sel4Training
{
    public class AdminLogin
    {
        IWebDriver driver;
        string adminUrl = "http://localhost/litecart/admin/";

        [SetUp]
        public void BeforeTest()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        [Test]
        public void AdminLoginTest()
        {
            driver.Navigate().GoToUrl(adminUrl);
            driver.FindElement(By.CssSelector("span input[name='username']")).SendKeys("admin");
            driver.FindElement(By.CssSelector("span input[name='password']")).SendKeys("admin");
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();
            Assert.That(driver.FindElement(By.CssSelector(".logotype")).Displayed, Is.True, "Не смогли залогиниться админом!");
        }

        [TearDown]
        public void AfterTest()
        {
            driver.Quit();
            driver = null;
        }
    }
}
