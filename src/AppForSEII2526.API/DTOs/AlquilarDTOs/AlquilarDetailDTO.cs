namespace AppForSEII2526.API.DTOs.AlquilarDTOs
{
    public class AlquilarDetailDTO
    {
        public AlquilarDetailDTO(int id, DateTime purchaseDate, string customerFullName,
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
        public List<AlquilarItemDTO> Items { get; set; } = new();

        public override bool Equals(object? obj)
        {
            return obj is AlquilarDetailDTO dto &&
                   Id == dto.Id &&
                   PurchaseDate == dto.PurchaseDate &&
                   CustomerFullName == dto.CustomerFullName &&
                   DeliveryAddress == dto.DeliveryAddress &&
                   TotalPrice == dto.TotalPrice &&
                   TotalQuantity == dto.TotalQuantity &&
                   Items.SequenceEqual(dto.Items);
        }

        public override int GetHashCode()
            => HashCode.Combine(Id, PurchaseDate, CustomerFullName, DeliveryAddress, TotalPrice, TotalQuantity, Items);
    }
}
