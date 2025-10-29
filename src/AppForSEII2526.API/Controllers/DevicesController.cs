using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppForSEII2526.API.Data;
using AppForSEII2526.API.DTOs.PurchaseDTOs; // <- namespace del DTO

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DevicesController> _logger;

        public DevicesController(ApplicationDbContext context, ILogger<DevicesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(List<DispositivoComprarDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetDevicesForBuying()
        {
            var devices = await _context.Devices
                .Select(d => new DispositivoComprarDTO(
                    d.Id,                 // id
                    d.Name,               // nombre
                    d.Brand,              // marca
                    d.Model.NameModel,    // modelo (JOIN por navegación)
                    d.Color,              // color
                    d.priceForPurchase    // precio
                ))
                .ToListAsync();

            return Ok(devices);
        }
    }
}
