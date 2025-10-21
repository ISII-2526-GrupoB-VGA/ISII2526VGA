using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppForSEII2526.API.Models
{
    public class Purchase : IEquatable<Purchase>
    {
        [Required]
        public string DeliveryAddress { get; set; }
        public int id { get; set; }

        [Required]
        public PaymentMethodType PaymentMethod { get; set; }

        public DateTime PurchaseDate { get; set; }

        [Required]
        public double TotalPrice { get; set; }

        [Required]
        public int TotalQuantity { get; set; }

        public List<PurchaseItem> PurchaseItems { get; set; } = new List<PurchaseItem>();

        public ApplicationUser ApplicationUser { get; set; }

        // Enum de método de pago
        public enum PaymentMethodType
        {
            CreditCard,
            PayPal,
            Cash
        }

        public Purchase() { }

        public Purchase(string deliveryAddress, int id, PaymentMethodType paymentMethod,
                        DateTime purchaseDate, double totalPrice, int totalQuantity)
        {
            DeliveryAddress = deliveryAddress;
            this.id = id;
            PaymentMethod = paymentMethod;
            PurchaseDate = purchaseDate;
            TotalPrice = totalPrice;
            TotalQuantity = totalQuantity;
        }

        public Purchase(string deliveryAddress, PaymentMethodType paymentMethod,
                        DateTime purchaseDate, double totalPrice, int totalQuantity)
        {
            DeliveryAddress = deliveryAddress;
            PaymentMethod = paymentMethod;
            PurchaseDate = purchaseDate;
            TotalPrice = totalPrice;
            TotalQuantity = totalQuantity;
        }

        public override bool Equals(object obj) => Equals(obj as Purchase);

        public bool Equals(Purchase other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (other is null) return false;

            // Igualdad por Id si ambos lo tienen asignado
            if (this.id != 0 && other.id != 0)
                return this.id == other.id;

            // Igualdad por valor cuando no hay Id
            return string.Equals(this.DeliveryAddress, other.DeliveryAddress, StringComparison.OrdinalIgnoreCase)
                && this.PaymentMethod == other.PaymentMethod
                && this.PurchaseDate == other.PurchaseDate
                && this.TotalPrice.Equals(other.TotalPrice)
                && this.TotalQuantity == other.TotalQuantity
                && string.Equals(this.ApplicationUser?.Id, other.ApplicationUser?.Id, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            if (id != 0) return id.GetHashCode();

            var addr = DeliveryAddress?.ToLowerInvariant();
            var userId = ApplicationUser?.Id?.ToLowerInvariant();

            return HashCode.Combine(addr, PaymentMethod, PurchaseDate, TotalPrice, TotalQuantity, userId);
        }

    }
}
