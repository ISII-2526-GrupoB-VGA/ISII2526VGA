using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Xunit;
using Xunit.Abstractions;
using AppForMovies.UIT.Shared;

namespace AppForSEII2526.UIT.PurchaseDevices
{
    public class UCPurchaseDevices_UIT : UC_UIT
    {
        public UCPurchaseDevices_UIT(ITestOutputHelper output) : base(output)
        {
            // abre la web y crea el PageObject principal
            Initial_step_opening_the_web_page();
            selectDevices = new SelectDevicesForPurchase_PO(_driver, _output);
        }

        private const string customerEmail = "alicia@example.com";
        private const string customerPassword = "P@ssw0rd!";

        private const string deviceName_iPhoneRow = "iPhone 15 Base";
        private const string deviceName_GalaxyRow = "Galaxy S24";

        private const string filterName_iPhone = "iPhone 15";
        private const string filterColor_Blue = "Blue";

        private const string expectedBrand_Apple = "Apple";
        private const string expectedModel_iPhone15 = "iPhone 15";
        private const string expectedPrice_799 = "799";

        private const string expectedModel_GalaxyS24 = "Galaxy S24";

        private const string firstName_Alicia = "Alicia";
        private const string lastName_Perez = "Pérez";
        private const string deliveryAddress_CMayor = "C/ Mayor 12, Albacete";
        private const string paymentMethod_PayPal = "PayPal";
        private const string giftDescription = "Regalo Navidad";

        private const string err_MissingDelivery = "Debes introducir una dirección de entrega.";
        private const string err_MissingFullName = "Debes introducir un nombre completo.";
        private const string err_MissingPaymentMethod = "Debes introducir un método de pago";
        private const string err_MissingQuantity = "Debes introducir una cantidad";

        private readonly SelectDevicesForPurchase_PO selectDevices;

        private void Precondition_perform_login()
        {
            // login con usuario seed
            Perform_login(customerEmail, customerPassword);
        }

        private void EnsureCartEmpty()
        {
            // vacía el carrito para que las pruebas no se contaminen entre sí
            var removeButtons = _driver.FindElements(By.CssSelector("button[id^='removeDevice_']"));
            while (removeButtons.Count > 0)
            {
                removeButtons[0].Click();
                System.Threading.Thread.Sleep(250);
                removeButtons = _driver.FindElements(By.CssSelector("button[id^='removeDevice_']"));
            }
        }

        private void InitialStepsForPurchaseDevices_UIT()
        {
            // login + navega a selección de dispositivos
            Precondition_perform_login();
            _driver.Navigate().GoToUrl(_URI + "purchase/SelectDevForPurchase");

            // espera la tabla y deja el carrito limpio
            selectDevices.WaitForBeingVisibleIgnoringExeptionTypes(By.Id("TableOfDevices"));
            EnsureCartEmpty();
        }

        private void RemoveCartItemByContainsText(string containsText)
        {
            // elimina un elemento del carrito buscando un fragmento en el texto del botón remove
            var buttons = _driver.FindElements(By.CssSelector("button[id^='removeDevice_']"));
            if (buttons.Count == 0)
                throw new NoSuchElementException("No cart items to remove.");

            foreach (var b in buttons)
            {
                if (b.Text.Contains(containsText, StringComparison.OrdinalIgnoreCase))
                {
                    b.Click();
                    return;
                }
            }

            throw new NoSuchElementException($"No cart item button contains '{containsText}'.");
        }

        private int GetFirstPurchaseItemDeviceId()
        {
            // obtiene el DeviceID de la primera fila (PurchaseItem_{DeviceID})
            var row = _driver.FindElement(By.CssSelector("tr[id^='PurchaseItem_']"));
            var id = row.GetAttribute("id");
            if (string.IsNullOrWhiteSpace(id) || !id.StartsWith("PurchaseItem_"))
                throw new InvalidOperationException("Cannot read PurchaseItem_{DeviceID} row id.");

            return int.Parse(id.Replace("PurchaseItem_", ""));
        }

        // UC1_1
        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC1_1_Esc1_BasicFlow_PurchaseSuccessful()
        {
            var createPurchase = new CreatePurchase_PO(_driver, _output);
            var detailPurchase = new DetailPurchase_PO(_driver, _output);

            InitialStepsForPurchaseDevices_UIT();

            // añade móvil al carrito
            selectDevices.SelectDevicesByName(new List<string> { deviceName_iPhoneRow });

            // pasa a crear compra
            selectDevices.GoToCreatePurchase();

            // obtiene DeviceID para escribir descripción
            var deviceId = GetFirstPurchaseItemDeviceId();

            // rellena datos de compra
            createPurchase.FillInPurchaseInfo(firstName_Alicia, lastName_Perez, deliveryAddress_CMayor, paymentMethod_PayPal);
            createPurchase.FillInPurchaseDescription(giftDescription, deviceId);

            // confirma compra (modal OK)
            createPurchase.PressConfirmPurchase();
            createPurchase.ConfirmPurchaseInDialog();

            // valida detalle de compra
            Assert.True(
                detailPurchase.CheckPurchaseDetail(
                    $"{firstName_Alicia} {lastName_Perez}",
                    deliveryAddress_CMayor,
                    DateTime.Now,
                    expectedPrice_799,
                    "1"),
                "El resumen de la compra no coincide con lo esperado.");

            // valida tabla de dispositivos comprados
            var expectedItems = new List<string[]>
            {
                new string[] { expectedBrand_Apple, expectedModel_iPhone15, filterColor_Blue, expectedPrice_799 + " €", "1", giftDescription }
            };

            Assert.True(detailPurchase.CheckListOfPurchasedDevices(expectedItems),
                "Los dispositivos comprados no coinciden con lo esperado.");
        }

        // UC1_2
        [Fact(Skip = "No se puede hacer porque siempre hay un dispositivo disponible debido al seed.")]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC1_2_Esc2_NoDevicesAvailable_ShowsWarning()
        {
        }

        // UC1_3
        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC1_3_Esc3_FA1_FilterByName_ShowsOnlyMatchingDevices()
        {
            var expected = new List<string[]>
            {
                new string[] { deviceName_iPhoneRow, expectedBrand_Apple, expectedModel_iPhone15, filterColor_Blue, expectedPrice_799 }
            };

            InitialStepsForPurchaseDevices_UIT();

            // filtra por nombre
            selectDevices.FilterDevices(filterName_iPhone, "");

            // valida resultados del filtro
            Assert.True(selectDevices.CheckListOfDevices(expected));
        }

        // UC1_4
        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC1_4_Esc3_FA1_FilterByColor_ShowsOnlyBlueDevices()
        {
            var expected = new List<string[]>
            {
                new string[] { deviceName_iPhoneRow, expectedBrand_Apple, expectedModel_iPhone15, filterColor_Blue, expectedPrice_799 }
            };

            InitialStepsForPurchaseDevices_UIT();

            // filtra por color
            selectDevices.FilterDevices("", filterColor_Blue);

            // valida resultados del filtro
            Assert.True(selectDevices.CheckListOfDevices(expected));
        }

        // UC1_5
        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC1_5_Esc4_FA3_RemoveDeviceFromCart_UpdatesTotal()
        {
            InitialStepsForPurchaseDevices_UIT();

            // añade dos móviles al carrito
            selectDevices.SelectDevicesByName(new List<string> { deviceName_iPhoneRow, deviceName_GalaxyRow });

            // comprueba total del carrito
            Assert.True(selectDevices.CheckShoppingCartTotalContains("1498"));
            Assert.True(selectDevices.CheckShoppingCartTotalContains("2 items"));

            // elimina el móvil del carrito
            RemoveCartItemByContainsText(expectedModel_GalaxyS24);

            // comprueba total actualizado
            Assert.True(selectDevices.CheckShoppingCartTotalContains(expectedPrice_799));
            Assert.True(selectDevices.CheckShoppingCartTotalContains("1 items"));
        }

        // UC1_6
        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC1_6_Esc5_FA4_PurchaseNotAvailable_WhenCartIsEmpty()
        {
            InitialStepsForPurchaseDevices_UIT();

            // comprueba que no se puede continuar si el carrito está vacío
            Assert.True(selectDevices.CheckPurchaseDisabledOrNotAvailable());
        }

        // UC1_7
        [Theory]
        [InlineData(firstName_Alicia, lastName_Perez, "", paymentMethod_PayPal, err_MissingDelivery)]
        [InlineData("", lastName_Perez, deliveryAddress_CMayor, paymentMethod_PayPal, err_MissingFullName)]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC1_7_Esc6_FA5_MandatoryDataMissing_ShowsError(
            string firstName, string lastName, string deliveryAddress, string paymentMethod, string expectedError)
        {
            var createPurchase = new CreatePurchase_PO(_driver, _output);

            InitialStepsForPurchaseDevices_UIT();

            // añade móvil al carrito
            selectDevices.SelectDevicesByName(new List<string> { deviceName_iPhoneRow });

            // pasa a crear compra
            selectDevices.GoToCreatePurchase();

            // rellena datos (con campo obligatorio vacío según el caso)
            createPurchase.FillInPurchaseInfo(firstName, lastName, deliveryAddress, paymentMethod);

            // intenta confirmar
            createPurchase.PressConfirmPurchase();

            // comprueba error mostrado
            Assert.True(createPurchase.CheckErrorsShownContains(expectedError));
        }

        // UC1_8
        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC1_8_Esc6_FA5_MissingPaymentMethod_ShowsError()
        {
            var createPurchase = new CreatePurchase_PO(_driver, _output);

            InitialStepsForPurchaseDevices_UIT();

            // añade móvil al carrito
            selectDevices.SelectDevicesByName(new List<string> { deviceName_iPhoneRow });

            // pasa a crear compra
            selectDevices.GoToCreatePurchase();

            // rellena datos con método de pago válido
            createPurchase.FillInPurchaseInfo(firstName_Alicia, lastName_Perez, deliveryAddress_CMayor, paymentMethod_PayPal);

            // deja el método de pago vacío
            var paymentSelect = new SelectElement(_driver.FindElement(By.Id("PaymentMethod")));
            paymentSelect.SelectByValue("");

            // intenta confirmar
            createPurchase.PressConfirmPurchase();

            // comprueba error mostrado
            Assert.True(createPurchase.CheckErrorsShownContains(err_MissingPaymentMethod));
        }

        // UC1_9
        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC1_9_Esc6_FA5_MissingQuantity_ShowsError()
        {
            var createPurchase = new CreatePurchase_PO(_driver, _output);

            InitialStepsForPurchaseDevices_UIT();

            // añade móvil al carrito
            selectDevices.SelectDevicesByName(new List<string> { deviceName_iPhoneRow });

            // pasa a crear compra
            selectDevices.GoToCreatePurchase();

            // obtiene DeviceID para modificar cantidad
            var deviceId = GetFirstPurchaseItemDeviceId();

            // rellena datos de compra
            createPurchase.FillInPurchaseInfo(firstName_Alicia, lastName_Perez, deliveryAddress_CMayor, paymentMethod_PayPal);

            // pone cantidad inválida
            createPurchase.FillInPurchaseQuantity(0, deviceId);

            // intenta confirmar
            createPurchase.PressConfirmPurchase();

            // comprueba error mostrado
            Assert.True(createPurchase.CheckErrorsShownContains(err_MissingQuantity));
        }

        // UC1_10
        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC1_10_Esc7_FA6_ModifySelectedDevices_KeepsCustomerData()
        {
            var createPurchase = new CreatePurchase_PO(_driver, _output);

            InitialStepsForPurchaseDevices_UIT();

            // añade móvil al carrito
            selectDevices.SelectDevicesByName(new List<string> { deviceName_iPhoneRow });

            // pasa a crear compra
            selectDevices.GoToCreatePurchase();

            // rellena datos de compra
            createPurchase.FillInPurchaseInfo(firstName_Alicia, lastName_Perez, deliveryAddress_CMayor, paymentMethod_PayPal);

            // vuelve a seleccionar dispositivos
            createPurchase.PressModifyDevices();

            // añade otro móvil al carrito
            selectDevices.WaitForBeingVisibleIgnoringExeptionTypes(By.Id("TableOfDevices"));
            selectDevices.SelectDevicesByName(new List<string> { deviceName_GalaxyRow });

            // regresa a crear compra
            selectDevices.GoToCreatePurchase();

            // comprueba que se mantienen los datos introducidos
            Assert.Equal(firstName_Alicia, createPurchase.GetFirstNameValue());
            Assert.Equal(lastName_Perez, createPurchase.GetLastNameValue());
            Assert.Equal(deliveryAddress_CMayor, createPurchase.GetDeliveryAddressValue());

            // comprueba que hay al menos 2 items en la tabla
            Assert.True(createPurchase.CheckPurchaseItemsTableHasAtLeastRows(2));
        }

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void ExamenSprint3()
        {
            var createPurchase = new CreatePurchase_PO(_driver, _output);
            var detailPurchase = new DetailPurchase_PO(_driver, _output);

            InitialStepsForPurchaseDevices_UIT();

            selectDevices.SelectDevicesByName(new List<string> { deviceName_GalaxyRow });//1
            selectDevices.FilterDevices(filterName_iPhone, "");//2
            selectDevices.SelectDevicesByName(new List<string> { deviceName_iPhoneRow });//3
            RemoveCartItemByContainsText(expectedModel_GalaxyS24);//4
            selectDevices.GoToCreatePurchase();//5
            var deviceId = GetFirstPurchaseItemDeviceId();
            createPurchase.FillInPurchaseInfo(firstName_Alicia, lastName_Perez, deliveryAddress_CMayor, paymentMethod_PayPal);
            createPurchase.FillInPurchaseDescription(giftDescription, deviceId);
            createPurchase.PressConfirmPurchase();
            createPurchase.ConfirmPurchaseInDialog();
            Assert.True(detailPurchase.CheckPurchaseDetail(
                $"{firstName_Alicia} {lastName_Perez}",
                deliveryAddress_CMayor,
                DateTime.Now,
                "799",
                "1"));

        }

        

    }
}
