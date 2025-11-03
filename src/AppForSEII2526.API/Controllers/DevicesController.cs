using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppForSEII2526.API.Data;
using AppForSEII2526.API.DTOs.PurchaseDTOs; // <- namespace del DTO
using Microsoft.AspNetCore.Mvc.ModelBinding;

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
        [ProducesResponseType(typeof(ModelError), (int)HttpStatusCode.BadRequest)]
        
        public async Task<ActionResult> GetDevicesForBuying()
        {
            var devices = await _context.Devices
                .Select(d => new DispositivoComprarDTO(
                    d.Id,                 
                    d.Name,               
                    d.Brand,              
                    d.Model.NameModel,    
                    d.Color,              
                    d.priceForPurchase    
                ))
                .ToListAsync();

            return Ok(devices);
        }
    }
}
