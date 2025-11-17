using AppForMovies.UT;
using AppForSEII2526.API.Controller;
using AppForSEII2526.API.DTOs.PurchaseDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AppForSEII2526.UT.PurchasesController_test
{
    public class GetPurchases_test : AppForSEII25264SqliteUT
    {
        public GetPurchases_test()
        {
            
            var models = new List<Model>()
            {
                
                new Model { NameModel = "Model1" },
                new Model { NameModel = "Model2" },
            };

           
            var devices = new List<Device>()
            {
                
                new Device(1, "BrandA", "Black", "Device1", 500.00, 300.00, 10, 5, 2020) { Model = models[0] },
                new Device(2, "BrandB", "White", "Device2", 700.00, 400.00,  8, 3, 2021) { Model = models[1] },
            };

            
            var user = new ApplicationUser(
                "Victor",                      
                "Valtueña",                   
                "victorUser",                 
                "victormanuel.valtuena@alu.uclm.es",
                "646464646"
            );

            
            var purchaseDate = new DateTime(2025, 1, 1, 10, 0, 0);

            var purchase = new Purchase(
                "victormanuel.valtuena@alu.uclm.es",
                Purchase.PaymentMethodType.CreditCard,
                purchaseDate,
                500.00,
                1
            )
            {
                ApplicationUser = user,
                DeliveryAddress = "C/ Mayor 1"
            };

            
            var purchaseItem = new PurchaseItem(
                devices[0].Id,               
                "Primer pedido",             
                devices[0].priceForPurchase, 
                1                            
            );

            purchaseItem.Device = devices[0];
            purchase.PurchaseItems.Add(purchaseItem);

            
            _context.ApplicationUsers.Add(user);
            _context.AddRange(models);
            _context.AddRange(devices);
            _context.Purchases.Add(purchase);
            _context.SaveChanges();
        }

        
        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetPurchase_NotFound_test()
        {
            
            var mock = new Mock<ILogger<PurchasesController>>();
            ILogger<PurchasesController> logger = mock.Object;
            var controller = new PurchasesController(_context, logger);

            
            var result = await controller.GetPurchase(0);   // id inexistente

            
            Assert.IsType<NotFoundResult>(result);
        }

        
        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetPurchase_Found_test()
        {
            
            var mock = new Mock<ILogger<PurchasesController>>();
            ILogger<PurchasesController> logger = mock.Object;
            var controller = new PurchasesController(_context, logger);

            
            var purchaseDate = new DateTime(2025, 1, 1, 10, 0, 0);

            var expectedItems = new List<PurchaseItemDTO>()
            {
                new PurchaseItemDTO(
                    1,              
                    "BrandA",      
                    "Model1",       
                    "Black",        
                    500.00,         
                    1,              
                    "Primer pedido" 
                )
            };

            var expectedPurchase = new PurchaseDetailDTO(
                1,                        
                purchaseDate,             
                "Victor Valtueña",        
                "C/ Mayor 1",            
                500.00,                   
                1,                        
                expectedItems
            );

            
            var result = await controller.GetPurchase(1);

            
            var okResult = Assert.IsType<OkObjectResult>(result);
            var purchaseDTOActual = Assert.IsType<PurchaseDetailDTO>(okResult.Value);

            Assert.Equal(expectedPurchase, purchaseDTOActual);
        }
    }
}
