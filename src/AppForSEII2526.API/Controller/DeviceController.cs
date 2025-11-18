using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using AppForSEII2526.API.Data;
using AppForSEII2526.API.DTOs.DeviceDTOs;
using AppForSEII2526.API.DTOs.ReviewDTOs;

namespace AppForSEII2526.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        
        private readonly ApplicationDbContext _context;
       
        private readonly ILogger<DeviceController> _logger;

        public DeviceController(ApplicationDbContext context, ILogger<DeviceController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // ---------- DISPOSITIVOS PARA COMPRAR ----------

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

        // ---------- DISPOSITIVOS PARA ALQUILAR ----------
        

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(List<DispositivoComprarDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ModelError), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> GetDevicesForRenting()
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

        // ---------- DISPOSITIVOS PARA RESEÑAS ----------

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(List<ReviewItemDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetDevicesForReview()
        {
            var devices = await _context.Devices
                .Select(d => new ReviewItemDTO(
                    d.Id,
                    d.Brand,
                    d.Name,
                    d.Color,
                    d.Year,
                    d.Model.NameModel
                ))
                .ToListAsync();

            return Ok(devices);
        }
    }
}
