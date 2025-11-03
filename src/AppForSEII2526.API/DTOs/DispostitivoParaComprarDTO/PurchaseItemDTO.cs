namespace AppForSEII2526.API.DTOs.PurchaseDTOs
{
    public class PurchaseItemDTO
    {
        // Para POST (entrada)
        [JsonConstructor]
        public PurchaseItemDTO(int deviceID, int quantity, string? description = "")
        {
            DeviceID = deviceID;
            Quantity = quantity;
            Description = description;
        }

        // Para GET (salida)
        public PurchaseItemDTO(int deviceID, string brand, string model, string color,
                               double price, int quantity, string? description = "")
        {
            DeviceID = deviceID;
            Brand = brand;
            Model = model;
            Color = color;
            Price = price;
            Quantity = quantity;
            Description = description;
        }

        // Requeridos en POST
        public int DeviceID { get; set; }
        [System.ComponentModel.DataAnnotations.Range(1, int.MaxValue)]
        public int Quantity { get; set; }
        public string? Description { get; set; }

        // Solo salida (opcionales en POST)
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public string? Color { get; set; }
        public double Price { get; set; }

        public override bool Equals(object? obj) =>
            obj is PurchaseItemDTO dto &&
            DeviceID == dto.DeviceID && Quantity == dto.Quantity &&
            Description == dto.Description && Brand == dto.Brand &&
            Model == dto.Model && Color == dto.Color && Price == dto.Price;

        public override int GetHashCode() =>
            HashCode.Combine(DeviceID, Quantity, Description, Brand, Model, Color, Price);
    }
}
