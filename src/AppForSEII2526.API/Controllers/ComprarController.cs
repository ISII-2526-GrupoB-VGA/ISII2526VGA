using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppForSEII2526.API.Data;
using AppForSEII2526.API.DTOs.PurchaseDTOs;
using AppForSEII2526.API.Models;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchasesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PurchasesController> _logger;

        public PurchasesController(ApplicationDbContext context, ILogger<PurchasesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // ---------- GET ----------
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(PurchaseDetailDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetPurchase(int id)
        {
            if (_context.Purchases == null)
            {
                _logger.LogError("Error: Purchases table does not exist");
                return NotFound();
            }

            var purchase = await _context.Purchases
                .Where(p => p.id == id)
                    .Include(p => p.PurchaseItems)
                        .ThenInclude(pi => pi.Device)
                            .ThenInclude(d => d.Model)
                .Include(p => p.ApplicationUser)
                .Select(p => new PurchaseDetailDTO(
                    p.id,
                    p.PurchaseDate,
                    (p.ApplicationUser.FirstName ?? "") + " " + (p.ApplicationUser.LastName ?? ""),
                    p.DeliveryAddress,
                    p.TotalPrice,
                    p.TotalQuantity,
                    p.PurchaseItems
                        .Select(pi => new PurchaseItemDTO(
                            pi.DeviceId,
                            pi.Device.Brand,
                            pi.Device.Model.NameModel,
                            pi.Device.Color,
                            pi.Price,
                            pi.Quantity,
                            pi.Description
                        )).ToList()
                ))
                .FirstOrDefaultAsync();

            if (purchase == null)
            {
                _logger.LogError($"Error: Purchase with id {id} does not exist");
                return NotFound();
            }

            return Ok(purchase);
        }

        // ---------- POST ----------
        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(PurchaseDetailDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        public async Task<ActionResult> CreatePurchase(PurchaseForCreateDTO purchaseForCreate)
        {
            // Validaciones de negocio simples
            if (purchaseForCreate.PurchaseItems.Count == 0)
                ModelState.AddModelError("PurchaseItems", "Error! You must include at least one device.");

            if (string.IsNullOrWhiteSpace(purchaseForCreate.CustomerFirstName))
                ModelState.AddModelError("CustomerFirstName", "First name is required");

            if (string.IsNullOrWhiteSpace(purchaseForCreate.CustomerLastName))
                ModelState.AddModelError("CustomerLastName", "Last name is required");

            var user = await _context.ApplicationUsers
                .FirstOrDefaultAsync(u => u.UserName == purchaseForCreate.CustomerUserName);
            if (user == null)
                ModelState.AddModelError("PurchaseApplicationUser", "Error! UserName is not registered.");

            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            // IDs de dispositivos desde PurchaseItemDTO (nota: DeviceID)
            var deviceIds = purchaseForCreate.PurchaseItems
                .Select(i => i.DeviceID)
                .Distinct()
                .ToList();

            // Traemos info mínima necesaria de los devices
            var devices = await _context.Devices
                .Where(d => deviceIds.Contains(d.Id))
                .Select(d => new
                {
                    d.Id,
                    d.Brand,
                    d.Color,
                    d.priceForPurchase,
                    d.quantityForPurchase
                })
                .ToListAsync();

            // Construimos la compra usando el PaymentMethod recibido en el DTO
            var purchase = new Purchase(
                deliveryAddress: purchaseForCreate.DeliveryAddress,
                id: 0,
                paymentMethod: purchaseForCreate.PaymentMethod, // <- del DTO
                purchaseDate: DateTime.Now,
                totalPrice: 0.0,
                totalQuantity: 0
            )
            {
                ApplicationUser = user,
                PurchaseItems = new List<PurchaseItem>()
            };

            double total = 0;
            int totalQty = 0;

            foreach (var item in purchaseForCreate.PurchaseItems) // item es PurchaseItemDTO
            {
                var dev = devices.FirstOrDefault(d => d.Id == item.DeviceID);
                if (dev == null)
                {
                    ModelState.AddModelError("PurchaseItems", $"Error! DeviceID {item.DeviceID} does not exist.");
                    continue;
                }

                if (dev.quantityForPurchase < item.Quantity)
                {
                    ModelState.AddModelError("PurchaseItems",
                        $"Error! Not enough stock for device {dev.Id}. Requested {item.Quantity}, available {dev.quantityForPurchase}.");
                    continue;
                }

                var unit = dev.priceForPurchase; // precio “congelado” en la línea
                total += unit * item.Quantity;
                totalQty += item.Quantity;

                purchase.PurchaseItems.Add(new PurchaseItem
                {
                    DeviceId = dev.Id,
                    Price = unit,
                    Quantity = item.Quantity,
                    Description = item.Description ?? string.Empty
                });
            }

            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            purchase.TotalPrice = total;
            purchase.TotalQuantity = totalQty;

            _context.Add(purchase);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while saving purchase");
                return Conflict("Error! There was an error while saving your purchase. Please, try again later.");
            }

            // Detalle de respuesta (re-lee datos del device para completar brand/model/color)
            var detail = new PurchaseDetailDTO(
                purchase.id,
                purchase.PurchaseDate,
                (user.FirstName ?? "") + " " + (user.LastName ?? ""),
                purchase.DeliveryAddress,
                purchase.TotalPrice,
                purchase.TotalQuantity,
                purchase.PurchaseItems.Select(pi => new PurchaseItemDTO(
                    pi.DeviceId,
                    _context.Devices.Where(d => d.Id == pi.DeviceId).Select(d => d.Brand).FirstOrDefault() ?? "",
                    _context.Devices.Where(d => d.Id == pi.DeviceId).Select(d => d.Model.NameModel).FirstOrDefault() ?? "",
                    _context.Devices.Where(d => d.Id == pi.DeviceId).Select(d => d.Color).FirstOrDefault() ?? "",
                    pi.Price,
                    pi.Quantity,
                    pi.Description
                )).ToList()
            );

            return CreatedAtAction(nameof(GetPurchase), new { id = purchase.id }, detail);
        }
    }
}
