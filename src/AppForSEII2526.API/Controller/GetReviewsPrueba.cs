using AppForSEII2526.API.DTOs.ReviewDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppForSEII2526.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetReviewsPrueba : ControllerBase
    {
        //used to enable your controller to access to the database
        private readonly ApplicationDbContext _context;
        //used to log any informatio when your system is running
        private readonly ILogger<GetReviewsPrueba> _logger;
        private object brand;

        public GetReviewsPrueba(ApplicationDbContext context, ILogger<GetReviewsPrueba> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]")] //Este get debe darme todas las reseñas
        [ProducesResponseType(typeof(List<ReviewForCreateDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetReviews()
        {
            var reviews = await _context.Reviews
                .Select(d => new ReviewsDTO(
                    d.ReviewId,
                    d.CustomerCountry,
                    d.CustomerId,
                    d.ReviewTitle,
                    d.ApplicationUserId  //¿Pq es un string????
                    ))
                .ToListAsync();

            return Ok(reviews);
        }




    }
}
