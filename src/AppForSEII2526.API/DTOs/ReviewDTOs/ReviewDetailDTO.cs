namespace AppForSEII2526.API.DTOs.ReviewDTOs
{
    public class ReviewDetailDTO : ReviewForCreateDTO  // DTO de GetDetails (paso  7). DTO de lectura
                                                    // que puede heredar del DTO de creación POST
    {
        public ReviewDetailDTO(int id,
                            DateTime dateOfReview, 
                            string customerCountry,
                            int customerId,
                            string reviewTitle,
                            double overallRating,
                            IList<ReviewItemForCreateDTO> reviewItems,
                            string? customerName = null)
            : base(customerCountry,
                   customerId,
                   dateOfReview,
                   overallRating,
                   id,
                   reviewTitle,
                   customerName,
                   reviewItems)
        {
            Id = id;
            DateOfReview = dateOfReview;
        }

        // Identificador de la reseña (para lectura)
        public int Id { get; set; }

        // Fecha en que se realizó la reseña (para lectura)
        public DateTime DateOfReview { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is ReviewDetailDTO dTO &&
                   Id == dTO.Id &&
                   DateOfReview == dTO.DateOfReview &&
                   CustomerCountry == dTO.CustomerCountry &&
                   CustomerName == dTO.CustomerName &&
                   CustomerId == dTO.CustomerId &&
                   ReviewTitle == dTO.ReviewTitle &&
                   OverallRating == dTO.OverallRating &&
                   ((ReviewItems == null && dTO.ReviewItems == null) ||
                    (ReviewItems != null && dTO.ReviewItems != null && ReviewItems.SequenceEqual(dTO.ReviewItems)));
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Id, DateOfReview);
        }
    }
}