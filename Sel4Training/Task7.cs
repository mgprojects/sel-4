using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace Sel4Training
{
    public class AdminNavigation
    {
        IWebDriver driver;
        private WebDriverWait wait;
        string adminUrl = "http://localhost/litecart/admin/";

        [SetUp]
        public void BeforeTest()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        }

        [Test]
        public void AdminNavigationTest()
        {
            driver.Navigate().GoToUrl(adminUrl);
            driver.FindElement(By.CssSelector("span input[name='username']")).SendKeys("admin");
            driver.FindElement(By.CssSelector("span input[name='password']")).SendKeys("admin");
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            var menuElements = driver.FindElements(By.CssSelector("#sidebar #box-apps-menu > li"));
            for (int i = 1; i <= menuElements.Count; i++)
            {
                // Открываем пункт основного меню
                var menuItem = driver.FindElement(By.CssSelector($"#sidebar #box-apps-menu > li:nth-of-type({i})"));
                wait.Until(ExpectedConditions.ElementToBeClickable(menuItem)).Click();

                // Проверяем отображение заголовка h1
                var header = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("h1")));
                Assert.That(header.Displayed, Is.True, "Не отображается заголовок!");

                var subMenuElements = driver.FindElements(By.CssSelector("#sidebar #box-apps-menu > li li"));
                for (int j = 1; j <= subMenuElements.Count; j++)
                {
                    // Открываем пункт подменю
                    var subMenuItem = driver.FindElement(By.CssSelector($"#sidebar #box-apps-menu > li li:nth-of-type({j})"));
                    wait.Until(ExpectedConditions.ElementToBeClickable(subMenuItem)).Click();

                    // Проверяем отображение заголовка h1
                    var subHeader = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("h1")));
                    Assert.That(subHeader.Displayed, Is.True, "Не отображается заголовок для подменю!");
                }
            }
        }

        [TearDown]
        public void AfterTest()
        {
            driver.Quit();
            driver = null;
        }
    }
}
