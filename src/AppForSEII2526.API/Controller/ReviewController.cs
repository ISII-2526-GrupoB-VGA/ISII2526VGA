using AppForSEII2526.API.DTOs.ReviewDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading.Tasks;

namespace AppForSEII2526.API.Controller 
                                        //Es del DTO de GetSelect (Paso 2)
{


    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ReviewController> _logger;

        public ReviewController(ApplicationDbContext context, ILogger<ReviewController> logger)
        {
            _context = context;
            _logger = logger;
        }
        // POST: api/Review
        // Crea una reseña (paso 6 del caso de uso: cliente rellena y guarda)


        [HttpPost]
        [ProducesResponseType(typeof(ReviewDetailDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        public async Task<ActionResult> CreateReview(ReviewForCreateDTO dto)
        {
            // --- VALIDACIONES ---
            if (string.IsNullOrWhiteSpace(dto.CustomerCountry))
                ModelState.AddModelError(nameof(dto.CustomerCountry),
                    "Error. El país es obligatorio (flujo alternativo 3)");

            if (string.IsNullOrWhiteSpace(dto.ReviewTitle))
                ModelState.AddModelError(nameof(dto.ReviewTitle),
                    "Error. El título de la reseña es obligatorio (flujo alternativo 3)");

            if (dto.ReviewItems == null || !dto.ReviewItems.Any())
                ModelState.AddModelError(nameof(dto.ReviewItems),
                    "Error. Debe incluir al menos un dispositivo para reseñar (flujo alternativo 2)");

            foreach (var item in dto.ReviewItems ?? Enumerable.Empty<ReviewItemForCreateDTO>())
            {
                if (item.Rating < 1 || item.Rating > 5)
                    ModelState.AddModelError(nameof(dto.ReviewItems),
                        $"Error! La valoración debe estar entre 1 y 5 (flujo alternativo 5). Dispositivo {item.DeviceId} tiene rating {item.Rating}");

                if (string.IsNullOrWhiteSpace(item.Comment))
                    ModelState.AddModelError(nameof(dto.ReviewItems),
                        $"Error! El comentario es obligatorio para cada dispositivo (flujo alternativo 3). Dispositivo {item.DeviceId} no tiene comentario");
            }

            //var user = await _context.Users.FirstOrDefaultAsync();
            //if (user == null)
            //    ModelState.AddModelError("ApplicationUser",
            //        "Error! No hay usuarios en el sistema. La precondición requiere un usuario conectado como Cliente");


            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            // --- VERIFICAR DISPOSITIVOS ---
            var deviceIds = dto.ReviewItems.Select(i => i.DeviceId).Distinct().ToList();
            var devices = await _context.Devices
                .Include(d => d.Model)
                .Where(d => deviceIds.Contains(d.Id))
                .Select(d => new
                {
                    d.Id,
                    d.Name,
                    d.Brand,
                    d.Color,
                    d.Year,
                    ModelName = d.Model.NameModel
                })
                .ToListAsync();


            // --- CREAR REVIEW ---
            //var review = new Review
            //{
            //    CustomerCountry = dto.CustomerCountry,
            //    CustomerId = 1, // TODO: sustituir por el ID del cliente real
            //    DateOfReview = DateTime.UtcNow,
            //    OverallRating = dto.ReviewItems.Average(i => i.Rating),
            //    ReviewTitle = dto.ReviewTitle,
            //    ApplicationUserId = user.Id
            //};



            //Buscar usuario con nombre
            //string userId = null; //Esto lo he añadido para q busque el id del usuario a partir del nombre
            //foreach (var userName in _context.Users)
            //{
            //    if (dto.CustomerName.Equals(userName))
            //    {
            //        userId = userName.Id;
            //        break;
            //    }
            //}

            //if (string.IsNullOrEmpty(userId))//Añadido 
            //    return BadRequest("Error: no se pudo identificar al usuario autenticado. Debe estar logueado.");//Añadido

            //buscar usuario. Estas 3 lineas las añadí
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == dto.CustomerName);
            if (user == null)
                return BadRequest($"Error: no se encontró ningún usuario con el nombre '{dto.CustomerName}'. Debe estar registrado en el sistema.");

            string userId = user.Id;


            Review review = new Review(dto.CustomerCountry, 1,userId, DateTime.Now, dto.ReviewItems.Average(i => i.Rating),dto.ReviewTitle);

            foreach (var item in dto.ReviewItems)
            {
                var dev = devices.FirstOrDefault(d => d.Id == item.DeviceId);
                if (!devices.Any(d => d.Id == item.DeviceId))
                {
                    ModelState.AddModelError(nameof(dto.ReviewItems),
                        $"Error! El dispositivo con ID {item.DeviceId} no existe o no está disponible");
                }
                else
                {
                    review.ReviewItems.Add(new ReviewItem(item.Comment, dev.Id, item.Rating, review));
                   
                }
                    

            }

            
            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            

            _context.Add(review);

            //foreach (var item in dto.ReviewItems)// esta frma de 9ntru8r esta mal            //{
            //    _context.ReviewItems.Add(new ReviewItem
            //    {
            //        DeviceId = item.DeviceId,
            //        Comment = item.Comment,
            //        Rating = item.Rating,
            //        Review = review
            //    });
            //}

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Reseña {review.ReviewId} creada exitosamente por {dto.CustomerCountry}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar la reseña en la base de datos");
                return Conflict($"Error! Hubo un problema al guardar la reseña. Detalles: {ex.Message}");
            }

            // --- CONSTRUIR DTO DE RESPUESTA ---
            var reviewDetail = new ReviewDetailDTO(
                id: review.ReviewId,
                dateOfReview: review.DateOfReview,
                customerCountry: review.CustomerCountry,
                reviewTitle: review.ReviewTitle,
                overallRating: review.OverallRating,
                reviewItems: review.ReviewItems.Select(ri =>
                {
                    var device = devices.First(d => d.Id == ri.DeviceId);
                    return new ReviewItemDetailDTO(
                        deviceId: ri.DeviceId,
                        deviceName: device.Name,
                        deviceModel: device.ModelName,
                        deviceYear: device.Year,
                        comment: ri.Comment,
                        rating: ri.Rating
                    );
                }).ToList(),
                customerName: user.UserName
            );

            return CreatedAtAction(nameof(GetReview), new { id = review.ReviewId }, reviewDetail);
        }




        // GET: api/Review/{id}
        // Devuelve el detalle de la reseña (paso 7 del caso de uso)
        [HttpGet("{id:int}")]
        [Route("[action]")]
        [ProducesResponseType(typeof(ReviewDetailDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetReview(int id)
        {
            //var review = await _context.Reviews
            //    .Include(r => r.ReviewItems)
            //        .ThenInclude(ri => ri.Device)
            //            .ThenInclude(d => d.Model)
            //    .FirstOrDefaultAsync(r => r.ReviewId == id);

            //if (review == null) return NotFound();

            // Paso 7: datos que se muestran (nombre y país del cliente, título, fecha, y por cada dispositivo: nombre, modelo, año, puntuación, comentario)
            //var result = new
            //{
            //    Id = review.ReviewId,
            //    DateOfReview = review.DateOfReview,
            //    CustomerCountry = review.CustomerCountry,
            //    CustomerName = "Cliente Test", // TODO: obtener de ApplicationUser si está relacionado
            //    ReviewTitle = review.ReviewTitle,
            //    OverallRating = review.OverallRating,
            //    Items = review.ReviewItems.Select(ri => new
            //    {
            //        ri.DeviceId,
            //        Name = ri.Device?.Name,
            //        Model = ri.Device?.Model?.NameModel,
            //        Year = ri.Device?.Year,
            //        ri.Comment,
            //        ri.Rating
            //    })
            //};

            ReviewDetailDTO? review = await _context.Reviews
                .Where(r => r.ReviewId == id)

                .Include(r => r.ReviewItems) //join table RentalItems
                    .ThenInclude(ri => ri.Device) //then join table Movies
                        .ThenInclude(device => device.Model) //then join table Genre

                .Include(r => r.ApplicationUser) //join table ApplicationUser

                .Select(r => new ReviewDetailDTO(r.ReviewId, r.DateOfReview, r.CustomerCountry,
                        r.ReviewTitle, r.OverallRating,
                        r.ReviewItems
                            .Select(ri => new ReviewItemDetailDTO(ri.DeviceId, ri.Device.Name, ri.Device.Model.NameModel, 
                            ri.Device.Year, ri.Comment,ri.Rating)).ToList(), r.ApplicationUser.FirstName)).FirstOrDefaultAsync();


            return Ok(review);
        }
    }
}


//public ReviewItemDetailDTO(int deviceId, string deviceName, string deviceModel, int deviceYear, string comment, float rating)
//{
//    DeviceId = deviceId;
//    DeviceName = deviceName;
//    DeviceModel = deviceModel;
//    DeviceYear = deviceYear;
//    Comment = comment;
//    Rating = rating;
//}

