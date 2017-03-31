using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace Sel4Training
{
    public class Cart
    {
        IWebDriver driver;
        private WebDriverWait wait;
        string mainPageUrl = "http://localhost/litecart/";

        [SetUp]
        public void BeforeTest()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        [Test]
        public void CartTest()
        {
            driver.Navigate().GoToUrl(mainPageUrl);

            // Добавляем 3 товара в корзину
            for (int i = 0, j = 1; i < 3; i++)
            {
                driver.FindElement(By.CssSelector(".content .product:first-of-type")).Click();
                if (driver.FindElements(By.CssSelector("select[name='options[Size]']")).Count > 0)
                    new SelectElement(driver.FindElement(By.CssSelector("select[name='options[Size]']"))).SelectByIndex(1);
                driver.FindElement(By.CssSelector("button[name=add_cart_product]")).Click();
                var cartItemsCount = driver.FindElement(By.CssSelector("div#cart .quantity"));
                wait.Until(webDriver => cartItemsCount.Text.Equals($"{j}"));

                driver.FindElement(By.CssSelector("#logotype-wrapper")).Click();
                j++;
            }

            driver.FindElement(By.CssSelector("#cart a.link")).Click();

            // Удаляем 3 товара из корзины
            var totalCartItems = driver.FindElements(By.CssSelector("#box-checkout-cart li.item")).Count;
            for (int i = 0; i < totalCartItems; i++)
            {
                var totalItemsInCartBefore = driver.FindElements(By.CssSelector("#order_confirmation-wrapper tr:not(.header) .item")).Count;
                driver.FindElement(By.CssSelector("button[name=remove_cart_item]")).Click();
                wait.Until(webDriver => webDriver.FindElements(By.CssSelector("#order_confirmation-wrapper tr:not(.header) .item")).Count < totalItemsInCartBefore);
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