

using AppForMovies.UT;
using AppForSEII2526.API.Controller;
using AppForSEII2526.API.DTOs.ReviewDTOs;
using AppForSEII2526.API.Models;
using Castle.Core.Resource;
using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace AppForSEII2526.UT.ReviewController_test
{
    public class CreateReview_test : AppForSEII25264SqliteUT
    {

        private const string _CustomerUserName1 = "Guillermo";
        private const string _CustomerUserSurname1 = "Morcillo";
        private const string _DeliveryAddress1 = "Calle Melancolia numero 7";
        private const string _CustomerUserName2 = "Bashar";
        private const string _CustomerUserSurname2 = "AlAssad";
        private const string _Country = "España";
        private const string _ReviewTitle1 = "El imperio romano duró al menos 6 años";
        private const string _ReviewTitle2 = "Nothing ever lasts forever";
        private const string _Custumer1Email = "guillermo.morcillo@alu.uclm.es";
        public DateTime FECHA = DateTime.Today;


        public CreateReview_test()
        {
            // 1. Primero crear y guardar los modelos
            var models = new List<Model>
            {
                new Model(_ReviewTitle1) { Id = 1 },
                new Model(_ReviewTitle2) { Id = 2 }
            };

            _context.AddRange(models);
            _context.SaveChanges(); // Guardar modelos primero

            // 2. Crear y guardar los dispositivos (necesitan ModelId)
            var devices = new List<Device>()
            {
                new Device(1, "Nokia", "Yellow", "NokiaName", 1000, 100, 10, 1, 2025) { ModelId = 1 },
                new Device(2, "Samsung", "Black", "SamsungName", 2000, 200, 20, 2, 2024) { ModelId = 2 }
            };

            _context.AddRange(devices);
            _context.SaveChanges(); // Guardar dispositivos

            // 3. Crear y guardar el usuario ANTES de crear la Review
            ApplicationUser user = new ApplicationUser
            {
                Id = "1",
                FirstName = _CustomerUserName1,
                LastName = _CustomerUserSurname1,
                Address = _DeliveryAddress1,
                Email = _Custumer1Email,
                PhoneNumber = null,
                UserName = _Custumer1Email
            };

            _context.Users.Add(user);
            _context.SaveChanges(); 

            // 4. Ahora sí crear la Review (ya existe el usuario en BD)
            var review = new Review
            {
                DateOfReview = FECHA,
                CustomerCountry = _Country,
                CustomerId = 1,
                ApplicationUserId = "1", // Ahora este FK existe
                OverallRating = 5,
                ReviewTitle = _ReviewTitle1
            };

            review.ReviewItems.Add(new ReviewItem("Comentario de relleno", devices[0].Id, 4, review));

            _context.Add(review);
            _context.SaveChanges();
        }

        public static IEnumerable<object[]> TestCasesFor_CreateReview()
        {
            IList<ReviewItemForCreateDTO> validItems = new List<ReviewItemForCreateDTO>()
            {
                new ReviewItemForCreateDTO
                {
                    DeviceId = 1,
                    Comment = "Comentario correcto",
                    Rating = 4
                }
            };

            //Caso 0: Prueba de antes del examen

            //var reviewEspana = new ReviewForCreateDTO
            //{
            //    ReviewTitle = $"{_Country} Galicia",
            //    CustomerCountry = _Country,
            //    CustomerName = _CustomerUserName1,
            //    ReviewItems = validItems
            //};

            // Caso 1: falta el título (usar null! para suprimir warning)
            var reviewNoTitulo = new ReviewForCreateDTO
            {
                ReviewTitle = null!, // Suprime warning CS8625
                CustomerCountry = _Country,
                CustomerName = _CustomerUserName1,
                ReviewItems = validItems
            };

            // Caso 2: falta el país
            var reviewNoPais = new ReviewForCreateDTO
            {
                ReviewTitle = _ReviewTitle1,
                CustomerCountry = null!, // Suprime warning CS8625
                CustomerName = _CustomerUserName1,
                ReviewItems = validItems
            };

            // Caso 3: lista vacía
            var noDispositivo = new ReviewForCreateDTO
            {
                ReviewTitle = _ReviewTitle1,
                CustomerCountry = _Country, 
                CustomerName = _CustomerUserName1,
                ReviewItems = new List<ReviewItemForCreateDTO>()
            };

            // Caso 4: puntuación incorrecta
            var puntuacionIncorrecta = new ReviewForCreateDTO
            {
                ReviewTitle = _ReviewTitle1,
                CustomerCountry = _Country,
                CustomerName = _CustomerUserName1,
                ReviewItems = new List<ReviewItemForCreateDTO>()
                {
                    new ReviewItemForCreateDTO
                    {
                        DeviceId = 1,
                        Comment = "Puntuacion incorrecta para el comentario",
                        Rating = 999
                    }
                }
            };

            // Caso 5: comentario vacío
            var comentarioVacio = new ReviewForCreateDTO
            {
                ReviewTitle = _ReviewTitle1,
                CustomerCountry = _Country,
                CustomerName = _CustomerUserName1,
                ReviewItems = new List<ReviewItemForCreateDTO>()
                {
                    new ReviewItemForCreateDTO
                    {
                        DeviceId = 1,
                        Comment = null!, // Suprime warning
                        Rating = 3
                    }
                }
            };

            return new List<object[]>
            {
                //new object[] { reviewEspana, "Error. El país debe ser España o una de sus comunidades autónomas" },
                new object[] { reviewNoTitulo, "Error. El título de la reseña es obligatorio (flujo alternativo 3)" },
                new object[] { reviewNoPais, "Error. El país es obligatorio (flujo alternativo 3)" },
                new object[] { noDispositivo, "Error. Debe incluir al menos un dispositivo para reseñar (flujo alternativo 2)" },
                new object[] { puntuacionIncorrecta, "Error! La valoración debe estar entre 1 y 5 (flujo alternativo 5)." },
                new object[] { comentarioVacio, "El comentario es obligatorio" },
            };
        }


        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [MemberData(nameof(TestCasesFor_CreateReview))]
        public async Task CreateRental_Error_test(ReviewForCreateDTO reviewDTO, string errorExpected)
        {
            // Arrange
            var mock = new Mock<ILogger<ReviewController>>();
            ILogger<ReviewController> logger = mock.Object;

            var controller = new ReviewController(_context, logger);

            // Act
            var result = await controller.CreateReview(reviewDTO);

            //Assert
            //we check that the response type is BadRequest and obtain the error returned
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);

            var errorActual = problemDetails.Errors.First().Value[0];

            //we check that the expected error message and actual are the same
            Assert.StartsWith(errorExpected, errorActual);
        }


        [Fact] //Esto test es para cuando le meto que busque una reseña que si existe
        [Trait("LevelTesting", "Unit Testing")] //Eitoso y tal
        //[Trait("Database", "WithoutFixture")]
        public async Task CreateReview_Success_test()
        {
            // Arrange
            var mock = new Mock<ILogger<ReviewController>>();
            ILogger<ReviewController> logger = mock.Object;

            var controller = new ReviewController(_context, logger);

            // Capturar la fecha ANTES de crear la review
            var fechaEsperada = DateTime.Now;

            ReviewForCreateDTO reviewDTO = new ReviewForCreateDTO()
            {
                ReviewTitle = _ReviewTitle2,
                CustomerCountry = _Country,
                CustomerName = _Custumer1Email,
                ReviewItems = new List<ReviewItemForCreateDTO>()
                {
                    new ReviewItemForCreateDTO
                    {
                        DeviceId = 1,
                        Comment = "Comentario válido y correcto",
                        Rating = 5
                    }
                }
            };

            // Act
            var result = await controller.CreateReview(reviewDTO);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var actualReviewDetailDTO = Assert.IsType<ReviewDetailDTO>(createdResult.Value);

            //Crear el objeto esperado CON LA FECHA REAL del resultado
            ReviewDetailDTO expectedReviewDetailDTO = new ReviewDetailDTO(
                id: actualReviewDetailDTO.Id,  // Usar el ID real generado
                dateOfReview: actualReviewDetailDTO.DateOfReview,  
                customerCountry: _Country,
                reviewTitle: _ReviewTitle2,
                overallRating: 5.0,
                reviewItems: new List<ReviewItemDetailDTO>()
                {
                    new ReviewItemDetailDTO(
                        deviceId: 1,
                        deviceName: "NokiaName",
                        deviceModel: _ReviewTitle1,
                        deviceYear: 2025,
                        comment: "Comentario válido y correcto",
                        rating: 5
                    )
                },
                customerName: _Custumer1Email
            );

            Assert.Equal(expectedReviewDetailDTO, actualReviewDetailDTO);
        }

    }
}