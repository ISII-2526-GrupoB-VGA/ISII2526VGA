namespace AppForSEII2526.API.DTOs.DeviceDTOs
{
    public class DeviceForReviewDTO //El de getSelect (paso2). 
    {
        public DeviceForReviewDTO()
        {
        }

        public DeviceForReviewDTO(int id, string name, string brand, string color, int year, string model)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Brand = brand ?? throw new ArgumentNullException(nameof(brand));
            Color = color ?? throw new ArgumentNullException(nameof(color));
            Year = year;
            Model = model ?? throw new ArgumentNullException(nameof(model));
        }

        public int Id { get; set; }

        [StringLength(50, ErrorMessage = "Name must have a maximum length of 50 characters")]
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        [StringLength(50, ErrorMessage = "Brand must have a maximum length of 50 characters")]
        [Required(AllowEmptyStrings = false)]
        public string Brand { get; set; }

        [StringLength(30, ErrorMessage = "Color must have a maximum length of 30 characters")]
        public string Color { get; set; }

        [Range(1900, 2100, ErrorMessage = "Year must be a valid year")]
        public int Year { get; set; }

        [StringLength(50, ErrorMessage = "Model must have a maximum length of 50 characters")]
        [Required(AllowEmptyStrings = false)]
        public string Model { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is DeviceForReviewDTO dTO &&
                   Id == dTO.Id &&
                   string.Equals(Name, dTO.Name, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(Brand, dTO.Brand, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(Color, dTO.Color, StringComparison.OrdinalIgnoreCase) &&
                   Year == dTO.Year &&
                   string.Equals(Model, dTO.Model, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id,
                                    Name?.ToLowerInvariant(),
                                    Brand?.ToLowerInvariant(),
                                    Color?.ToLowerInvariant(),
                                    Year,
                                    Model?.ToLowerInvariant());
        }
    }
}