using System;
using System.IO;
using System.Reflection;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace Sel4Training
{
    public class AddProduct
    {
        IWebDriver driver;
        private WebDriverWait wait;
        string adminUrl = "http://localhost/litecart/admin";

        [SetUp]
        public void BeforeTest()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        [Test]
        public void AddProductTest()
        {
            driver.Navigate().GoToUrl(adminUrl);
            driver.FindElement(By.CssSelector("span input[name='username']")).SendKeys("admin");
            driver.FindElement(By.CssSelector("span input[name='password']")).SendKeys("admin");
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();
            Product product = new Product();
            
            driver.FindElement(By.CssSelector("#box-apps-menu a[href*=catalog]")).Click();
            driver.FindElement(By.CssSelector("a.button[href*=edit_product]")).Click();

            // Заполнение General tab
            var generalTab = driver.FindElement(By.CssSelector("#content .index a[href*=general]"));
            generalTab.Click();
            wait.Until(webDriver => generalTab.GetCssValue("opacity").Equals("1"));
            
            var generalTabContent = driver.FindElement(By.CssSelector("div#tab-general"));
            generalTabContent.FindElement(By.CssSelector("input[name*=name]")).SendKeys(product.name);
            generalTabContent.FindElement(By.CssSelector("input[type=radio][value='1']")).Click();
            generalTabContent.FindElement(By.CssSelector("input[name=code]")).SendKeys(product.code);
            generalTabContent.FindElement(By.CssSelector("input[type=file]")).SendKeys(Environment.CurrentDirectory + product.image);
            generalTabContent.FindElement(By.CssSelector("input[name*=categories][value='2']")).Click();
            generalTabContent.FindElement(By.CssSelector("input[name*=product_groups][value='1-3']")).Click();

            // Заполнение Information tab
            var infoTab = driver.FindElement(By.CssSelector("#content .index a[href*=information]"));
            infoTab.Click();
            wait.Until(webDriver => infoTab.GetCssValue("opacity").Equals("1"));

            var infoTabContent = driver.FindElement(By.CssSelector("div#tab-information"));
            new SelectElement(infoTabContent.FindElement(By.CssSelector("[name=manufacturer_id]"))).SelectByValue("1");
            infoTabContent.FindElement(By.CssSelector(".trumbowyg-editor")).SendKeys(product.description);

            // Заполнение Prices tab
            var pricesTab = driver.FindElement(By.CssSelector("#content .index a[href*=prices]"));
            pricesTab.Click();
            wait.Until(webDriver => pricesTab.GetCssValue("opacity").Equals("1"));

            var pricesTabContent = driver.FindElement(By.CssSelector("div#tab-prices"));
            var price = pricesTabContent.FindElement(By.CssSelector("input[name=purchase_price]"));
            price.Clear();
            price.SendKeys(product.price);
            new SelectElement(pricesTabContent.FindElement(By.CssSelector("select[name=purchase_price_currency_code]"))).SelectByValue("USD");

            // Сохраняем товар
            driver.FindElement(By.CssSelector("button[name=save]")).Click();

            // Проверяем, что отображается в админке
            var createdProduct = driver.FindElement(By.XPath($"//form[@name='catalog_form']//a[text()='{product.name}']"));
            Assert.That(createdProduct.Displayed, Is.True, "Не отображается созданный товар!");
        }

        [TearDown]
        public void AfterTest()
        {
            driver.Quit();
            driver = null;
        }

        public class Product
        {
            public string name { get; set; }
            public string image { get; set; }
            public string description { get; set; }
            public string price { get; set; }
            public string code { get; set; }

            public Product()
            {
                Random rand = new Random();
                name = $"Product {rand.Next(100000)}";
                image = @"\Resources\1.jpg";
                description = "This is new amazing product for people!";
                price = "5.10";
                code = rand.Next(1000000).ToString();
            }
        }
    }
}