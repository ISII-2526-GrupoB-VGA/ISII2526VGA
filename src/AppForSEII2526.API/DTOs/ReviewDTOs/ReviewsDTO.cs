
namespace AppForSEII2526.API.DTOs.ReviewDTOs

{
    public class ReviewsDTO //Esyo es para hcer una prueba de si se crea  o no la reseña
    {

        public int ReviewID { get; set; }       
        public string CustomerCountry { get; set; }
        public int CustomerId { get; set; }
        //public string DateOfReviewO { get; set; }
        public string ReviewTitle{ get; set; }
        public string AplicationUserId { get; set; } //Lo cambio a string para q furule

        public ReviewsDTO(int reviewId, string customerCountry, int customerID, string reviewTitle, string apliacationUserId)
        {
            ReviewID = reviewId;
            CustomerCountry = customerCountry;
            CustomerId = customerID;
            ReviewTitle = reviewTitle;
            AplicationUserId = apliacationUserId;
        }

        public override bool Equals(object? obj)
        {
            return obj is ReviewsDTO dTO &&
                   ReviewID == dTO.ReviewID &&
                   CustomerCountry == dTO.CustomerCountry &&
                   CustomerId == dTO.CustomerId &&
                   ReviewTitle == dTO.ReviewTitle &&
                   AplicationUserId == dTO.AplicationUserId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ReviewID, CustomerCountry, CustomerId, ReviewTitle, AplicationUserId);
        }

        


    }
}