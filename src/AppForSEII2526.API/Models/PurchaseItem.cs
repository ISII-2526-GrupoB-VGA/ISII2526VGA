using System;
namespace AppForSEII2526.API.Models;

[PrimaryKey(nameof(DeviceId), nameof(purchaseId))]
public class PurchaseItem
{

	
	public string Description { get; set; }

	public int DeviceId { get; set; }
    public Device Device { get; set; }

	[Precision(10, 2)]
    [Required]
    public double Price { get; set; }

	public int purchaseId { get; set; }
	public Purchase Purchase { get; set; }

    [Required]
    public int Quantity { get; set; }

    public PurchaseItem()
	{
	}

	public PurchaseItem(int deviceId, string description, double price, int quantity)
	{
		DeviceId = deviceId;
		Description = description;
		Price = price;
		Quantity = quantity;
    }

	public PurchaseItem(int deviceId, string description, double price, int quantity, int purchaseId)
		{
		DeviceId = deviceId;
		Description = description;
		Price = price;
		Quantity = quantity;
		this.purchaseId = purchaseId;
    }

	public override bool Equals(object obj)
		{
		if (ReferenceEquals(this, obj))
			return true;
		if (obj == null || GetType() != obj.GetType())
			return false;
		PurchaseItem other = (PurchaseItem)obj;
		return DeviceId == other.DeviceId && Description == other.Description && Price == other.Price && Quantity == other.Quantity && purchaseId == other.purchaseId;
    }

	public override int GetHashCode()
		{
		return HashCode.Combine(DeviceId, Description, Price, Quantity, purchaseId);
    }
}
