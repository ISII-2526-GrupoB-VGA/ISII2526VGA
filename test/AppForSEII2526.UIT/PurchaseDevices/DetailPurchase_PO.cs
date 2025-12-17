using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.PurchaseDevices
{
    public class DetailPurchase_PO : PageObject
    {
        private By _customerFullNameBy = By.Id("CustomerFullName");
        private By _deliveryAddressBy = By.Id("DeliveryAddress");
        private By _purchaseDateBy = By.Id("PurchaseDate");
        private By _totalQuantityBy = By.Id("TotalQuantity");
        private By _totalPriceHeaderBy = By.Id("TotalPriceHeader");

        private By _purchasedDevicesTableBy = By.Id("PurchasedDevices");

        public DetailPurchase_PO(IWebDriver driver, ITestOutputHelper output)
            : base(driver, output)
        {
        }

        public bool CheckPurchaseDetail(
            string expectedCustomerFullName,
            string expectedDeliveryAddress,
            DateTime expectedPurchaseDate,
            string expectedTotalPriceFragment,
            string expectedTotalQuantityFragment)
        {
            WaitForBeingVisible(_totalPriceHeaderBy);

            bool result = true;

            result = result && _driver.FindElement(_customerFullNameBy).Text.Contains(expectedCustomerFullName);
            result = result && _driver.FindElement(_deliveryAddressBy).Text.Contains(expectedDeliveryAddress);
            result = result && _driver.FindElement(_totalPriceHeaderBy).Text.Contains(expectedTotalPriceFragment);
            result = result && _driver.FindElement(_totalQuantityBy).Text.Contains(expectedTotalQuantityFragment);

            var actualPurchaseDate = DateTime.Parse(_driver.FindElement(_purchaseDateBy).Text);
            result = result && ((actualPurchaseDate - expectedPurchaseDate).Duration() < TimeSpan.FromMinutes(1));

            return result;
        }

        public bool CheckListOfPurchasedDevices(List<string[]> expectedItems)
        {
            return CheckBodyTable(expectedItems, _purchasedDevicesTableBy);
        }
    }
}
