namespace AppForSEII2526.API.DTOs.DeviceDTOs
{
    public class DispositivoComprarDTO
    {
        public DispositivoComprarDTO(int id, string nombre, string marca, string modelo, string color, double precio)
        {
            Id = id;
            Nombre = nombre;
            Marca = marca;
            Modelo = modelo;   // <- Model.NameModel
            Color = color;
            Precio = precio;   // <- Device.priceForPurchase
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Color { get; set; }
        public double Precio { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is DispositivoComprarDTO dto &&
                   Id == dto.Id &&
                   Nombre == dto.Nombre &&
                   Marca == dto.Marca &&
                   Modelo == dto.Modelo &&
                   Color == dto.Color &&
                   Precio == dto.Precio;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Nombre, Marca, Modelo, Color, Precio);
        }
    }
}
