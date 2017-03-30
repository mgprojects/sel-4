using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Sel4Training
{
    public class ProductsStickers
    {
        IWebDriver driver;
        string mainPageUrl = "http://localhost/litecart/";

        [SetUp]
        public void BeforeTest()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        [Test]
        public void ProductsStickersTest()
        {
            driver.Navigate().GoToUrl(mainPageUrl);

            var products = driver.FindElements(By.CssSelector(".middle > .content .box li")); // все товары
            foreach (var item in products)
            {
                var stickers = item.FindElements(By.CssSelector(".sticker")); // все стикеры товара
                Assert.That(stickers.Count + 1, Is.EqualTo(1), $"Неверное количество стикеров у товара '{item.Text}'");
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
