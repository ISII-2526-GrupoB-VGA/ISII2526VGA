using AppForSEII2526.API.Models;
using System;
using System.Linq;

namespace AppForSEII2526.API.DTOs.ReviewDTOs
{
    public class ReviewForCreateDTO        // DTO para crear una reseña completa
                                           // DTO usado en el POST (Crear reseña), Solo contiene la
                                           // informacion que cliente introduce en paso 5
    {


        public ReviewForCreateDTO()
        {
            ReviewItems = new List<ReviewItemForCreateDTO>();
        }
        /*
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
        */
        // --- Cabecera de la reseña ---
        // País desde donde se hace la reseña (obligatorio según flujo)
        [Required(AllowEmptyStrings = false, ErrorMessage = "El país es obligatorio")]
        public string CustomerCountry { get; set; }

        // Nombre del cliente (opcional según flujo paso 5)
        public string? CustomerName { get; set; }


        // Título de la reseña (obligatorio según flujo)
        [Required(AllowEmptyStrings = false, ErrorMessage = "El título de la reseña es obligatorio")]
        public string ReviewTitle { get; set; }

        // Lista de ítems: por cada dispositivo seleccionado en el carrito
        [Required]
        [MinLength(1, ErrorMessage = "Debe reseñar al menos un dispositivo")]
        public IList<ReviewItemForCreateDTO> ReviewItems { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is ReviewForCreateDTO dTO &&
                   CustomerCountry == dTO.CustomerCountry &&
                   CustomerName == dTO.CustomerName &&
                   ReviewTitle == dTO.ReviewTitle &&
                   EqualityComparer<IList<ReviewItemForCreateDTO>>.Default.Equals(ReviewItems, dTO.ReviewItems);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(CustomerCountry, CustomerName, ReviewTitle, ReviewItems);
        }
    }

    // DTO para cada dispositivo reseñado (comentario + puntuación obligatoria)
    public class ReviewItemForCreateDTO
    {

        /*
        public ReviewItemForCreateDTO(int deviceId, string name, string model, int year, string comment, float rating)
        {
            DeviceId = deviceId;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Model = model ?? throw new ArgumentNullException(nameof(model));
            Year = year;
            Comment = comment ?? throw new ArgumentNullException(nameof(comment));
            Rating = rating;
        }
        */

        public ReviewItemForCreateDTO() { }

        // ID del dispositivo reseñado (obligatorio)
        [Required]
        public int DeviceId { get; set; }

        // Comentario por dispositivo (obligatorio según flujo paso 5)
        [Required(AllowEmptyStrings = false, ErrorMessage = "El comentario es obligatorio")]
        public string Comment { get; set; }

        // Puntuación 1..5 (obligatorio según flujo paso 5 y flujo alternativo 5)
        [Range(1, 5, ErrorMessage = "La puntuación debe estar entre 1 y 5")]
        public float Rating { get; set; }

        // NOTAS:
        // - Name, Model, Year NO se piden en el POST (ya están en la BD asociados al DeviceId)
        // - Solo se usan en el GET (paso 7) para mostrar info del dispositivo
    }
}