namespace AppForSEII2526.API.DTOs.ReviewDTOs

{
    public class ReviewItemDTO
    {

        public int Id { get; set; }       //Es el común o ykse
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Color { get; set; }
        public int year { get; set; }
        public string Model { get; set; }

        public ReviewItemDTO(int id, string nombre, string marca, string color, int año, string modelo)
        {
            Id = id;
            Name = nombre;
            Brand = marca;
            Color = color;
            year = año;
            Model = modelo;
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