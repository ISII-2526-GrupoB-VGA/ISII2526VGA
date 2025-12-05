using AppForMovies.UT;
using AppForSEII2526.API.Controller;
using AppForSEII2526.API.DTOs.DeviceDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;



namespace AppForSEII2526.UT.DevicesController_test
{
    public class GetDevices_test : AppForSEII25264SqliteUT
    {
        public GetDevices_test()
        {
            
            var models = new List<Model>() {
                new Model("Model1"),
                new Model("Model2"),
                new Model("Model3"),
                new Model("Model4")
            };

         

            var devices = new List<Device>() {
                new Device(1, "BrandA", "Black", "Device1", 500.00, 300.00, 10, 5, 2020) { Model = models[0] },
                new Device(2, "BrandB", "White", "Device2", 700.00, 400.00,  8, 3, 2021) { Model = models[1] },
                new Device(3, "BrandC", "Blue",  "Device3", 600.00, 350.00, 15, 7, 2019) { Model = models[2] },
                new Device(4, "BrandD", "Red",   "Device4", 800.00, 450.00,  5, 2, 2022) { Model = models[3] }
            };

            
            ApplicationUser user = new ApplicationUser(
                "Victor",
                "Valtueña",
                "a",
                "victormanuel.valtuena@alu.uclm.es",
                "646464646"
            );

            var purchase1 = new Purchase(
                "a",
                Purchase.PaymentMethodType.CreditCard,
                DateTime.Now,
                1200.00,
                2
            )
            {
                ApplicationUser = user
            };


            _context.Add(user);
            _context.AddRange(models);
            _context.AddRange(devices);
            _context.Add(purchase1);
            _context.SaveChanges();
        }

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetDevicesForBuying_OK_test()
        {
           
            var controller = new DeviceController(_context, null);

            
            var result = await controller.GetDevicesForBuying();

            
            var okResult = Assert.IsType<OkObjectResult>(result);
            var deviceDTOsActual =
                Assert.IsType<List<DispositivoComprarDTO>>(okResult.Value);

          

            var expectedDevices = new List<DispositivoComprarDTO>()
            {
                new DispositivoComprarDTO(1, "Device1", "BrandA", "Model1", "Black", 500.00),
                new DispositivoComprarDTO(2, "Device2", "BrandB", "Model2", "White", 700.00),
                new DispositivoComprarDTO(3, "Device3", "BrandC", "Model3", "Blue",  600.00),
                new DispositivoComprarDTO(4, "Device4", "BrandD", "Model4", "Red",   800.00),
            };

            
            var expectedOrdered = expectedDevices.OrderBy(d => d.Id).ToList();
            var actualOrdered = deviceDTOsActual.OrderBy(d => d.Id).ToList();

            Assert.Equal(expectedOrdered, actualOrdered);
        }
    }
}

