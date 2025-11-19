using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using AppForSEII2526.API.Data;
using AppForSEII2526.API.DTOs.DeviceDTOs;
using AppForSEII2526.API.DTOs.ReviewDTOs;

using AppForSEII2526.API.DTOs.ReviewDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace AppForSEII2526.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        //used to enable your controller to access to the database
        private readonly ApplicationDbContext _context;
        //used to log any informatio when your system is running
        private readonly ILogger<DeviceController> _logger;
        private object brand;

        public DeviceController(ApplicationDbContext context, ILogger<DeviceController> logger)
        {
            _context = context;
            _logger = logger;
        }




        [HttpGet] //Este get me da todos los dispositivos 
        [Route("[action]")]
        [ProducesResponseType(typeof(List<ReviewForCreateDTO>), (int)HttpStatusCode.OK)]
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


        /*

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<Device>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetMoviesForRenting()
        {
            IList<Device> devices = await _context.Devices.ToListAsync();
            return Ok(devices);
        }

        [HttpGet] //Select
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<RentalItemDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetDevicesForRental()
        {
            var Devices = await _context.Devices
                .Select(m=>new RentalItemDTO(m.Id, m.Brand)). //No se si es brand o title
                ToListAsync();
            return Ok(Devices);
        }

        [HttpGet] //Where
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<RentalItemDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetDevicesForRental(string? title)
        {
            var movies = await _context.Devices
                .Where(m=> (m.Brand).Contains(title) || (title==null))
                .Select(m=>new RentalItemDTO(m.Id, m.Brand))
                .ToListAsync();
            return Ok(movies);
        }


        [HttpGet] //OrderBy
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<RentalItemDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> getRentalItem(string? brand)
        {
            var devices = await _context.Devices
                .Where(m=> (m.Brand.Contains(brand)) || (brand==null))
                .OrderBy(m=>m.Brand)
                .Select(m=>new RentalItemDTO(m.Id, m.Brand))
                .ToListAsync();
            return Ok(devices);
        }

        [HttpGet] //ThenBy
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<RentalItemDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetRentalItems(string? brand)
        {
            var devices = await _context.Devices
                .Include(m=>m.Name)
                .Where(m=>((m.Brand.Contains(brand)) || (brand==null)))
                .OrderBy(m=>m.Brand)
                    .ThenBy(m=>m.priceForPurchase)
                .Select(m=>new RentalItemDTO(m.Id, m.Brand, m.Name))
                .ToListAsync();
            return Ok(devices);
        }
        
        [HttpGet] //Include
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<RentalItemDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetRentalItems(string? brand, string? genre)
        {
            var movies=await _context.Devices
                .Include(m=>m.Name)
                .Where(m=>((m.Brand.Contains(brand)) || (brand==null))
                    && ((m.Name.Equals(genre) || (genre==null))))
                .OrderBy(m=>m.Brand)
                .Select(m=>new RentalItemDTO(m.Id, m.Brand, m.Name))
                .ToListAsync();
            return Ok(movies);
        }


        [HttpGet] //Join
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<RentalItemDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetRentalItems(string? title, string? genre, DateTime? from, DateTime? to)
        {
            var movies = await _context.Devices
                .Include(m=>m.Genre)
                .Include(m=>m.priceForRenting)
                    .ThenInclude(ri=>ri.Review) //NPI de q pasa ahí
                .Where(m=>((m.Brand.Contains(title)) || (title==null))
                    && ((m.Genre.Equals(genre)) || (genre==null)))
                .OrderBy(m=>m.Brand)
                .Select(m=>new RentalItemDTO(m.Id, m.Brand, m.Genre, m.priceForRenting))
                .ToListAsync();
            return Ok(movies);
        }



        [HttpGet] //from hasta to
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<RentalItemDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetRentalItems(string? title, string? genre, DateTime from, DateTime to)
        {
            var movies = await _context.Devices
                .Include(m => m.Genre)
                .Include(m=>m.priceForRenting)
                    .ThenInclude(ri => ri.Review) //NPI de q pasa ahí
                .Where(m => ((m.Brand.Contains(brand)) || (brand == null))
                    && ((m.Name.Equals(genre)) || (genre == null))
                    && (m.RentalItems.Count(ri=>ri.Rental.From<=to
                        && ri.Rental.To>=from) < m.QuantityForRent))
                .OrderBy(m=> m.Brand)
                .Select(m => new RentalItemDTO(m.Id, m.Brand, m.Name, m.ReleaseDate,m.priceForRenting,m.RentalItems.Max(ri=>ri.Rental.RentalDate)))
                .ToListAsync();
            return Ok(movies);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<RentalItemDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetStatisticsOfDevicesGroupedByGenre()
        {
            IList<DeviceForStatistics> statistics = await _context.Devices
                .Include(m=>m.Genre)
                    .GroupBy(g => g.Genre,
                (genre,movies)=> new DeviceForStatistics(genre.Name,
                    movies.Count(), movies.Max(m=>m.priceForRenting),
                    movies.Max(m=>m.priceForPurchase))
                    )
                .ToListAsync();
            return Ok(statistics);
        }





        //Cada uno se rellena su base de datos, y cada uno hace su propio controlador
        [HttpGet] //¿Esto donde se mete???
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<MenosInformacionReseñaDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetDevicesForRenting()
        {
            var devices = await _context.Devices.Select(c 
                => new MenosInformacionReseñaDTO(c.Id, c.Name, c.Type, c.PriceForRenting, c.Description)).ToListAsync();
            return Ok(devices);
        }

        */

    }
}
