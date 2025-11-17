using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
// enum anidado de tu modelo:
using PaymentMethodType = AppForSEII2526.API.Models.Purchase.PaymentMethodType;

namespace AppForSEII2526.API.DTOs.AlquilarDTOs
{
    public class AlquilarForCreateDTO
    {
        public AlquilarForCreateDTO(
            string customerUserName,
            string customerFirstName,
            string customerLastName,
            string deliveryAddress,
            PaymentMethodType paymentMethod,
            IList<AlquilarItemDTO> purchaseItems)
        {
            CustomerUserName = customerUserName ?? throw new ArgumentNullException(nameof(customerUserName));
            CustomerFirstName = customerFirstName ?? throw new ArgumentNullException(nameof(customerFirstName));
            CustomerLastName = customerLastName ?? throw new ArgumentNullException(nameof(customerLastName));
            DeliveryAddress = deliveryAddress ?? throw new ArgumentNullException(nameof(deliveryAddress));
            PaymentMethod = paymentMethod;
            PurchaseItems = purchaseItems ?? throw new ArgumentNullException(nameof(purchaseItems));
        }

        public AlquilarForCreateDTO() => PurchaseItems = new List<AlquilarItemDTO>();

        [EmailAddress, Required] public string CustomerUserName { get; set; } = default!;
        [Required, StringLength(30, MinimumLength = 2)] public string CustomerFirstName { get; set; } = default!;
        [Required, StringLength(30, MinimumLength = 2)] public string CustomerLastName { get; set; } = default!;

        [DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
        [Display(Name = "Delivery Address")]
        [StringLength(80, MinimumLength = 10)]
        [Required(AllowEmptyStrings = false)]
        public string DeliveryAddress { get; set; } = default!;

        [Required] public PaymentMethodType PaymentMethod { get; set; }

        [MinLength(1)] public IList<AlquilarItemDTO> PurchaseItems { get; set; }

        [Display(Name = "Total Quantity")]
        [JsonPropertyName("TotalQuantity")]
        public int TotalQuantity => PurchaseItems?.Sum(pi => pi.Quantity) ?? 0;

        public override bool Equals(object? obj) =>
            obj is AlquilarForCreateDTO dto &&
            CustomerUserName == dto.CustomerUserName &&
            CustomerFirstName == dto.CustomerFirstName &&
            CustomerLastName == dto.CustomerLastName &&
            DeliveryAddress == dto.DeliveryAddress &&
            PaymentMethod == dto.PaymentMethod &&
            PurchaseItems.SequenceEqual(dto.PurchaseItems) &&
            TotalQuantity == dto.TotalQuantity;

        public override int GetHashCode() =>
            HashCode.Combine(CustomerUserName, CustomerFirstName, CustomerLastName,
                             DeliveryAddress, PaymentMethod, TotalQuantity, PurchaseItems);
    }
}
