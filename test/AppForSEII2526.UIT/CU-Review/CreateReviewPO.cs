using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.CU_Review
{
    public class CreateReviewPO : PageObject
    {
        private By _ReviewTitleBy = By.Id("ReviewTitle");
        private By _CustomerNameBy = By.Id("CustomerName");
        private By _CustomerCountryBy = By.Id("CustomerCountry");
        private By _SubmitBy = By.Id("Submit");

        private IWebElement _reviewTitle() => _driver.FindElement(_ReviewTitleBy);
        private IWebElement _customerName() => _driver.FindElement(_CustomerNameBy);
        private IWebElement _customerCountry() => _driver.FindElement(_CustomerCountryBy);
        private IWebElement _comment(int deviceId) => _driver.FindElement(By.Id($"comment_{deviceId}"));
        private IWebElement _rating(int deviceId) => _driver.FindElement(By.Id($"rating_{deviceId}"));
        private IWebElement _submit() => _driver.FindElement(_SubmitBy);

        public CreateReviewPO(IWebDriver driver, ITestOutputHelper output) :
            base(driver, output)
        {
        }

        public void FillInReviewInfo(string reviewTitle, string customerName, string country)
        {
            WaitForBeingVisible(_ReviewTitleBy);
            
            _reviewTitle().Clear();
            _reviewTitle().SendKeys(reviewTitle);

            WaitForBeingVisible(_CustomerNameBy);
            _customerName().Clear();
            _customerName().SendKeys(customerName);

            WaitForBeingVisible(_CustomerCountryBy);
            _customerCountry().Clear();
            _customerCountry().SendKeys(country);
        }

        public void AddDeviceReviewComent(int deviceId, string comentario)
        {
            var commentBy = By.Id($"comment_{deviceId}");
            WaitForBeingVisible(commentBy);
            _comment(deviceId).Clear();
            _comment(deviceId).SendKeys(comentario);
        }

        public void AddDeviceReviewRating(int deviceId, int rating)
        {
            var ratingBy = By.Id($"rating_{deviceId}");
            WaitForBeingVisible(ratingBy);
            _rating(deviceId).Clear();
            _rating(deviceId).SendKeys(rating.ToString());
        }

        public void PressReviewYourDevices()
        {
                WaitForBeingVisible(_SubmitBy);
            Thread.Sleep(500); // Pequeña pausa para asegurar que el formulario esté listo
            _submit().Click();
        }

        public bool CheckValidationError(string expectedError)
        {
            Thread.Sleep(1000); // Esperar a que se muestre el error de validación
            return _driver.PageSource.Contains(expectedError);
        }

        //public bool IsSubmitButtonDisabled()
        //{
        //    try
        //    {
        //        WaitForBeingVisible(_SubmitBy);
        //        return !_submit().Enabled;
        //    }
        //    catch (NoSuchElementException)
        //    {
        //        return true;
        //    }
        //}

        public void ClickModifyDevices()
        {
            var modifyDevicesBy = By.Id("ModifyDevices");
            WaitForBeingVisible(modifyDevicesBy);
            _driver.FindElement(modifyDevicesBy).Click();
        }

        public bool IsDeviceInReviewTable(int deviceId)
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


        public string GetReviewTitle()
        {
            return _reviewTitle().GetAttribute("value");
        }

        public string GetCustomerCountry()
        {
            return _customerCountry().GetAttribute("value");
        }

        //public void ConfirmDialog()
        //{
        //    // Esperamos a que aparezca el texto del diálogo
        //    WaitForBeingVisible(By.XPath("//button[normalize-space()='Save']"));

        //    var saveButton = _driver.FindElement(
        //        By.XPath("//button[normalize-space()='Save']")
        //    );

        //    saveButton.Click();
        //}

        public void ConfirmDialog()
        {
            var okButtonBy = By.Id("Button_DialogOK");

            WaitForBeingVisible(okButtonBy);
            WaitForBeingClickable(okButtonBy);

            _driver.FindElement(okButtonBy).Click();
        }




    }
}