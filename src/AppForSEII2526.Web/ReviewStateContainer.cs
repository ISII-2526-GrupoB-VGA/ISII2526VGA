using System;
using System.Collections.Generic;
using System.Linq;
using AppForSEII2526.Web.OpenAPI; //

namespace AppForSEII2526.Web
{
    // Servicio inyectable para mantener el estado de la reseña en el cliente Blazor
    public class ReviewStateContainer
    {
        // DTO principal usado por tu UI (Swagger/NSwag)
        public ReviewForCreateDTO Review { get; private set; }

        public ReviewStateContainer()
        {
            Review = new ReviewForCreateDTO
            {
                CustomerCountry = string.Empty,
                CustomerName = string.Empty,
                ReviewTitle = string.Empty,
                // inicializa la lista vacía para evitar null checks por todas partes
                ReviewItems = new List<ReviewItemForCreateDTO>()
            };
        }

        public event Action? OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();

        // Añade un dispositivo a la reseña (desde el DTO devuelto por el cliente OpenAPI)
        public void AddDeviceToReview(ReviewItemDTO device)
        {
            if (device == null) return;

            if (Review.ReviewItems == null)
                Review.ReviewItems = new List<ReviewItemForCreateDTO>();

            // evitar duplicados por DeviceId
            if (Review.ReviewItems.Any(x => x.DeviceId == device.Id))
                return;

            // Comentario por defecto que cumple la validación del controlador ("Reseña para ...")
            var defaultComment = $"Reseña para {device.Name ?? "dispositivo"}";

            var item = new ReviewItemForCreateDTO
            {
                DeviceId = device.Id,
                Comment = defaultComment,
                // rating por defecto (puedes cambiarlo)
                Rating = 3.0f
            };

            Review.ReviewItems.Add(item);
            NotifyStateChanged();
        }

        // Elimina un item de la reseña (recibe el DTO de creación, que tu componente ya maneja)
        public void RemoveDeviceFromReview(ReviewItemForCreateDTO item)
        {
            if (item == null || Review.ReviewItems == null) return;

            var existing = Review.ReviewItems.FirstOrDefault(x => x.DeviceId == item.DeviceId);
            if (existing != null)
            {
                Review.ReviewItems.Remove(existing);
                NotifyStateChanged();
            }
        }

        // Vacía el carrito / reseña (útil tras crear la reseña)
        public void ClearReview()
        {
            Review = new ReviewForCreateDTO
            {
                CustomerCountry = string.Empty,
                CustomerName = string.Empty,
                ReviewTitle = string.Empty,
                ReviewItems = new List<ReviewItemForCreateDTO>()
            };
            NotifyStateChanged();
        }

        // Opcional: obtener lista de ids
        public List<int> GetDeviceIds() =>
            Review.ReviewItems?.Select(i => i.DeviceId).ToList() ?? new List<int>();
    }
}
