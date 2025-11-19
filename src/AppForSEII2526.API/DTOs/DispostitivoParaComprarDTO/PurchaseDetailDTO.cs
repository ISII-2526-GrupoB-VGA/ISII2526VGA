namespace AppForSEII2526.API.DTOs.PurchaseDTOs
{
    public class PurchaseDetailDTO
    {
        public PurchaseDetailDTO(int id, DateTime purchaseDate, string customerFullName,
                                 string deliveryAddress, double totalPrice, int totalQuantity,
                                 List<PurchaseItemDTO> items)
        {
            Id = id;
            PurchaseDate = purchaseDate;
            CustomerFullName = customerFullName;
            DeliveryAddress = deliveryAddress;
            TotalPrice = totalPrice;
            TotalQuantity = totalQuantity;
            Items = items;
        }

        public int Id { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string CustomerFullName { get; set; } = default!;
        public string DeliveryAddress { get; set; } = default!;
        public double TotalPrice { get; set; }
        public int TotalQuantity { get; set; }
        public List<PurchaseItemDTO> Items { get; set; } = new();


        protected bool CompararDate(DateTime d1, DateTime d2)
        {
            return (d1 - d2).Duration() < TimeSpan.FromMinutes(1);
        }

        public override bool Equals(object? obj)
        {
            return obj is PurchaseDetailDTO dto &&
                   Id == dto.Id &&
                   CompararDate(PurchaseDate, dto.PurchaseDate) &&
                   //PurchaseDate == dto.PurchaseDate &&
                   CustomerFullName == dto.CustomerFullName &&
                   DeliveryAddress == dto.DeliveryAddress &&
                   TotalPrice == dto.TotalPrice &&
                   TotalQuantity == dto.TotalQuantity &&
                   Items.SequenceEqual(dto.Items);
        }

        public override int GetHashCode()
        => HashCode.Combine(
            Id,
            CustomerFullName,
            DeliveryAddress,
            TotalPrice,
            TotalQuantity,
            Items?.Count ?? 0
        );

    }
}
