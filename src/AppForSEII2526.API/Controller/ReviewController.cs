using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppForSEII2526.API.DTOs.ReviewDTOs;
using AppForSEII2526.API.Models;
using Microsoft.Extensions.Logging;

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
        // Crea una reseña (paso 5). El servidor asigna fecha/id; el DTO de creación no debe pedirlos.
        [HttpPost]
        public async Task<IActionResult> CreateReview([FromBody] ReviewForCreateDTO dto)
        {
            if (dto == null) return BadRequest();

            // Validaciones mínimas
            if (string.IsNullOrWhiteSpace(dto.CustomerCountry) || string.IsNullOrWhiteSpace(dto.ReviewTitle) || dto.ReviewItems == null || !dto.ReviewItems.Any())
                return BadRequest("Faltan datos obligatorios (país, título o items).");

            // Mapear a entidad (ajusta según tu modelo)
            var review = new Review
            {
                CustomerCountry = dto.CustomerCountry,
                // CustomerId: normalmente se obtiene del contexto de autenticación; aquí lo tomamos si viene en el DTO
                CustomerId = dto.CustomerId,
                DateOfReview = DateTime.UtcNow,
                OverallRating = dto.OverallRating,
                ReviewTitle = dto.ReviewTitle,
                ReviewItems = dto.ReviewItems.Select(i => new ReviewItem
                {
                    DeviceId = i.DeviceId,
                    Comment = i.Comment,
                    Rating = i.Rating
                }).ToList()
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            // Devuelve la ubicación del nuevo recurso. El id puede ser review.ReviewId (ajusta si tu PK difiere)
            return CreatedAtAction(nameof(GetReview), new { id = review.ReviewId }, null);
        }

        // GET: api/Review/{id}
        // Devuelve el detalle de la reseña (paso 7). Mapea a DTO ReviewDetail si lo tienes.
        [HttpGet("{id:int}")]
        public async Task<ActionResult<object>> GetReview(int id)
        {
            var review = await _context.Reviews
                .Include(r => r.ReviewItems)
                    .ThenInclude(ri => ri.Device)
                .FirstOrDefaultAsync(r => r.ReviewId == id);

            if (review == null) return NotFound();

            // Mapeo ligero (puedes reemplazar por ReviewDetail DTO)
            var result = new
            {
                Id = review.ReviewId,
                DateOfReview = review.DateOfReview,
                CustomerCountry = review.CustomerCountry,
                //CustomerName = review.ApplicationUser != null ? $"{review.ApplicationUser.Name} {review.ApplicationUser.Surname}" : null,
                ReviewTitle = review.ReviewTitle,
                OverallRating = review.OverallRating,
                Items = review.ReviewItems.Select(ri => new
                {
                    ri.DeviceId,
                    Name = ri.Device?.Name,
                    Model = ri.Device?.Model?.NameModel,
                    Year = ri.Device?.Year,
                    ri.Comment,
                    ri.Rating
                })
            };

            return Ok(result);
        }
    }
}