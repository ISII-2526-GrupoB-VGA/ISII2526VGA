using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.PurchaseDevices
{
    // PO: pantalla "Select devices for purchase"
    public class SelectDevicesForPurchase_PO : PageObject
    {
        private readonly By _deviceNameBy = By.Id("inputName");
        private readonly By _deviceColorBy = By.Id("selectColor");

        private readonly By _searchDevicesBy = By.Id("searchDevices");
        private readonly By _purchaseDevicesButtonBy = By.Id("purchaseDevicesButton");

        private readonly By _tableOfDevicesBy = By.Id("TableOfDevices");
        private readonly By _errorsShownBy = By.Id("ErrorsShown");

        private readonly By _cartTotalBy = By.XPath("//strong[contains(.,'Total:')]");

        private IWebElement _deviceName() => _driver.FindElement(_deviceNameBy);
        private IWebElement _deviceColor() => _driver.FindElement(_deviceColorBy);
        private IWebElement _searchDevicesButton() => _driver.FindElement(_searchDevicesBy);
        private IWebElement _purchaseDevicesButton() => _driver.FindElement(_purchaseDevicesButtonBy);

        public SelectDevicesForPurchase_PO(IWebDriver driver, ITestOutputHelper output)
            : base(driver, output)
        {
        }

        public void FilterDevices(string nameFilter, string colorSelected)
        {
            WaitForBeingVisible(_deviceNameBy);

            _deviceName().Clear();
            _deviceName().SendKeys(nameFilter ?? string.Empty);

            if (string.IsNullOrWhiteSpace(colorSelected)) colorSelected = "All";
            new SelectElement(_deviceColor()).SelectByText(colorSelected);

            _searchDevicesButton().Click();
            System.Threading.Thread.Sleep(2000);
        }

        public void SelectDevices(List<string> deviceNames)
        {
            foreach (var deviceName in deviceNames)
            {
                var addButtonBy = By.Id($"deviceToPurchase_{deviceName}");
                WaitForBeingVisible(addButtonBy);
                _driver.FindElement(addButtonBy).Click();
            }
        }

        public void SelectDevicesByName(List<string> deviceNames) => SelectDevices(deviceNames);

        public void PurchaseDevices()
        {
            WaitForBeingClickable(_purchaseDevicesButtonBy);
            _purchaseDevicesButton().Click();
        }

        public void GoToCreatePurchase() => PurchaseDevices();

        public void RemoveDeviceFromCartById(int deviceId)
        {
            var removeBy = By.Id($"removeDevice_{deviceId}");
            WaitForBeingClickable(removeBy);
            _driver.FindElement(removeBy).Click();
        }

        public bool CheckListOfDevices(List<string[]> expectedDevices)
        {
            return CheckBodyTable(expectedDevices, _tableOfDevicesBy);
        }

        public bool CheckPurchaseDisabledOrNotAvailable()
        {
            try
            {
                return !_purchaseDevicesButton().Displayed || !_purchaseDevicesButton().Enabled;
            }
            catch (NoSuchElementException)
            {
                return true;
            }
        }

        public bool CheckShoppingCartTotalContains(string expectedFragment)
        {
            try
            {
                WaitForBeingVisible(_cartTotalBy);
                return _driver.FindElement(_cartTotalBy).Text.Contains(expectedFragment);
            }
            catch
            {
                return false;
            }
        }

        public bool CheckNoAvailableDevicesMessage(string expectedError)
        {
            return _driver.PageSource.Contains(expectedError);
        }

        public bool CheckErrorsShownContains(string expectedError)
        {
            try
            {
                WaitForBeingVisible(_errorsShownBy);
                return _driver.FindElement(_errorsShownBy).Text.Contains(expectedError);
            }
            catch
            {
                return false;
            }
        }
    }
}
