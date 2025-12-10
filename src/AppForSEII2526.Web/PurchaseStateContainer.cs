using AppForSEII2526.Web;
using AppForSEII2526.Web.OpenAPI; //

namespace AppForSEII2526.Web
{
    public class PurchaseStateContainer
    {
        public PurchaseForCreateDTO Purchase { get; private set; } =
            new PurchaseForCreateDTO
            {
                PurchaseItems = new List<PurchaseItemDTO>()
            };

        // Precio total de los dispositivos del carrito
        public decimal TotalPrice =>
            Purchase.PurchaseItems == null
                ? 0m
                : Convert.ToDecimal(Purchase.PurchaseItems.Sum(pi => pi.Price * pi.Quantity));

        // Cantidad total de dispositivos en el carrito
        public int TotalQuantity =>
            Purchase.PurchaseItems == null
                ? 0
                : Purchase.PurchaseItems.Sum(pi => pi.Quantity);

        public event Action? OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();

        // Añadir un dispositivo al carrito de compra
        public void AddDeviceToPurchase(DispositivoComprarDTO device)
        {
            if (Purchase.PurchaseItems == null)
            {
                Purchase.PurchaseItems = new List<PurchaseItemDTO>();
            }

            var existing = Purchase.PurchaseItems
                .FirstOrDefault(pi => pi.DeviceID == device.Id);

            if (existing == null)
            {
                Purchase.PurchaseItems.Add(new PurchaseItemDTO
                {
                    DeviceID = device.Id,          
                    Quantity = 1,
                    Description = string.Empty,    
                    Brand = device.Marca,
                    Model = device.Modelo,
                    Color = device.Color,
                    Price = device.Precio
                });
            }
            else
            {
                existing.Quantity += 1;
            }

            NotifyStateChanged();
        }

        // Quitar un item del carrito
        public void RemovePurchaseItem(PurchaseItemDTO item)
        {
            Purchase.PurchaseItems.Remove(item);
            NotifyStateChanged();
        }

        // Vaciar completamente el carrito de compra
        public void ClearPurchaseCart()
        {
            Purchase.PurchaseItems.Clear();
            NotifyStateChanged();
        }
        public void PurchaseProcessed()
        {
            Purchase = new PurchaseForCreateDTO
            {
                PurchaseItems = new List<PurchaseItemDTO>()
            };
            NotifyStateChanged();
        }
    }
}
