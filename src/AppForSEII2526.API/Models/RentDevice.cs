namespace AppForSEII2526.API.Models
{
    public class RentDevice
    {
        public int DeviceID { get; set; }
        public int RentID { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }


        public Rental Rent  { get; set; }


        public Device Device { get; set; }

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
