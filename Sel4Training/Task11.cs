using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace Sel4Training
{
    public class UserRegistration
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
        public void UserRegistrationTest()
        {
            driver.Navigate().GoToUrl(mainPageUrl);
            User user = new User();

            // 1. Регистрация
            driver.FindElement(By.CssSelector(".left a[href*=create_account]")).Click();
            var registrationForm = driver.FindElement(By.CssSelector("form[name=customer_form]"));
            registrationForm.FindElement(By.CssSelector("input[name=firstname]")).SendKeys(user.firstName);
            registrationForm.FindElement(By.CssSelector("input[name=lastname]")).SendKeys(user.lastName);
            registrationForm.FindElement(By.CssSelector("input[name=address1]")).SendKeys(user.address);
            registrationForm.FindElement(By.CssSelector("input[name=postcode]")).SendKeys(user.postcode);
            registrationForm.FindElement(By.CssSelector("input[name=city]")).SendKeys(user.city);
            new SelectElement(registrationForm.FindElement(By.CssSelector("select[name=country_code]"))).SelectByValue("US");
            new SelectElement(registrationForm.FindElement(By.CssSelector("select[name=zone_code]"))).SelectByIndex(2);
            registrationForm.FindElement(By.CssSelector("input[name=email]")).SendKeys(user.email);
            registrationForm.FindElement(By.CssSelector("input[name=phone]")).SendKeys(user.phone);
            registrationForm.FindElement(By.CssSelector("input[name=password]")).SendKeys(user.password);
            registrationForm.FindElement(By.CssSelector("input[name=confirmed_password]")).SendKeys(user.password);
            registrationForm.FindElement(By.CssSelector("button[type=submit]")).Click();

            // 2. Логаут
            driver.FindElement(By.CssSelector(".left a[href*=logout]")).Click();

            // 3. Логин
            var loginForm = driver.FindElement(By.CssSelector("form[name=login_form]"));
            loginForm.FindElement(By.CssSelector("input[name=email]")).SendKeys(user.email);
            loginForm.FindElement(By.CssSelector("input[name=password]")).SendKeys(user.password);
            loginForm.FindElement(By.CssSelector("button[name=login]")).Click();

            // 4. Логаут
            driver.FindElement(By.CssSelector(".left a[href*=logout]")).Click();
        }

        [TearDown]
        public void AfterTest()
        {
            driver.Quit();
            driver = null;
        }

        public class User
        {
            public string email { get; set; }
            public string password { get; set; }
            public string firstName { get; set; }
            public string lastName { get; set; }
            public string address { get; set; }
            public string postcode { get; set; }
            public string city { get; set; }
            public string country { get; set; }
            public string phone { get; set; }

            public User()
            {
                Random rand = new Random();
                email = $"testemail_{rand.Next(100000)}{DateTime.Now.Millisecond}@test.ru";
                password = "1111";
                firstName = "Kallistrat";
                lastName = "Maratov";
                address = "1st str, 10-280";
                postcode = "22233";
                city = "Miami";
                country = "USA";
                phone = "+1 29 2929922";
            }
        }
    }
}