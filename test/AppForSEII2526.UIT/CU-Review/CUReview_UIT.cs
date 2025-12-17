using AppForMovies.UIT.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AppForSEII2526.UIT.CU_Review
{
    public class UCReview_UIT : UC_UIT
    {
        private SelectDeviceForReviewPO selectDevices;
        public UCReview_UIT(ITestOutputHelper output) : base(output)
        {
            Initial_step_opening_the_web_page();
            selectDevices = new SelectDeviceForReviewPO(_driver, _output);
        }

        private const int deviceId1 = 1;
        private const string deviceName1 = "Apple";
        private const string deviceBrand1 = "Apple";
        private const string deviceColor1 = "Blue";
        private const int deviceYear1 = 0;
        private const string deviceModel1 = "iPhone 15";

        private const int deviceId2 = 2;
        private const string deviceName2 = "Samsung";
        private const string deviceBrand2 = "Galaxy S24";
        private const string deviceColor2 = "Black";
        private const int deviceYear2 = 2024;
        private const string deviceModel2 = "Galaxy S24";

        private void Precondition_perform_login()
        {
            Perform_login("alicia@example.es", "Password1234%"); //Cambiar contraseña
        }

        private void InitialStepsForReviewDevice_UIT()
        {
            
            Thread.Sleep(1000);

            selectDevices.WaitForBeingVisibleIgnoringExeptionTypes(By.Id("CreateReview"));
            _driver.FindElement(By.Id("CreateReview")).Click();
        }

        [Fact] //Con esto compruebo que el filtro de busqueda de marca funciona correctamente
        [Trait("LevelTesting", "Functional Testing")]
        public void UC3_2FilterDevices_ByBrand()
        {
            // Arrange
            InitialStepsForReviewDevice_UIT();

            // Act
            selectDevices.SearchDevicesByBrand(deviceBrand1);

            // Assert
            Assert.True(selectDevices.IsDeviceInTable(deviceId1));

            Thread.Sleep(1000);
        }

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC3_3FilterDevices_ByYear()
        {
            // Arrange
            InitialStepsForReviewDevice_UIT();

            // Act
            selectDevices.SearchDevicesByYear(deviceYear1);

            // Assert
            Assert.True(selectDevices.IsDeviceInTable(deviceId1));

            Thread.Sleep(1000);
        }



        [Fact]
        [Trait("LevelTesting", "Funcional Testing")] //Esta es la de eliminar 1 dispositivo
        public void UC3_4_ModifySelectDevices()
        {
            //Arrange
            //Act
            InitialStepsForReviewDevice_UIT();

            selectDevices.SeleccionarDevices(new List<string> { deviceId1.ToString(), deviceId2.ToString() });
            Thread.Sleep(1000);
            selectDevices.ModificarCarrito(deviceId2);


            //Assert            
            Assert.True(selectDevices.VerElCarrito(deviceId1));
        }

        

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC3_5_ReviewButtonNotAvailable_WhenCartIsEmpty()
        {
            // Arrange
            InitialStepsForReviewDevice_UIT();

            // Act
            // Añadimos un dispositivo
            selectDevices.SeleccionarDevices(new List<string> { deviceId1.ToString() });
            Thread.Sleep(500);

            // Lo eliminamos → carrito vacío
            selectDevices.ModificarCarrito(deviceId1);
            Thread.Sleep(500);

            // Assert
            Assert.False(
                selectDevices.ReviewButtonExists(),
                "Review button should NOT be available when the cart is empty"
            );
        }




    }





}
