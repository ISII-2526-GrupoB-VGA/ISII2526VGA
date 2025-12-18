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

        public void SearchDevicesByBrand(string brand)
        {
            WaitForBeingVisible(inputBrand);
            _deviceBrand().Clear();
            _deviceBrand().SendKeys(brand);
            _driver.FindElement(searchDevices).Click();

            Thread.Sleep(2000);
        }

        public void CleanBrand()
        {
            WaitForBeingVisible(inputBrand);
            _deviceBrand().Clear();
        }

        public void CleanYear()
        {
            WaitForBeingVisible(inputYear);
            _deviceYear().Clear();
        }

        public void ClickReviewButton()
        {
            WaitForBeingVisible(DevicesReviewButton);
            var button = _driver.FindElement(DevicesReviewButton);
            
            // Usar JavaScript para hacer click si el elemento no es interactuable normalmente
            if (!button.Displayed || !button.Enabled)
            {
                throw new InvalidOperationException("El botón Review no está disponible para hacer clic");
            }
            
            button.Click();
        }

        public void SearchDevicesByYear(int year)
        {
            WaitForBeingVisible(inputYear);
            _deviceYear().Clear();
            _deviceYear().SendKeys(year.ToString());
            _driver.FindElement(searchDevices).Click();

            Thread.Sleep(2000);
        }

        public void AddDeviceToReview(int deviceId)
        {
            var addButton = By.Id($"deviceToReview_{deviceId}");
            WaitForBeingVisible(addButton);
            _driver.FindElement(addButton).Click();
            Thread.Sleep(300); // Pequeña pausa para que se actualice el UI
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

        public void SeleccionarDevices(List<string> deviceids)
        {
            foreach(var deviceId in deviceids)
            {
                WaitForBeingVisible(By.Id($"deviceToReview_{deviceId}"));
                _driver.FindElement(By.Id($"deviceToReview_{deviceId}")).Click();
                Thread.Sleep(300); // Pequeña pausa entre clicks
            }
        }

        public void ModificarCarrito(int deviceId)
        {
            var removeButton = By.Id($"removeDevice_{deviceId}");
            WaitForBeingVisible(removeButton);
            _driver.FindElement(removeButton).Click();
            Thread.Sleep(500); // Esperar a que se actualice el carrito
        }

        public bool VerElCarrito(int deviceId)
        {
            try
            {
                var removeButton = By.Id($"removeDevice_{deviceId}");
                var button = _driver.FindElement(removeButton);
                return button.Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public bool ReviewDeviceButtonIsDisabled()
        {
            try
            {
                var button = _driver.FindElement(DevicesReviewButton);
                
                // Verificar si el botón está oculto por su contenedor padre
                var parent = button.FindElement(By.XPath(".."));
                if (parent.GetAttribute("hidden") != null)
                {
                    return true; // El contenedor padre está oculto, consideramos el botón como deshabilitado
                }
                
                // Verificar si el botón está deshabilitado o no es interactuable
                return !button.Enabled || !button.Displayed;
            }
            catch (NoSuchElementException)
            {
                return true; // Si no se encuentra, consideramos que está deshabilitado
            }
        }

        public bool IsReviewButtonVisible()
        {
            try
            {
                var button = _driver.FindElement(DevicesReviewButton);
                var parent = button.FindElement(By.XPath(".."));
                
                // Verificar que ni el botón ni su contenedor padre estén ocultos
                return button.Displayed && parent.GetAttribute("hidden") == null;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }



        //Copiada bsta. Leugo borrar
        public void SelectDevices(List<string> deviceids)
        {
            //we wait for till the movies are available to be selected 
            foreach (var deviceId in deviceids)
            {
                WaitForBeingVisible(By.Id($"deviceToReview_{deviceId}"));
                _driver.FindElement(By.Id($"deviceToReview_{deviceId}")).Click();
            }
        }

        public void ModifyReviewCart(string name)
        {
            Thread.Sleep(500);
            WaitForBeingVisible(By.Id($"removeDevice_{name}"));
            _driver.FindElement(By.Id($"removeDevice_{name}")).Click();

        }


        public bool CheckReviewDeviceDisabled()
        {
            //we return true if the button is disabled
            return (_reviewButton().Enabled);
        }

        public bool ReviewButtonExists()
        {
            try
            {
                return _driver.FindElement(By.Id("DevicesReviewButton")).Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }




    }
}