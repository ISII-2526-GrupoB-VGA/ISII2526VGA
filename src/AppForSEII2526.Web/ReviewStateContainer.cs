using AppForSEII2526.API.DTOs.DeviceDTOs;
using AppForSEII2526.API.Models;


namespace AppForSEII2526.Web
{
    public class ReviewStateContainer
    {


        //Creamos una instancia de Review cuando se crea una instancia de ReviewStateContainer
        //Se crea reseña vacía
        public ReviewForCreateDTO Review { get; private set; } = new ReviewForCreateDTO()
        {
            ReviewItems = new List<ReviewItemForCreateDTO>()
        };

        public event Action? OnChange; //¿Esto pa q vrgs sirve?

        private void NotifyStateChanged() => OnChange?.Invoke(); //Tampoco se pa q sirve


        //Añadir review a lista 
        public void AddDeviceToReview(DeviceForReviewDTO device)
        {
            if  (!Review.ReviewItems.Any(ri => ri.DeviceId == device.Id)){
                //Lo añadimos x si no está en la lista
                Review.ReviewItems.Add(new ReviewItemForCreateDTO()
                {

                    DeviceId = device.Id,
                    Comment = string.Empty,
                    Rating = 0
                });
            }

        }

        //Eliminar un dispositivo del carrito
        public void RemoveDeviceFromReview(ReviewItemForCreateDTO item)
        {
            Review.ReviewItems.Remove(item);
            NotifyStateChanged(); //No se si debo ponerlo o quitarlo
        }


        //we eliminate all the movies from the list
        public void ClearReviewCart()
        {
            Review.ReviewItems.Clear();
            NotifyStateChanged();
        }

        public void ReviewProcessed()
        {
            //Creamos nuevo objeto sin datos despues de finalizar la review
            Review = new ReviewForCreateDTO()
            {
                ReviewItems = new List<ReviewItemForCreateDTO>()
            };
        }

    }
}
