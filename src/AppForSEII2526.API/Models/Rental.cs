using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;

namespace AppForSEII2526.API.Models
{
    public class Rental
    {
   

        public string DeliverAddress { get; set; }
        [Required]
        public int Id { get; set; }
        [Required]
        public PaymentMethodType PaymentMethod { get; set; }
        [Required]
        public DateTime RentalDate { get; set; }
        [Required]
        public DateTime RentalDateFrom { get; set; }
        [Required]
        public DateTime RentalDateTo { get; set; }
        [Required]
        [Range(0.01, 10000.00, ErrorMessage = "El precio debe estar entre 0.01 y 10000.00")]
        public double Price { get; set; }
        public IList<RentDevice> RentalDevices { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public Rental() { }
        public Rental(int id, DateTime rentalDate, DateTime rentalDateFrom, DateTime rentalDateTo, double price, string deliverAddress)
        {
            Id = id;
            RentalDate = rentalDate;
            RentalDateFrom = rentalDateFrom;
            RentalDateTo = rentalDateTo;
            Price = price;
            DeliverAddress = deliverAddress;
        }
        public Rental(DateTime rentalDate, DateTime rentalDateFrom, DateTime rentalDateTo, double price, string deliverAddress)
        {
          
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
            if (ReferenceEquals(this, obj)) return true;
            if (obj is not Rental other) return false;

            // Si ambos tienen Id asignado, la igualdad se basa solo en el Id.
            if (this.Id != 0 && other.Id != 0)
                return this.Id == other.Id;

            // Igualdad por valor cuando el Id no está disponible.
            return string.Equals(this.DeliverAddress, other.DeliverAddress, StringComparison.OrdinalIgnoreCase)
                && this.PaymentMethod == other.PaymentMethod
                && this.RentalDate == other.RentalDate
                && this.RentalDateFrom == other.RentalDateFrom
                && this.RentalDateTo == other.RentalDateTo
                && this.Price.Equals(other.Price)
                && string.Equals(this.ApplicationUser?.Id, other.ApplicationUser?.Id, StringComparison.OrdinalIgnoreCase);
        }


        public override int GetHashCode()
        {
            if (Id != 0)
                return Id.GetHashCode();

            // Combinar hashes de propiedades utilizadas en Equals
            var address = DeliverAddress?.ToLowerInvariant();
            var userId = ApplicationUser?.Id?.ToLowerInvariant();

            return HashCode.Combine(address, PaymentMethod, RentalDate, RentalDateFrom, RentalDateTo, Price, userId);
        }

        public enum PaymentMethodType
        {
            CreditCard,
            PayPal,
            Cash
        }

    }
}
