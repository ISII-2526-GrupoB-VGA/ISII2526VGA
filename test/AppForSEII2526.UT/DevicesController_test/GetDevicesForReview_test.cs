using AppForMovies.UT;
using AppForSEII2526.API.Controller;
using AppForSEII2526.API.DTOs.DeviceDTOs;
using AppForSEII2526.API.DTOs.ReviewDTOs;
using AppForSEII2526.UT;
using Microsoft.EntityFrameworkCore;



namespace AppForSEII2526.UT.DeviceController_test
{
    public class GetDevicesForReview_test : AppForSEII25264SqliteUT    //Debería ser AppForSEII25264SqliteUT
    {
        public GetDevicesForReview_test()
        {

            var models = new List<Model>
            {
                new Model { Id = 1, NameModel = "iPhone 15" },
                new Model { Id = 2, NameModel = "Galaxy S24" }
            };

            _context.AddRange(models);
            _context.SaveChanges(); // Guardar primero los modelos


            var devices = new List<Device>()
            {

                new Device(1, "Nokia", "yellow", "iPhone 15", 100, 10, 1, 10, 2025) { ModelId = 1 },
                new Device(2, "Samsung", "black", "Galaxy S24", 200, 20, 2, 20, 2024) { ModelId = 2 },

                //Este no me lo va a devolver en las pruebas
                //new Device(3, "No devuelve", "black", "No debe devolver", 0, 0, 0, 0, 2024) { ModelId = 1 }

            };

            _context.AddRange(devices);
            _context.SaveChanges();
        }

        /*      
              [Fact] //Pruebas no parametrizadas
              public async Task GetDevicesForReview_NULL_CustomerCountry()
              {
                  //Arrange (Preparar)
                  var expectedDevices = new List<ReviewItemDTO>
                  {
                      // Device 1: Brand="Nokia", Name="iPhone 15"
                      // Mapeo del controlador: (1, "Nokia", "iPhone 15", "yellow", 2025, "iPhone 15")
                      new ReviewItemDTO(1, "Nokia", "iPhone 15", "yellow", 2025, "iPhone 15"),

                      // Device 2: Brand="Samsung", Name="Galaxy S24"
                      new ReviewItemDTO(2, "Samsung", "Galaxy S24", "black", 2024, "Galaxy S24"),

                      // Device 3: Brand="No devuelve", Name="No debe devolver"
                      //new ReviewItemDTO(3, "No devuelve", "No debe devolver", "black", 2024, "iPhone 15")
                  };

                  var controller = new DeviceController(_context, null);

                  //Act

                  var result = await controller.GetDevicesForReview();

                  //Assert (Comprobar)
                  //Chequeamos que el resultado es OkObjectResult
                  var okResult = Assert.IsType<OkObjectResult>(result);//Comprueba que el resultado es de tipo OkObjectResult
                  //Y obtenemos la lista de Devices
                  var deviceDTOsActual = Assert.IsType<List<ReviewItemDTO>>(okResult.Value);
                  Assert.Equal(expectedDevices, deviceDTOsActual);
              } 

              */

        public static IEnumerable<object[]> TestCasesFor_GetDevicesForReview_OK()
        { //Solo está el caso en el que hay dispositivos y en el que está la lista vacía
            var deviceDTOs = new List<ReviewItemDTO>
            {
                new ReviewItemDTO(1, "Nokia", "iPhone 15", "yellow", 2025, "iPhone 15"),
                new ReviewItemDTO(2, "Samsung", "Galaxy S24", "black", 2024, "Galaxy S24"),
            };

            //Caso 1. 2 dispositivos disponibles
            var esperado1 = new List<ReviewItemDTO>() { deviceDTOs[0], deviceDTOs[1] };

            //No hay dispositivos en la BBDD
            var esperado2 = new List<ReviewItemDTO>() { };



            return new List<object[]>
            {
                new object[] { "Normal", esperado1 }, //Solo devolveria si devuelvo
                //new object[] { "NoDevices", esperado2 }
            };
        }


        //¿Porque uso esta en vez de la de arriba? PREGUNTAR
        [Theory] //Metodo test con parametros
        [Trait("LevelTesting", "Unit Testing")]
        [MemberData(nameof(TestCasesFor_GetDevicesForReview_OK))]   //MemberData es para datos de entrda y salida esperadas
                                                                    //Y especificar fuente dichos parametros
        public async Task GetDevicesForReview_filter_test(string testName, List<ReviewItemDTO> esperado)
        {
            //Arrange
            var controller = new DeviceController(_context, null); //Saco de aquí los datos

            //Act
            var result = await controller.GetDevicesForReview(); //

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result); //¿Para que COJONES sirven estas líneas?
            var actualList = Assert.IsType<List<ReviewItemDTO>>(okResult.Value);

            Assert.Equal(esperado.Count, actualList.Count);

            for (int i = 0; i < esperado.Count; i++)
            {
                Assert.Equal(esperado[i].Id, actualList[i].Id);
                Assert.Equal(esperado[i].Name, actualList[i].Name); //Idea es q compare el objeto entero con uno solo
                Assert.Equal(esperado[i].Brand, actualList[i].Brand); //Usar el equals del DTO reviewDetail
                Assert.Equal(esperado[i].Color, actualList[i].Color);
                Assert.Equal(esperado[i].year, actualList[i].year);
                Assert.Equal(esperado[i].Model, actualList[i].Model);

            }
        }







    }
}
