
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

        /*
        private int id;
        private string genre; //Cambio object por string
        private string color;
        //private double priceForPurchase;
        private string nameModel;

        public ReviewItemDTO(int id, string genre)
        {
            this.id = id;
            this.genre = genre;
        }

        public ReviewItemDTO(int id, string genre, string title) : this(id, genre)
        {
            this.id= id;
            this.genre = genre;
            this.Title = title;
        }

        public ReviewItemDTO(int movieID, string title, string genre, double priceForRenting, string description = "")
        {
            MovieID = movieID;
            Title = title;
            //PriceForRenting = priceForRenting;
            Genre = genre;
            Description = description;
        }

        public ReviewItemDTO(int id, string genre, string title, string color, double priceForPurchase, string nameModel) : this(id, genre, title)
        {
            this.color = color;
            //this.priceForPurchase = priceForPurchase;
            this.nameModel = nameModel;
        }

        public int MovieID { get; set; }

        [StringLength(50, ErrorMessage = "Title can't be longer than 50 characters")]
        public string Title { get; set; }


        public double PriceForRenting { get; set; }

        [StringLength(50, ErrorMessage = "Description can't be longer than 50 characters")]
        public string? Description { get; set; }
        [StringLength(50, ErrorMessage = "Genre can't be longer than 50 characters")]
        public string Genre { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is ReviewItemDTO dTO &&
                   MovieID == dTO.MovieID &&
                   Title == dTO.Title &&
                   PriceForRenting == dTO.PriceForRenting &&
                   Description == dTO.Description &&
                   Genre == dTO.Genre;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(MovieID, Title, PriceForRenting, Description, Genre);
        }

        */


    }
}