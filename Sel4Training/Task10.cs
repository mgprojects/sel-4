using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;

namespace Sel4Training
{
    public class OpenProduct
    {
        IWebDriver driver;
        private WebDriverWait wait;
        string mainPageUrl = "http://localhost/litecart/";

        [SetUp]
        public void BeforeTest()
        {
            driver = new FirefoxDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        }

        [Test]
        public void OpenProductTest()
        {
            driver.Navigate().GoToUrl(mainPageUrl);

            /*
             * Главная страница с товарами
             */

            var product = driver.FindElement(By.CssSelector("#box-campaigns ul li:first-of-type"));
            var productName = product.FindElement(By.CssSelector(".name")).Text; // имя товара
            var fullPrice = product.FindElement(By.CssSelector(".regular-price"));
            var fullPriceText = fullPrice.Text; // полная цена
            var discountPrice = product.FindElement(By.CssSelector(".campaign-price"));
            var discountPriceText = discountPrice.Text; // цена со скидкой

            // Проверяем полную цену на главной: серая и зачёркнутая
            var fullPriceColor = fullPrice.GetCssValue("color");
            Assert.Multiple(() =>
            {
                var rgba = fullPriceColor.Substring(fullPriceColor.IndexOf('(') + 1).TrimEnd(')').Split(',');
                var r = rgba[0].Trim();
                var g = rgba[1].Trim();
                var b = rgba[2].Trim();

                string error = "Цвет полной цены не серый!";
                Assert.That(r, Is.EqualTo(g), error);
                Assert.That(r, Is.EqualTo(b), error);
                Assert.That(g, Is.EqualTo(b), error);
            });
            Assert.That(fullPrice.TagName, Is.EqualTo("s"), "Полная цена не зачёркнута!");
            
            // Проверяем скидочную цену на главной: красная и жирная
            var discountPriceColor = discountPrice.GetCssValue("color");
            Assert.Multiple(() =>
            {
                var rgba = discountPriceColor.Substring(fullPriceColor.IndexOf('(') + 1).TrimEnd(')').Split(',');
                var g = rgba[1].Trim();
                var b = rgba[2].Trim();

                string error = "Цвет скидочной цены не красный!";
                Assert.That(g, Is.EqualTo("0"), error);
                Assert.That(b, Is.EqualTo("0"), error);
            });
            Assert.That(discountPrice.TagName, Is.EqualTo("strong"), "Скидочная цена не жирная!");

            // Проверяем, что акционная цена крупнее, чем обычная
            double fullPriceFontSize = Convert.ToDouble(fullPrice.GetCssValue("font-size").Replace("px", "").Replace('.', ','));
            double discountPriceFontSize = Convert.ToDouble(discountPrice.GetCssValue("font-size").Replace("px", "").Replace('.', ','));
            Assert.That(discountPriceFontSize, Is.GreaterThan(fullPriceFontSize), "Размер шрифта акционной цены меньше, чем полной цены!");

            product.Click(); // переходим на страницу товара

            /*
             * Страница открытого товара
             */

            var productOpened = driver.FindElement(By.CssSelector("#box-product"));
            var productOpenedName = productOpened.FindElement(By.CssSelector("[itemprop=name]")).Text;
            // Проверяем совпадение имени товара на главной и на странице товара
            Assert.That(productName, Is.EqualTo(productOpenedName), "Имя товара на главной не совпадает с именем на странице товара!");

            // Проверяем полную цену на странице товара: серая и зачёркнутая
            fullPrice = productOpened.FindElement(By.CssSelector(".regular-price"));
            fullPriceColor = fullPrice.GetCssValue("color");
            Assert.Multiple(() =>
            {
                var rgba = fullPriceColor.Substring(fullPriceColor.IndexOf('(') + 1).TrimEnd(')').Split(',');
                var r = rgba[0].Trim();
                var g = rgba[1].Trim();
                var b = rgba[2].Trim();

                string error = "Цвет полной цены не серый!";
                Assert.That(r, Is.EqualTo(g), error);
                Assert.That(r, Is.EqualTo(b), error);
                Assert.That(g, Is.EqualTo(b), error);
            });
            Assert.That(fullPrice.TagName, Is.EqualTo("s"), "Полная цена не зачёркнута!");

            // Проверяем скидочную цену на странице товара: красная и жирная
            discountPrice = productOpened.FindElement(By.CssSelector(".campaign-price"));
            discountPriceColor = discountPrice.GetCssValue("color");
            Assert.Multiple(() =>
            {
                var rgba = discountPriceColor.Substring(fullPriceColor.IndexOf('(') + 1).TrimEnd(')').Split(',');
                var g = rgba[1].Trim();
                var b = rgba[2].Trim();

                string error = "Цвет скидочной цены не красный!";
                Assert.That(g, Is.EqualTo("0"), error);
                Assert.That(b, Is.EqualTo("0"), error);
            });
            Assert.That(discountPrice.TagName, Is.EqualTo("strong"), "Скидочная цена не жирная!");

            // Проверяем, что акционная цена крупнее, чем обычная
            fullPriceFontSize = Convert.ToDouble(fullPrice.GetCssValue("font-size").Replace("px", "").Replace('.', ','));
            discountPriceFontSize = Convert.ToDouble(discountPrice.GetCssValue("font-size").Replace("px", "").Replace('.', ','));
            Assert.That(discountPriceFontSize, Is.GreaterThan(fullPriceFontSize), "Размер шрифта акционной цены меньше, чем полной цены!");

            var fullPriceOpenedText = fullPrice.Text;
            var discountPriceOpenedText = discountPrice.Text;
            
            // Проверяем, что цены на главной и на странице товара совпадают
            Assert.That(fullPriceOpenedText, Is.EqualTo(fullPriceText), "Полная цена товара на главной не совпадает с полной ценой на странице товара!");
            Assert.That(discountPriceOpenedText, Is.EqualTo(discountPriceText), "Скидочная цена товара на главной не совпадает со скидочной ценой на странице товара!");

        }

        [TearDown]
        public void AfterTest()
        {
            driver.Quit();
            driver = null;
        }
    }
}
