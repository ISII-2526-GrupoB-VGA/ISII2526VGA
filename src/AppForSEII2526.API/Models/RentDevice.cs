namespace AppForSEII2526.API.Models
{
    public class RentDevice
    {
        [Required]
        public int DeviceID { get; set; }
        [Required]
        public int RentID { get; set; }
        [Required]
        [Range(1, 10000, ErrorMessage = "Debes elegir entre 1 disposito o 10000")]
        public int Quantity { get; set; }
        [Required]
        [Range(0.01, 10000.00, ErrorMessage = "El precio debe estar entre 0.01 y 10000.00")]
        public double Price { get; set; }
        public RentDevice() { }
        public RentDevice(int deviceID, int rentID, int quantity, double price)
        {
            DeviceID = deviceID;
            RentID = rentID;
            Quantity = quantity;
            Price = price;
        }
        public RentDevice(int deviceID, int rentID)
        {
            DeviceID = deviceID;
            RentID = rentID;
        }   
        public RentDevice(int deviceID)
        {
            DeviceID = deviceID;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            RentDevice other = (RentDevice)obj;
            return DeviceID == other.DeviceID && RentID == other.RentID && Quantity == other.Quantity && Price == other.Price;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(DeviceID, RentID, Quantity, Price);
        }


    }
}
