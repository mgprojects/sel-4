using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace Sel4Training
{
    public class CountriesAndGeoZonesSorting
    {
        IWebDriver driver;
        string countriesPageUrl = "http://localhost/litecart/admin/?app=countries&doc=countries";
        string geoZonesPageUrl = "http://localhost/litecart/admin/?app=geo_zones&doc=geo_zones";

        [SetUp]
        public void BeforeTest()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        [Test]
        public void CountriesTabTest()
        {
            driver.Navigate().GoToUrl(countriesPageUrl);
            driver.FindElement(By.CssSelector("span input[name='username']")).SendKeys("admin");
            driver.FindElement(By.CssSelector("span input[name='password']")).SendKeys("admin");
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();
            
            // Проверяем сортировку стран
            var countriesTableRows = driver.FindElements(By.CssSelector("form[name=countries_form] tr.row"));
            List<string> countries = new List<string>();
            foreach (var row in countriesTableRows)
                countries.Add(row.FindElement(By.CssSelector("td:nth-of-type(5) a")).Text);

            Assert.That(IsSorted(countries), Is.True, "Список стран отсортирован неверно!");

            // Проверяем сортировку зон, если они есть у страны
            for (int i = 1; i <= countriesTableRows.Count; i++)
            {
                var countryRow = driver.FindElement(By.XPath($"//form[contains(@name, 'countries_form')]//tr[contains(@class, 'row')][{i}]"));
                var zonesCount = countryRow.FindElement(By.CssSelector("td:nth-of-type(6)")).Text;
                if (zonesCount != "0")
                {
                    countryRow.FindElement(By.CssSelector("td a")).Click();

                    var zonesTableRows = driver.FindElements(By.XPath("//table[@id='table-zones']//td/input[contains(@name, 'name')]/.."));
                    List<string> zonesList = new List<string>();
                    foreach (var zone in zonesTableRows)
                        zonesList.Add(zone.Text);
                    zonesList.RemoveAt(zonesList.Count - 1);

                    Assert.That(IsSorted(zonesList), Is.True, "Список зон отсортирован неверно!");

                    driver.FindElement(By.CssSelector("button[name=cancel]")).Click();
                }
            }
        }

        [Test]
        public void GeoZonesTabTest()
        {
            driver.Navigate().GoToUrl(geoZonesPageUrl);
            driver.FindElement(By.CssSelector("span input[name='username']")).SendKeys("admin");
            driver.FindElement(By.CssSelector("span input[name='password']")).SendKeys("admin");
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            var geoZonesTableRows = driver.FindElements(By.CssSelector("form[name=geo_zones_form] tr.row"));
            for (int i = 1; i <= geoZonesTableRows.Count; i++)
            {
                // Заходим внутрь зоны
                var geoZoneName = driver.FindElement(By.XPath($"//form[contains(@name, 'geo_zones_form')]//tr[contains(@class, 'row')][{i}]//td//a"));
                geoZoneName.Click();

                var zones = driver.FindElements(By.CssSelector("form[name=form_geo_zone] tr select[name*=zone_code]"));
                List<string> zonesList = new List<string>();
                foreach (var zone in zones)
                    zonesList.Add(new SelectElement(zone).SelectedOption.Text);

                Assert.That(IsSorted(zonesList), Is.True, "Список зон отсортирован неверно!");

                driver.FindElement(By.CssSelector("button[name=cancel]")).Click();
            }
        }

        [TearDown]
        public void AfterTest()
        {
            driver.Quit();
            driver = null;
        }

        public static bool IsSorted<T>(IEnumerable<T> list) where T : IComparable<T>
        {
            var y = list.First();
            return list.Skip(1).All(x =>
            {
                bool b = y.CompareTo(x) < 0;
                if (!b)
                    TestContext.WriteLine($"Неверная сортировка элементов {y} и {x}");
                y = x;
                return b;
            });
        }
    }
}
