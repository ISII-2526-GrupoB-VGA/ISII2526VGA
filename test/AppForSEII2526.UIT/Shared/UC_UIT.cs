using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using Xunit.Abstractions;

namespace AppForMovies.UIT.Shared
{
    public class UC_UIT : IDisposable
    {
        private readonly bool _pipeline = false;

        // Browser: "Chrome" | "Firefox" | "Edge"
        private readonly string _browser = "Chrome";

        protected IWebDriver _driver;
        protected readonly ITestOutputHelper _output;

        public string _URI => "https://localhost:7081/";

        public UC_UIT(ITestOutputHelper output)
        {
            _output = output;

            switch (_browser)
            {
                case "Firefox":
                    SetUp_FireFox4UIT();
                    break;
                case "Edge":
                    SetUp_EdgeFor4UIT();
                    break;
                default:
                    SetUp_Chrome4UIT();
                    break;
            }

            _driver.Manage().Window.Maximize();
        }

        protected void Initial_step_opening_the_web_page()
        {
            _driver.Navigate().GoToUrl(_URI);
        }

        protected void Perform_login(string email, string password)
        {
            _driver.Navigate().GoToUrl(_URI + "Account/Login");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(30));
            wait.Until(d => d.FindElements(By.Name("Input.Email")).Count > 0);

            var emailInput = _driver.FindElement(By.Name("Input.Email"));
            emailInput.Clear();
            emailInput.SendKeys(email);

            var passwordInput = _driver.FindElement(By.Name("Input.Password"));
            passwordInput.Clear();
            passwordInput.SendKeys(password);

            // Login submit (evita XPath absoluto)
            IWebElement submitButton;
            try
            {
                // En el template Identity suele existir
                submitButton = _driver.FindElement(By.Id("login-submit"));
            }
            catch (NoSuchElementException)
            {
                // Fallback genérico
                submitButton = _driver.FindElement(By.CssSelector("form button[type='submit']"));
            }

            submitButton.Click();

            // Espera a salir de la página de login (si falla, el test seguirá y fallará donde toque)
            wait.Until(d => !d.Url.Contains("/Account/Login", StringComparison.OrdinalIgnoreCase));
        }

        protected void SetUp_Chrome4UIT()
        {
            var options = new ChromeOptions
            {
                PageLoadStrategy = PageLoadStrategy.Normal,
                AcceptInsecureCertificates = true
            };

            if (_pipeline) options.AddArgument("--headless");

            _driver = new ChromeDriver(options);
        }

        protected void SetUp_FireFox4UIT()
        {
            var options = new FirefoxOptions
            {
                PageLoadStrategy = PageLoadStrategy.Normal,
                AcceptInsecureCertificates = true
            };

            if (_pipeline) options.AddArgument("--headless");

            _driver = new FirefoxDriver(options);
        }

        protected void SetUp_EdgeFor4UIT()
        {
            var options = new EdgeOptions
            {
                PageLoadStrategy = PageLoadStrategy.Normal,
                AcceptInsecureCertificates = true
            };

            if (_pipeline) options.AddArgument("--headless");

            _driver = new EdgeDriver(options);
        }

        public void Dispose()
        {
            try
            {
                _driver?.Quit();
            }
            catch
            {
                // Ignorar errores de cierre del navegador
            }
            finally
            {
                _driver?.Dispose();
                GC.SuppressFinalize(this);
            }
        }
    }
}
