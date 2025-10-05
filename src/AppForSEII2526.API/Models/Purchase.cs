using System;

public class Purchase
{

	public string CustomerUserName { get; set; }
	public string CustomerUserSurname { get; set; }
	public string DeliveryAddress { get; set; }
	[Key]
    public int id { get; set; }
	public PaymentMethodType PaymentMethod { get; set; }
	public DateTime PurchaseDate { get; set; }
	public double TotalPrice { get; set; }
	public int TotalQuantity { get; set; }

    public	Purchase()
	{
	}

	public Purchase(string customerUserName, string customerUserSurname, string deliveryAddress, int id, PaymentMethodType paymentMethod, DateTime purchaseDate, double totalPrice, int totalQuantity)
	{
		CustomerUserName = customerUserName;
		CustomerUserSurname = customerUserSurname;
		DeliveryAddress = deliveryAddress;
		this.id = id;
		PaymentMethod = paymentMethod;
		PurchaseDate = purchaseDate;
		TotalPrice = totalPrice;
		TotalQuantity = totalQuantity;
    }

	public Purchase(string customerUserName, string customerUserSurname, string deliveryAddress, PaymentMethodType paymentMethod, DateTime purchaseDate, double totalPrice, int totalQuantity)
	{
		CustomerUserName = customerUserName;
		CustomerUserSurname = customerUserSurname;
		DeliveryAddress = deliveryAddress;
		PaymentMethod = paymentMethod;
		PurchaseDate = purchaseDate;
		TotalPrice = totalPrice;
		TotalQuantity = totalQuantity;
    }

	public equals override bool Equals(object obj)
	{
		if (obj == null || GetType() != obj.GetType())
		{
			return false;
		}
		Purchase other = (Purchase)obj;
		return CustomerUserName == other.CustomerUserName &&
			   CustomerUserSurname == other.CustomerUserSurname &&
			   DeliveryAddress == other.DeliveryAddress &&
			   id == other.id &&
			   PaymentMethod == other.PaymentMethod &&
			   PurchaseDate == other.PurchaseDate &&
			   TotalPrice == other.TotalPrice &&
			   TotalQuantity == other.TotalQuantity;
    }

	public override int GetHashCode()
		{
		return HashCode.Combine(CustomerUserName, CustomerUserSurname, DeliveryAddress, id, PaymentMethod, PurchaseDate, TotalPrice, TotalQuantity);
    }

}
