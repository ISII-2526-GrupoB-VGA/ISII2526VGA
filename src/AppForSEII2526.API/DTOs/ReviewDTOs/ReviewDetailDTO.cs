using System;
using System.Collections.Generic;
using System.Linq;

namespace AppForSEII2526.API.DTOs.ReviewDTOs
{
    
    // DTO de GetDetails(paso 7 del caso de uso).
    // Muestra la reseña realizada: datos del cliente(nombre y país), título, fecha,
    // y por cada dispositivo: nombre, modelo, año, puntuación y comentario.
    // NO hereda de ReviewForCreateDTO porque son DTOs con propósitos distintos.
    public class ReviewDetailDTO
    {
        public ReviewDetailDTO()
        {
            ReviewItems = new List<ReviewItemDetailDTO>();
        }

        public ReviewDetailDTO(
            int id,
            DateTime dateOfReview,
            string customerCountry,
            string reviewTitle,
            double overallRating,
            IList<ReviewItemDetailDTO> reviewItems,
            string? customerName = null)
        {
            Id = id;
            DateOfReview = dateOfReview;
            CustomerCountry = customerCountry;
            CustomerName = customerName;
            ReviewTitle = reviewTitle;
            OverallRating = overallRating;
            ReviewItems = reviewItems ?? new List<ReviewItemDetailDTO>();
        }

        // Identificador de la reseña
        public int Id { get; set; }

        // Fecha en que se realizó la reseña (asignada por el servidor)
        public DateTime DateOfReview { get; set; }

        // País desde donde se hizo la reseña
        public string CustomerCountry { get; set; }

        // Nombre del cliente (opcional)
        public string? CustomerName { get; set; }

        // Título de la reseña
        public string ReviewTitle { get; set; }

        // Valoración global (calculada como promedio de los ratings)
        public double OverallRating { get; set; }

        // Lista de ítems reseñados (con info completa del dispositivo)
        public IList<ReviewItemDetailDTO> ReviewItems { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is ReviewDetailDTO dTO &&
                   Id == dTO.Id &&
                   DateOfReview == dTO.DateOfReview &&
                   CustomerCountry == dTO.CustomerCountry &&
                   CustomerName == dTO.CustomerName &&
                   ReviewTitle == dTO.ReviewTitle &&
                   OverallRating == dTO.OverallRating &&
                   ((ReviewItems == null && dTO.ReviewItems == null) ||
                    (ReviewItems != null && dTO.ReviewItems != null && ReviewItems.SequenceEqual(dTO.ReviewItems)));
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, DateOfReview, CustomerCountry, CustomerName, ReviewTitle, OverallRating);
        }
    }

    
    // DTO para cada ítem de reseña en el detalle (GET).
    // Incluye información completa del dispositivo (nombre, modelo, año) además del comentario y rating.
    public class ReviewItemDetailDTO
    {
        public ReviewItemDetailDTO() { }

        public ReviewItemDetailDTO(int deviceId, string deviceName, string deviceModel, int deviceYear, string comment, float rating)
        {
            DeviceId = deviceId;
            DeviceName = deviceName;
            DeviceModel = deviceModel;
            DeviceYear = deviceYear;
            Comment = comment;
            Rating = rating;
        }

        public int DeviceId { get; set; }
        public string DeviceName { get; set; }
        public string DeviceModel { get; set; }
        public int DeviceYear { get; set; }
        public string Comment { get; set; }
        public float Rating { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is ReviewItemDetailDTO dTO &&
                   DeviceId == dTO.DeviceId &&
                   DeviceName == dTO.DeviceName &&
                   DeviceModel == dTO.DeviceModel &&
                   DeviceYear == dTO.DeviceYear &&
                   Comment == dTO.Comment &&
                   Rating == dTO.Rating;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(DeviceId, DeviceName, DeviceModel, DeviceYear, Comment, Rating);
        }
    }
}