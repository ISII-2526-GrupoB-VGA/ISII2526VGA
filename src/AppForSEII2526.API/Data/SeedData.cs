using AppForSEII2526.API.Data;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

// Alias al enum anidado en Purchase
using PaymentMethodType = AppForSEII2526.API.Models.Purchase.PaymentMethodType;

namespace AppForSEII2526.API.Data
{
    public static class SeedData
    {
        public static void Initialize(ApplicationDbContext dbContext, IServiceProvider serviceProvider, ILogger logger)
        {
            // 1) Roles
            var roles = new List<string> { "Administrator", "Employee", "Customer" };
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            try { SeedRoles(roleManager, roles); }
            catch (Exception ex) { logger.LogError(ex, "Error seeding roles"); }

            // 2) Usuarios
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            try { SeedUsers(userManager, roles); }
            catch (Exception ex) { logger.LogError(ex, "Error seeding users"); }

            // 3) Modelos y Dispositivos
            try { SeedModelsAndDevices(dbContext); }
            catch (Exception ex) { logger.LogError(ex, "Error seeding models/devices"); }

            // 4) Compra de ejemplo 
            try
            {
                var user = dbContext.Users.OfType<ApplicationUser>().FirstOrDefault(u => u.UserName == "alicia@example.com");
                if (user != null) SeedPurchase(dbContext, user);
            }
            catch (Exception ex) { logger.LogError(ex, "Error seeding a Purchase"); }
        }

        private static void SeedRoles(RoleManager<IdentityRole> roleManager, List<string> roles)
        {
            foreach (var roleName in roles)
            {
                if (!roleManager.RoleExistsAsync(roleName).Result)
                {
                    var role = new IdentityRole { Name = roleName, NormalizedName = roleName.ToUpperInvariant() };
                    roleManager.CreateAsync(role).Wait();
                }
            }
        }

        private static void SeedUsers(UserManager<ApplicationUser> userManager, List<string> roles)
        {
            // Admin
            if (userManager.FindByNameAsync("admin@example.com").Result == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "admin@example.com",
                    Email = "admin@example.com",
                    FirstName = "Admin",
                    LastName = "User",
                    Address = "Av. Sistemas 1, Albacete",
                    EmailConfirmed = true
                };
                var result = userManager.CreateAsync(user, "P@ssw0rd!").Result;
                if (result.Succeeded) userManager.AddToRoleAsync(user, roles[0]).Wait();
            }

            // Employee
            if (userManager.FindByNameAsync("empleado@example.com").Result == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "empleado@example.com",
                    Email = "empleado@example.com",
                    FirstName = "Empleado",
                    LastName = "Tienda",
                    Address = "C/ Trabajo 3, Madrid",
                    EmailConfirmed = true
                };
                var result = userManager.CreateAsync(user, "P@ssw0rd!").Result;
                if (result.Succeeded) userManager.AddToRoleAsync(user, roles[1]).Wait();
            }

            // Customer
            if (userManager.FindByNameAsync("alicia@example.com").Result == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "alicia@example.com",
                    Email = "alicia@example.com",
                    FirstName = "Alicia",
                    LastName = "Pérez",
                    Address = "C/ Mayor 12, Albacete",
                    EmailConfirmed = true
                };
                var result = userManager.CreateAsync(user, "P@ssw0rd!").Result;
                if (result.Succeeded) userManager.AddToRoleAsync(user, roles[2]).Wait();
            }
        }

        private static void SeedModelsAndDevices(ApplicationDbContext db)
        {
            // Modelos
            var iphoneModel = db.Models.FirstOrDefault(m => m.NameModel == "iPhone 15")
                              ?? db.Models.Add(new Model { NameModel = "iPhone 15" }).Entity;

            var s24Model = db.Models.FirstOrDefault(m => m.NameModel == "Galaxy S24")
                           ?? db.Models.Add(new Model { NameModel = "Galaxy S24" }).Entity;

            db.SaveChanges(); // asegura Ids

            // Dispositivos
            if (!db.Devices.Any(d => d.Name == "iPhone 15 Base"))
            {
                db.Devices.Add(new Device
                {
                    Brand = "Apple",
                    Color = "Blue",
                    Name = "iPhone 15 Base",
                    priceForPurchase = 799,
                    priceForRent = 25,
                    quantityForPurchase = 10,
                    quantityForRent = 3,
                    Year = 2023,
                    ModelId = iphoneModel.Id
                });
            }

            if (!db.Devices.Any(d => d.Name == "Galaxy S24"))
            {
                db.Devices.Add(new Device
                {
                    Brand = "Samsung",
                    Color = "Black",
                    Name = "Galaxy S24",
                    priceForPurchase = 699,
                    priceForRent = 22,
                    quantityForPurchase = 15,
                    quantityForRent = 5,
                    Year = 2024,
                    ModelId = s24Model.Id
                });
            }

            db.SaveChanges();
        }

        private static void SeedPurchase(ApplicationDbContext db, ApplicationUser user)
        {
            if (db.Purchases.AsNoTracking().Any()) return; // ya hay compras, no repetir

            var anyDevice = db.Devices.FirstOrDefault();
            if (anyDevice == null) return;

            var purchase = new Purchase(
                deliveryAddress: user.Address ?? "Dirección de prueba",
                id: 0,
                paymentMethod: PaymentMethodType.CreditCard,
                purchaseDate: DateTime.Now,
                totalPrice: 0.0,
                totalQuantity: 0
            )
            {
                ApplicationUser = user,
                PurchaseItems = new List<PurchaseItem>()
            };

           
            purchase.PurchaseItems.Add(new PurchaseItem
            {
                DeviceId = anyDevice.Id,
                Price = anyDevice.priceForPurchase,
                Quantity = 1,
                Description = "Compra de prueba"
            });

            purchase.TotalQuantity = purchase.PurchaseItems.Sum(i => i.Quantity);
            purchase.TotalPrice = purchase.PurchaseItems.Sum(i => i.Price * i.Quantity);

            db.Purchases.Add(purchase);
            db.SaveChanges();
        }
    }
}
