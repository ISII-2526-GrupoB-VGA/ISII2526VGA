using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.Shared
{
    public abstract class PageObject
    {
        protected IWebDriver _driver;
        protected readonly ITestOutputHelper _output;

        private readonly By _modalTitle = By.ClassName("modal-title");
        private readonly By _modalBody = By.ClassName("modal-body");
        private readonly By _okModalDialog = By.Id("Button_DialogOK");

        protected PageObject(IWebDriver driver, ITestOutputHelper output)
        {
            _driver = driver;
            _output = output;
        }

        public void InputDateInDatePicker(By datepicker, DateTime date)
        {
            IWebElement webElement = _driver.FindElement(datepicker);

            var action = new Actions(_driver);
            webElement.Clear();
            webElement.Click();

            action.KeyDown(Keys.Left).Perform();
            action.KeyDown(Keys.Left).Perform();
            action.SendKeys(date.ToString("dd")).Perform();

            action.KeyDown(Keys.Left).Perform();
            action.KeyDown(Keys.Left).Perform();
            action.KeyDown(Keys.Right).Perform();
            action.SendKeys(date.ToString("MM")).Perform();

            action.KeyDown(Keys.Right).Perform();
            action.KeyDown(Keys.Right).Perform();
            action.SendKeys(date.ToString("yyyy")).Perform();
        }

        public bool CheckBodyTable(List<string[]> expectedRows, By IdTable)
        {
            string expectedRow, actualRow;
            bool result = true;

            WaitForBeingVisible(IdTable);

            IList<IWebElement> actualrows = _driver
                .FindElement(IdTable)
                .FindElement(By.TagName("tbody"))
                .FindElements(By.TagName("tr"))
                .ToList();

            if (actualrows.Count != expectedRows.Count)
            {
                _output.WriteLine($"Error:\n Expected number of rows:{expectedRows.Count}\n Actual number of rows:{actualrows.Count}");
                return false;
            }

            for (int i = 0; i < expectedRows.Count; i++)
            {
                expectedRow = expectedRows[i][0];
                for (int j = 1; j < expectedRows[i].Length; j++)
                    expectedRow = expectedRow + " " + expectedRows[i][j];

                actualRow = actualrows.Select(m => m.Text).ToList()[i];

                if (!actualRow.StartsWith(expectedRow))
                {
                    _output.WriteLine($"Error:\n\t expected row:{expectedRow}\n\t actual row:{actualRow}");
                    result = false;
                }
            }

            return result;
        }

        public bool CheckModalBodyText(string expectedBody, By modal)
        {
            WaitForBeingVisible(modal);
            var actualBody = _driver.FindElement(_modalBody).Text;
            return actualBody.Contains(expectedBody);
        }

        public bool CheckModalTitleText(string expectedTitle, By modal)
        {
            WaitForBeingVisible(modal);
            var actualTitle = _driver.FindElement(_modalTitle).Text;
            return actualTitle.Contains(expectedTitle);
        }

        public void PressOkModalDialog()
        {
            WaitForBeingVisible(_okModalDialog);
            _driver.FindElement(_okModalDialog).Click();
        }

        public void WaitForBeingClickable(By IdElement)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(30));
            wait.Until(ExpectedConditions.ElementToBeClickable(IdElement));
        }

        public void WaitForBeingVisible(By IdElement)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(30));
            wait.Until(ExpectedConditions.ElementIsVisible(IdElement));
        }

        public void WaitForBeingVisibleIgnoringExeptionTypes(By IdElement)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromMinutes(10));

            wait.IgnoreExceptionTypes(
                typeof(NoSuchElementException),
                typeof(WebDriverTimeoutException),
                typeof(UnhandledAlertException),
                typeof(ElementClickInterceptedException));

            bool notFoundButton = true;
            while (notFoundButton)
            {
                try
                {
                    wait.Until(ExpectedConditions.ElementIsVisible(IdElement));
                    notFoundButton = false;
                }
                catch (ElementClickInterceptedException ex)
                {
                    _output.WriteLine(ex.Message);
                }
            }
        }

        public void WaitForTextToBePresentInElement(By IdElement, string expectedText)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(30));
            IWebElement element = _driver.FindElement(IdElement);
            wait.Until(ExpectedConditions.TextToBePresentInElement(element, expectedText));
        }

        public void ImplicitWait(int seconds) =>
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(seconds);
    }
}
