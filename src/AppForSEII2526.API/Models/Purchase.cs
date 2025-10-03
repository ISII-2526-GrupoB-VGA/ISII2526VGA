using System;

public class Puchase
{
	pulic string CustomerUserName { get; set; }
	public string CustomerUserSurname { get; set; }
	public string DeliveryAddress { get; set; }
	public int id { get; set; }
	public PaymentMethodType PaymentMethod { get; set; }
	public DateTime PurchaseDate { get; set; }
	public double TotalPrice { get; set; }
	public int TotalQuantity { get; set; }

    public	Puchase()
	{
	}

	public Puchase(string customerUserName, string customerUserSurname, string deliveryAddress, int id, PaymentMethodType paymentMethod, DateTime purchaseDate, double totalPrice, int totalQuantity)
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

	public Puchase(string customerUserName, string customerUserSurname, string deliveryAddress, PaymentMethodType paymentMethod, DateTime purchaseDate, double totalPrice, int totalQuantity)
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
		Puchase other = (Puchase)obj;
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
