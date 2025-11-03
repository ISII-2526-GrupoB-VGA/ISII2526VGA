using AppForSEII2526.API.Models;
using System;
using System.Linq;

namespace AppForSEII2526.API.DTOs.ReviewDTOs
{
    public class ReviewForCreateDTO        // DTO para crear una reseña completa
                                           // DTO usado en el POST (Crear reseña), Solo contiene la
                                           // informacion que cliente introduce en paso 5
    {
        public ReviewForCreateDTO(string customerCountry, int customerId, DateTime dateOfReview, double overallRating, int reviewId, string reviewTitle, string? customerName = null, IList<ReviewItemForCreateDTO>? reviewItems = null)
        {
            CustomerCountry = customerCountry ?? throw new ArgumentNullException(nameof(customerCountry));
            CustomerId = customerId;
            DateOfReview = dateOfReview;
            OverallRating = overallRating;
            ReviewId = reviewId;
            ReviewTitle = reviewTitle ?? throw new ArgumentNullException(nameof(reviewTitle));
            CustomerName = customerName;
            ReviewItems = reviewItems ?? new List<ReviewItemForCreateDTO>();
        }

        // Para deserialización / model binding
        public ReviewForCreateDTO()
        {
            DateOfReview = DateTime.UtcNow;
            ReviewItems = new List<ReviewItemForCreateDTO>();
        }

        // --- Cabecera de la reseña ---
        [Required(AllowEmptyStrings = false)]
        public string CustomerCountry { get; set; }

        // Opcional según el flujo
        public string? CustomerName { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public DateTime DateOfReview { get; set; }

        // Puede calcularse o proporcionarse
        public double OverallRating { get; set; }

        [Required]
        public int ReviewId { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string ReviewTitle { get; set; }

        // Lista de ítems: por cada dispositivo seleccionado en el carrito
        public IList<ReviewItemForCreateDTO> ReviewItems { get; set; }
    }

    // DTO para cada dispositivo reseñado (comentario + puntuación obligatoria)
    public class ReviewItemForCreateDTO
    {
        public ReviewItemForCreateDTO(int deviceId, string name, string model, int year, string comment, float rating)
        {
            DeviceId = deviceId;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Model = model ?? throw new ArgumentNullException(nameof(model));
            Year = year;
            Comment = comment ?? throw new ArgumentNullException(nameof(comment));
            Rating = rating;
        }

        public ReviewItemForCreateDTO() { }

        [Required]
        public int DeviceId { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Model { get; set; }

        public int Year { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Comment { get; set; }

        [Range(1, 5)]
        public float Rating { get; set; }
    }
}