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
        private const string deviceBrand2 = "Samsung";
        private const string deviceColor2 = "Black";
        private const int deviceYear2 = 0;
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


        [Theory]
        [InlineData("Elena", "", "Título válido", "Reseña para un dispositivo", 5, "The CustomerCountry field is required.")]
        [InlineData("Elena", "Spain", "Título válido", "Comentario sin q empiece por reseña", 5, "")]
        [InlineData("Elena", "Spain", "", "Reseña para un dispositivo", 5,"The ReviewTitle field is required.")]
        [InlineData("Elena", "Spain", "Título válido", "dispositivo", 5,"")] 
        [InlineData("Elena", "Spain", "Título válido", "Reseña para un dispositivo", 6, "The field Rating must be between 1 and 5.")]
        [InlineData("Elena", "Spain", "Título válido", "Reseña para un dispositivo", 0, "")]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC3_6_7_8_10_11_12_InvalidReviewData_ShowError(
       string username,
        string country,
        string reviewTitle,
        string comment,
        int rating,
        string expectedError)

        {
            // Arrange
            var createReview = new CreateReviewPO(_driver, _output);
            InitialStepsForReviewDevice_UIT();

            // Seleccionar 1 dispositivo
            selectDevices.SeleccionarDevices(new List<string> { deviceId1.ToString() });
            //Thread.Sleep(4000);
            selectDevices.GoToReview();
            //Thread.Sleep(4000);


            // Act
            createReview.FillInReviewInfo(reviewTitle, username, country);
            //Thread.Sleep(4000);
            createReview.AddDeviceReviewComent(deviceId1, comment);
            //Thread.Sleep(4000);
            createReview.AddDeviceReviewRating(deviceId1, rating);
            //Thread.Sleep(4000);
            createReview.PressReviewYourDevices();
            //Thread.Sleep(4000);

            // Assert
            Assert.True(
                
                createReview.CheckValidationError(expectedError),$"Expected error: {expectedError}"

            );
        }


        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC3_9_ModifyDevicesAfterEnteringReviewData()
        {
            // Arrange
            var createReview = new CreateReviewPO(_driver, _output);
            InitialStepsForReviewDevice_UIT();
            //Seleccionamos 2 dispositivos
            selectDevices.SeleccionarDevices(new List<string>{
                deviceId1.ToString(),
                deviceId2.ToString()
            });

            // Vamos a CreateReview
            selectDevices.GoToReview();

            // Rellenamos datos
            string reviewTitle = "Reseña para Apple";
            string customerName = "Elena";
            string country = "Spain";
            string comment = "Reseña para un dispositivo";
            createReview.FillInReviewInfo(reviewTitle, customerName, country);
            createReview.AddDeviceReviewComent(deviceId1, comment);
            createReview.AddDeviceReviewRating(deviceId1, 5);

            createReview.ClickModifyDevices();             // Volvemos atrás

            // Eliminamos Device2
            selectDevices.ModificarCarrito(deviceId2);

            // Volvemos a CreateReview
            selectDevices.GoToReview();

            // Assert
            Assert.True(createReview.IsDeviceInReviewTable(deviceId1));
            Assert.False(createReview.IsDeviceInReviewTable(deviceId2));

            Assert.Equal(reviewTitle, createReview.GetReviewTitle());
            Assert.Equal(country, createReview.GetCustomerCountry());
        }




        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC3_1_CreateReviewSuccessfully()
        {
            var createReview = new CreateReviewPO(_driver, _output);

            InitialStepsForReviewDevice_UIT();

            selectDevices.SeleccionarDevices(new List<string> { deviceId1.ToString() });
            selectDevices.GoToReview();

            createReview.FillInReviewInfo(
                "Título película",
                "alicia@example.com",
                "Spain"
            );

            createReview.AddDeviceReviewComent(
                deviceId1,
                "Reseña para un dispositivo"
            );

            createReview.AddDeviceReviewRating(deviceId1, 5);

            createReview.PressReviewYourDevices();
            Thread.Sleep(1000);

            createReview.ConfirmDialog();
            Thread.Sleep(1000);

            // ASSERT CORRECTO
            Assert.Contains("/review/detailreview", _driver.Url);
        }




        //----------------------EXAMEN----------------------
        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UCExamen_Sprint_3()
        {
            //Arrange
            InitialStepsForReviewDevice_UIT();

            // Act
            //selectDevices.SearchDevicesByBrand(deviceBrand1);

            Thread.Sleep(1000);
            //Añado un elemento
            selectDevices.SeleccionarDevices(new List<string> { deviceId1.ToString() }); //Seleccione el deviec 1
            Thread.Sleep(1000);

            //Filtro por marca
            selectDevices.SearchDevicesByBrand(deviceBrand2);

            Thread.Sleep(1000);
            //Añado otro elemento
            selectDevices.SeleccionarDevices(new List<string> { deviceId2.ToString() }); //Seleccione el deviec 1
            //Quito el primer elemento
            selectDevices.ModificarCarrito(deviceId1);
            Thread.Sleep(1000);

            //Aquí sigo con todo el proceso de hacer la review
            selectDevices.GoToReview();

            Thread.Sleep(1000);

            var createReview = new CreateReviewPO(_driver, _output);

            createReview.FillInReviewInfo(
                "Título película",
                "alicia@example.com",
                "Spain"
            );

            createReview.AddDeviceReviewComent(
                deviceId2,
                "Reseña para un dispositivo"
            );

            Thread.Sleep(1000);

            createReview.AddDeviceReviewRating(deviceId2, 5);

            Thread.Sleep(1000);

            createReview.PressReviewYourDevices();
            Thread.Sleep(1000);

            createReview.ConfirmDialog();
            Thread.Sleep(1000);

            //Assert
            Assert.Contains("/review/detailreview", _driver.Url);
            Thread.Sleep(1000);
        }



    }





}
