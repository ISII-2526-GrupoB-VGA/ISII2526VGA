using AppForMovies.UT;
using AppForSEII2526.API.Controller;
using AppForSEII2526.API.DTOs.ReviewDTOs;
using AppForSEII2526.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.ReviewController_test
{
    public class GetReview_test : AppForSEII25264SqliteUT
    {
        private DateTime _dateOfReview;
        private int _reviewId;
        public GetReview_test()
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
                //new Device { Id = 1, Brand = "Apple", Color = "Yellow", Name = "iPhone 15", priceForPurchase = 100 , priceForRent = 10, quantityForPurchase = 1 , quantityForRent = 10 , Year = 2025},
                //new Device { Id = 2, Brand = "Samsung", Color = "black", Name = "galaxy S24", priceForPurchase = 200 , priceForRent = 20, quantityForPurchase = 2 , quantityForRent = 20 , Year = 2024}
                new Device(1, "Nokia", "yellow", "iPhone 15", 100, 10, 1, 10, 2025) { ModelId = 1 },
                new Device(2, "Samsung", "black", "Galaxy S24", 200, 20, 2, 20, 2024) { ModelId = 2 },
            };

            _context.AddRange(devices);
            _context.SaveChanges();

            ApplicationUser user = new ApplicationUser { Id = "user-alicia-001", FirstName = "Alicia", LastName = "Pérez", Address = "C/ Mayor 12, Albacete", Email = "alicia@example.com", PhoneNumber = null, UserName = "alicia@example.com" };

            _dateOfReview = DateTime.Now;

            var review = new Review
            {
                DateOfReview = _dateOfReview,  // Usar la misma fecha
                CustomerCountry = "Spain",
                CustomerId = 1,
                ApplicationUserId = "user-alicia-001",
                OverallRating = 5,
                ReviewTitle = "Great devices!"
            };


            review.ReviewItems.Add(new ReviewItem("Comentario de ReviewItem1", devices[0].Id, 5, review));

            _context.Users.Add(user);
            _context.SaveChanges();


            _context.Add(review);
            _context.SaveChanges();

            _reviewId = review.ReviewId;
        }


        [Fact] //Esto test es para cuando le meto que busque una reseña q no existe
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetReview_NotFound_test()
        {
            //Arrange (Preparar)
            var mock = new Mock<ILogger<ReviewController>>();
            ILogger<ReviewController> logger = mock.Object;

            var controller = new ReviewController(_context, logger);

            // Act
            var result = await controller.GetReview(0); //Con esto pone que no existe la reseña

            //Assert (Comprobar)
            //we check that the response type is OK and obtain the list of reviews
            Assert.IsType<NotFoundResult>(result);
        }



        [Fact] //Esto test es para cuando le meto que busque una reseña que si existe
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]   //Esto es un burrullo que nadie sabe que pasa
        public async Task GetReview_Found_test()
        {
            // Arrange: se prepara el logger (mock) y el controlador
            var mock = new Mock<ILogger<ReviewController>>();
            ILogger<ReviewController> logger = mock.Object;
            var controller = new ReviewController(_context, logger);

            // Se crea el DTO esperado
            var expectedReview = new ReviewDetailDTO(
                id: _reviewId,
                dateOfReview: _dateOfReview,
                customerCountry: "Spain",
                reviewTitle: "Great devices!",
                overallRating: 5.0,
                reviewItems: new List<ReviewItemDetailDTO>
                {
                    new ReviewItemDetailDTO(
                        deviceId: 1,
                        deviceName: "iPhone 15",
                        deviceModel: "iPhone 15",
                        deviceYear: 2025,
                        comment: "Comentario de ReviewItem1",
                        rating: 5f
                    )
                },
                customerName: "Alicia" 
            );

            // Act: se llama al método del controlador
            var result = await controller.GetReview(_reviewId);

            // Assert: comprobamos que la respuesta sea OK
            var okResult = Assert.IsType<OkObjectResult>(result);
            var reviewDTOActual = Assert.IsType<ReviewDetailDTO>(okResult.Value);

            // Se compara el DTO esperado con el recibido
            Assert.Equal(expectedReview, reviewDTOActual);
        }


    }
}