using AppForMovies.UT;
using AppForSEII2526.API.Controller;
using AppForSEII2526.API.DTOs.PurchaseDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using PaymentMethodType = AppForSEII2526.API.Models.Purchase.PaymentMethodType;

namespace AppForSEII2526.UT.PurchasesController_test
{

    public class CreatePurchase_test : AppForSEII25264SqliteUT
    {
        private const string _deliveryAddress = "C/ Mayor 1";
        private const string _customerFirstName = "Alicia";
        private const string _customerLastName = "Pérez";
        private const string _customerFullName = _customerFirstName + " " + _customerLastName;
        private const string _userNameCustomer = "alicia@example.com";

        private const string _device1Name = "iPhone 15 Base";
        private const string _device2Name = "Galaxy S24";
        private const string _brand1 = "Apple";
        private const string _brand2 = "Samsung";


        public CreatePurchase_test()
        {
            // CARGAR EN LA BD EN MEMORIA

            // Modelos
            var iphoneModel = new Model { NameModel = "iPhone 15" };
            var s24Model = new Model { NameModel = "Galaxy S24" };

            _context.AddRange(iphoneModel, s24Model);
            _context.SaveChanges();

            // Dispositivos: uno SIN stock para provocar error, otro con stock OK
            var devices = new List<Device>
            {
                new Device
                {
                    Brand = _brand1,
                    Color = "Blue",
                    Name = _device1Name,
                    priceForPurchase = 799,
                    priceForRent = 25,
                    quantityForPurchase = 0,
                    quantityForRent = 3,
                    Year = 2023,
                    ModelId = iphoneModel.Id
                },
                new Device
                {
                    Brand = _brand2,
                    Color = "Black",
                    Name = _device2Name,
                    priceForPurchase = 699,
                    priceForRent = 22,
                    quantityForPurchase = 10,
                    quantityForRent = 5,
                    Year = 2024,
                    ModelId = s24Model.Id
                },

                 new Device
                {
                    Brand = "Huawei",
                    Color = "Black",
                    Name = _device2Name,
                    priceForPurchase = 699,
                    priceForRent = 22,
                    quantityForPurchase = 10,
                    quantityForRent = 5,
                    Year = 2024,
                    ModelId = s24Model.Id
                },

                 new Device
                {
                    Brand = "Xiaomi",
                    Color = "Black",
                    Name = _device2Name,
                    priceForPurchase = 699,
                    priceForRent = 22,
                    quantityForPurchase = 10,
                    quantityForRent = 5,
                    Year = 2024,
                    ModelId = s24Model.Id
                }
            };
            _context.AddRange(devices);

            // Usuario registrado
            var user = new ApplicationUser
            {
                UserName = _userNameCustomer,
                Email = _userNameCustomer,
                FirstName = "Alicia",
                LastName = "Pérez",
                Address = _deliveryAddress,
                EmailConfirmed = true
            };
            _context.Add(user);

            _context.SaveChanges();


            var purchase = new Purchase(
                deliveryAddress: _deliveryAddress,
                id: 1,
                paymentMethod: PaymentMethodType.CreditCard,
                purchaseDate: DateTime.Today,
                totalPrice: 0,
                totalQuantity: 0)
            {
                ApplicationUser = user,
                PurchaseItems = new List<PurchaseItem>()
            };

            purchase.PurchaseItems.Add(new PurchaseItem
            {
                DeviceId = devices[1].Id,
                Price = devices[1].priceForPurchase,
                Quantity = 1,
                Description = "Compra inicial"
            });

            purchase.TotalQuantity = purchase.PurchaseItems.Sum(i => i.Quantity);
            purchase.TotalPrice = purchase.PurchaseItems.Sum(i => i.Price * i.Quantity);

            _context.Add(purchase);
            _context.SaveChanges();
        }

        // CASOS DE PRUEBA PARA ERRORES

        public static IEnumerable<object[]> TestCasesFor_CreatePurchase()
        {
            var emptyItems = new List<PurchaseItemDTO>();

            var itemsOk = new List<PurchaseItemDTO>
    {
        new PurchaseItemDTO(
            deviceID: 2,
            quantity: 1,
            description: "Compra correcta",
            brand: _brand2,
            model: _device2Name,
            color: "Black",
            price: 699)
    };

            var itemsDeviceNotAvailable = new List<PurchaseItemDTO>
    {
        new PurchaseItemDTO(
            deviceID: 1,
            quantity: 1,
            description: "Sin stock",
            brand: _brand1,
            model: _device1Name,
            color: "Blue",
            price: 799)
    };

            var marcaHuawei = new List<PurchaseItemDTO>
    {
        new PurchaseItemDTO(
            deviceID: 3,
            quantity: 1,
            description: "Marca invalida",
            brand: "Huawei",
            model: _device2Name,
            color: "Black",
            price: 699)
    };

            var marcaXiaomi = new List<PurchaseItemDTO>
    {
        new PurchaseItemDTO(
            deviceID: 4,
            quantity: 1,
            description: "Marca invalida",
            brand: "Xiaomi",
            model: _device2Name,
            color: "Black",
            price: 699)
    };

            // 1) Sin items
            var purchaseNoItem = new PurchaseForCreateDTO(
                customerUserName: _userNameCustomer,
                customerFirstName: _customerFirstName,
                customerLastName: _customerLastName,
                deliveryAddress: _deliveryAddress,
                paymentMethod: PaymentMethodType.CreditCard,
                purchaseItems: emptyItems);

            // 2) Usuario no registrado
            var applicationUserNotRegistered = new PurchaseForCreateDTO(
                customerUserName: "no.registrado@example.com",
                customerFirstName: _customerFirstName,
                customerLastName: _customerLastName,
                deliveryAddress: _deliveryAddress,
                paymentMethod: PaymentMethodType.CreditCard,
                purchaseItems: itemsOk);

            // 3) Dispositivo sin stock
            var purchaseDeviceNotAvailable = new PurchaseForCreateDTO(
                customerUserName: _userNameCustomer,
                customerFirstName: _customerFirstName,
                customerLastName: _customerLastName,
                deliveryAddress: _deliveryAddress,
                paymentMethod: PaymentMethodType.CreditCard,
                purchaseItems: itemsDeviceNotAvailable);

            // 4) Dispositivo Huawei
            var compraHuawei = new PurchaseForCreateDTO(
                customerUserName: _userNameCustomer,
                customerFirstName: _customerFirstName,
                customerLastName: _customerLastName,
                deliveryAddress: _deliveryAddress,
                paymentMethod: PaymentMethodType.CreditCard,
                purchaseItems: marcaXiaomi);

            // 5 Dispositivo Xiaomi
            var compraXiaomi = new PurchaseForCreateDTO(
                customerUserName: _userNameCustomer,
                customerFirstName: _customerFirstName,
                customerLastName: _customerLastName,
                deliveryAddress: _deliveryAddress,
                paymentMethod: PaymentMethodType.CreditCard,
                purchaseItems: marcaHuawei);

            var allTests = new List<object[]>
            {

                new object[]
                {
                    purchaseNoItem, "Error! Hay que incluir un dispositivo"
                },

                new object[]
                {
                    applicationUserNotRegistered,"Error! Nombre de usuario no registrado"
                },

                new object[]
                {
                    purchaseDeviceNotAvailable,"Error! No hay suficiente stock de este dispositivo"
                },
                 new object[]
                {
                    compraXiaomi,"Error: Las tecnologías de estas marcas ya no están disponibles, siguiendo recomendaciones de las autoridades competentes en materia de seguridad."
                },
                 new object[]
                {
                    compraHuawei,"Error: Las tecnologías de estas marcas ya no están disponibles, siguiendo recomendaciones de las autoridades competentes en materia de seguridad."
                },
            };


            return allTests;
        }


        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [MemberData(nameof(TestCasesFor_CreatePurchase))]
        public async Task CreatePurchase_Error_test(PurchaseForCreateDTO purchaseDTO, string errorExpected)
        {
            // Arrange
            var mock = new Mock<ILogger<PurchasesController>>();
            ILogger<PurchasesController> logger = mock.Object;

            var controller = new PurchasesController(_context, logger);

            // Act
            var result = await controller.CreatePurchase(purchaseDTO);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);

            var errorActual = problemDetails.Errors.First().Value[0];

            Assert.StartsWith(errorExpected, errorActual);
        }

        //CASO DE ÉXITO

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task CreatePurchase_Success_test()
        {
            // Arrange
            var mock = new Mock<ILogger<PurchasesController>>();
            ILogger<PurchasesController> logger = mock.Object;

            var controller = new PurchasesController(_context, logger);

            var items = new List<PurchaseItemDTO>
    {
        new PurchaseItemDTO(
            deviceID: 2,
            quantity: 2,
            description: "Compra correcta",
            brand: _brand2,
            model: _device2Name,
            color: "Black",
            price: 699)
    };

            var purchaseDTO = new PurchaseForCreateDTO(
                customerUserName: _userNameCustomer,
                customerFirstName: _customerFirstName,
                customerLastName: _customerLastName,
                deliveryAddress: _deliveryAddress,
                paymentMethod: PaymentMethodType.CreditCard,
                purchaseItems: items);


            var expectedPurchaseDetailDTO = new PurchaseDetailDTO(
                id: 2,
                purchaseDate: DateTime.Now,
                customerFullName: _customerFullName,
                deliveryAddress: _deliveryAddress,
                totalPrice: 2 * 699,
                totalQuantity: 2,
                items: items);

            // Act
            var result = await controller.CreatePurchase(purchaseDTO);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var actualDetailDTO = Assert.IsType<PurchaseDetailDTO>(createdResult.Value);

            Assert.Equal(expectedPurchaseDetailDTO, actualDetailDTO);
        }

    }
}