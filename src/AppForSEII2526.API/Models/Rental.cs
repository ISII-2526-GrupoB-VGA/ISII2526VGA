using System.Globalization;

namespace AppForSEII2526.API.Models
{
    public class Rental
    {

        public string DeliverAddress { get; set; }
        public  int Id { get; set; }
        public string Name { get; set; }

        public PaymentMethodType PaymentMethod { get; set; }
        public DateTime RentalDate { get; set; }
        public DateTime RentalDateFrom { get; set; }
        public DateTime RentalDateTo { get; set; }
        public string Surname { get; set; }
        public double Price { get; set; }
        public IList<RentDevice> RentalDevices { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public Rental() { }
        public Rental(int id, string name, string surname, DateTime rentalDate, DateTime rentalDateFrom, DateTime rentalDateTo, double price, string deliverAddress)
        {
            Id = id;
            Name = name;
            Surname = surname;
            RentalDate = rentalDate;
            RentalDateFrom = rentalDateFrom;
            RentalDateTo = rentalDateTo;
            Price = price;
            DeliverAddress = deliverAddress;
        }
        public Rental(string name, string surname, DateTime rentalDate, DateTime rentalDateFrom, DateTime rentalDateTo, double price, string deliverAddress)
        {
            Name = name;
            Surname = surname;
            RentalDate = rentalDate;
            RentalDateFrom = rentalDateFrom;
            RentalDateTo = rentalDateTo;
            Price = price;
            DeliverAddress = deliverAddress;
        }
        public Rental(int id)
        {
            Id = id;
        }
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            Rental other = (Rental)obj;
            return Id == other.Id && Name == other.Name && Surname == other.Surname && RentalDate == other.RentalDate && RentalDateFrom == other.RentalDateFrom && RentalDateTo == other.RentalDateTo && Price == other.Price && DeliverAddress == other.DeliverAddress;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, Surname, RentalDate, RentalDateFrom, RentalDateTo, Price, DeliverAddress);
        }

        public enum PaymentMethodType
        {
            CreditCard,
            PayPal,
            Cash
        }

    }
}
