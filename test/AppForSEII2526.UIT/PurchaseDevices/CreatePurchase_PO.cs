using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.PurchaseDevices
{
    public class CreatePurchase_PO : PageObject
    {
        private By _firstNameBy = By.Id("FirstName");
        private By _lastNameBy = By.Id("LastName");
        private By _deliveryAddressBy = By.Id("DeliveryAddress");
        private By _paymentMethodBy = By.Id("PaymentMethod");

        private By _submitBy = By.Id("Submit");
        private By _modifyDevicesBy = By.Id("ModifyDevices");
        private By _tableOfPurchaseItemsBy = By.Id("TableOfPurchaseItems");

        private By _modalBy = By.Id("DialogOKSaveDelete");
        private By _errorsShownBy = By.Id("ErrorsShown");

        private IWebElement _firstName() => _driver.FindElement(_firstNameBy);
        private IWebElement _lastName() => _driver.FindElement(_lastNameBy);
        private IWebElement _deliveryAddress() => _driver.FindElement(_deliveryAddressBy);
        private IWebElement _paymentMethod() => _driver.FindElement(_paymentMethodBy);

        public CreatePurchase_PO(IWebDriver driver, ITestOutputHelper output)
            : base(driver, output)
        {
        }

        public void FillInPurchaseInfo(string firstName, string lastName, string deliveryAddress, string paymentMethod)
        {
            WaitForBeingVisible(_firstNameBy);

            _firstName().Clear();
            _firstName().SendKeys(firstName);

            _lastName().Clear();
            _lastName().SendKeys(lastName);

            _deliveryAddress().Clear();
            _deliveryAddress().SendKeys(deliveryAddress);

            var selectElement = new SelectElement(_paymentMethod());

            // Si paymentMethod == "" seleccionamos el placeholder (index 0)
            if (string.IsNullOrWhiteSpace(paymentMethod))
                selectElement.SelectByIndex(0);
            else
                selectElement.SelectByText(paymentMethod);
        }

        public void FillInPurchaseDescription(string description, int deviceId)
        {
            var descBy = By.Id("description_" + deviceId);
            WaitForBeingVisible(descBy);

            var input = _driver.FindElement(descBy);
            input.Clear();
            input.SendKeys(description);
        }

        public void FillInPurchaseQuantity(int quantity, int deviceId)
        {
            var qtyBy = By.Id("qty_" + deviceId);
            WaitForBeingVisible(qtyBy);

            var input = _driver.FindElement(qtyBy);
            input.Clear();
            input.SendKeys(quantity.ToString());
        }

        public void PressConfirmPurchase()
        {
            WaitForBeingClickable(_submitBy);
            _driver.FindElement(_submitBy).Click();
        }

        public void PressModifyDevices()
        {
            WaitForBeingClickable(_modifyDevicesBy);
            _driver.FindElement(_modifyDevicesBy).Click();
        }

        public void ConfirmPurchaseInDialog()
        {
            WaitForBeingVisible(_modalBy);
            PressOkModalDialog();
        }

        public void CancelPurchaseInDialog()
        {
            WaitForBeingVisible(_modalBy);
            var cancelBy = By.Id("Button_DialogCancel");
            WaitForBeingClickable(cancelBy);
            _driver.FindElement(cancelBy).Click();
        }

        public bool CheckListOfPurchaseItems(List<string[]> expectedItems)
        {
            return CheckBodyTable(expectedItems, _tableOfPurchaseItemsBy);
        }

        public bool CheckValidationError(string expectedError)
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

        public string GetFirstNameValue()
        {
            WaitForBeingVisible(_firstNameBy);
            return _driver.FindElement(_firstNameBy).GetAttribute("value") ?? string.Empty;
        }

        public string GetLastNameValue()
        {
            WaitForBeingVisible(_lastNameBy);
            return _driver.FindElement(_lastNameBy).GetAttribute("value") ?? string.Empty;
        }

        public string GetDeliveryAddressValue()
        {
            WaitForBeingVisible(_deliveryAddressBy);
            return _driver.FindElement(_deliveryAddressBy).GetAttribute("value") ?? string.Empty;
        }

        public bool CheckPurchaseItemsAtLeastRows(int minRows)
        {
            WaitForBeingVisible(_tableOfPurchaseItemsBy);

            var rows = _driver.FindElement(_tableOfPurchaseItemsBy)
                .FindElement(By.TagName("tbody"))
                .FindElements(By.TagName("tr"));

            return rows.Count >= minRows;
        }

        public bool CheckPurchaseItemsTableHasAtLeastRows(int minRows) => CheckPurchaseItemsAtLeastRows(minRows);
    }
}
