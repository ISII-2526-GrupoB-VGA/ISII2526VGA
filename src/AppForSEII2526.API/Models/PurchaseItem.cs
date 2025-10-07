using System;

public class PurchaseItem
{
    [PrimaryKey(nameof(DeviceId), nameof(purchaseId))]
    
	public string Description { get; set; }
	
	public int DeviceId { get; set; }
    [ForeignKey(nameof(DeviceId))]
    public Device Device { get; set; }
    

    public double Price { get; set; }

	public int purchaseId { get; set; }
	[ForeignKey(nameof(purchaseId))]
	public Purchase Purchase { get; set; }

    
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
