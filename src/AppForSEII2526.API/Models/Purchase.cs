using System;

public class Purchase
{
    [System.ComponentModel.DataAnnotations.Required]
    public string CustomerUserName { get; set; }
	public string CustomerUserSurname { get; set; }
    [System.ComponentModel.DataAnnotations.Required]
    public string DeliveryAddress { get; set; }
    public int id { get; set; }
	public PaymentMethodType PaymentMethod { get; set; }
	public DateTime PurchaseDate { get; set; }
    [System.ComponentModel.DataAnnotations.Precision(10, 2)]
    public double TotalPrice { get; set; }
	public int TotalQuantity { get; set; }
    public IList<PurchaseItem> PurchaseItems { get; set; } = new List<PurchaseItem>();

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

    public override bool Equals(object? obj)
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

    public enum PaymentMethodType
    {
        Cash,
        CreditCard,
        Paypal
    }


}
