using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.CU_Review
{
    public class SelectDeviceForReviewPO : PageObject
    {
        private By inputBrand = By.Id("inputBrand");
        private By inputYear = By.Id("inputYear");
        private By searchDevices = By.Id("searchDevices");
        private By _ShowReviewCartBy = By.Id("showReviewCart");
        private By _reviewButtonBy = By.Id("DevicesReviewButton");
        private By TableOfDevices = By.Id("TableOfDevices");
        private By DevicesReviewButton = By.Id("DevicesReviewButton");

        private IWebElement _deviceBrand() => _driver.FindElement(inputBrand);
        private IWebElement _deviceYear() => _driver.FindElement(inputYear);
        private IWebElement _searchDevices() => _driver.FindElement(searchDevices);
        private IWebElement _TableOfDevices() => _driver.FindElement(TableOfDevices);
        private IWebElement _DevicesReviewButton() => _driver.FindElement(DevicesReviewButton);
        private IWebElement _showReviewCartButton() => _driver.FindElement(_ShowReviewCartBy);
        private IWebElement _reviewButton() => _driver.FindElement(_reviewButtonBy);

        public SelectDeviceForReviewPO(IWebDriver driver, ITestOutputHelper output) :
            base(driver, output)
        {
        }

        public void SearchDevices(string brand, int year)
        {
            //WaitForBeingVisible(inputBrand);
            //_deviceBrand().Clear();
            //_deviceBrand().SendKeys(brand);

            //WaitForBeingVisible(inputYear);
            //_deviceYear().Clear();
            //_deviceYear().SendKeys(year.ToString());

            //_searchDevices().Click();

            //System.Threading.Thread.Sleep(2000);

            WaitForBeingVisible(inputBrand);
            _deviceBrand().SendKeys(brand);
            _driver.FindElement(searchDevices).Click();

            WaitForBeingVisible(inputYear);
            _deviceYear().SendKeys(year.ToString());
            _driver.FindElement(searchDevices).Click();

            System.Threading.Thread.Sleep(2000);

        }

        public void AddDeviceToReview(int deviceId)
        {
            var addButton = By.Id($"deviceToReview_{deviceId}");
            WaitForBeingVisible(addButton);
            _driver.FindElement(addButton).Click();
        }

        public void GoToReview()
        {
            WaitForBeingVisible(DevicesReviewButton);
            _DevicesReviewButton().Click();
        }

        public bool IsDeviceInTable(int deviceId)
        {
            var deviceRow = By.Id($"DeviceData_{deviceId}");
            try
            {
                return _driver.FindElement(deviceRow).Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
    }
}