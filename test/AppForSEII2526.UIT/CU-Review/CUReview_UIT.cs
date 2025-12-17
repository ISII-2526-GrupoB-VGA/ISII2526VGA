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
            Precondition_perform_login();
            Thread.Sleep(1000);

            selectDevices.WaitForBeingVisibleIgnoringExeptionTypes(By.Id("CreateReview"));
            _driver.FindElement(By.Id("CreateReview")).Click();
        }

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC3_FilterDevices_ByBrandAndYear()
        {
            // Arrange
            InitialStepsForReviewDevice_UIT();

            // Act
            selectDevices.SearchDevices(deviceBrand1, deviceYear1);

            // Assert
            Assert.True(selectDevices.IsDeviceInTable(deviceId1));
        }


    }





}
