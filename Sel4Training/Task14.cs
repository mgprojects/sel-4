using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace Sel4Training
{
    public class LinksOpenedInNewWindow
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
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        [Test]
        public void LinksOpenedInNewWindowTest()
        {
            driver.Navigate().GoToUrl(adminUrl);
            driver.FindElement(By.CssSelector("span input[name='username']")).SendKeys("admin");
            driver.FindElement(By.CssSelector("span input[name='password']")).SendKeys("admin");
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            driver.Navigate().GoToUrl("http://localhost/litecart/admin/?app=countries&doc=countries");

            driver.FindElement(By.CssSelector("a.button[href*=edit_country]")).Click();

            var links = driver.FindElements(By.CssSelector("#content td a:not([href='#'])"));
            foreach (var lnk in links)
            {
                var windowsBefore = driver.WindowHandles;
                var currentWindow = driver.CurrentWindowHandle;
                lnk.Click();
                var newWindow = wait.Until(d => anyWindowOtherThan(windowsBefore));
                driver.SwitchTo().Window(newWindow);
                Assert.That(driver.Title, Is.Not.Null.Or.Empty, "Новое окно не открылось!");
                driver.Close();
                driver.SwitchTo().Window(currentWindow);
            }
        }

        [TearDown]
        public void AfterTest()
        {
            driver.Quit();
            driver = null;
        }

        public string anyWindowOtherThan(ReadOnlyCollection<string> oldWindows)
        {
            wait.Until(d => d.WindowHandles.ToList().Count > oldWindows.Count);
            var handles = driver.WindowHandles.ToList();
            var newWindow = handles.Except(oldWindows).First();
            return newWindow.Any() ? newWindow : null;
        }
    }
}