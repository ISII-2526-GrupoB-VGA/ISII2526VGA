using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppForSEII2526.API.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using AppForSEII2526.API.DTOs.DeviceDTOs;

namespace AppForSEII2526.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlquilarControler : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AlquilarControler> _logger;

        public AlquilarControler(ApplicationDbContext context, ILogger<AlquilarControler> logger)
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
