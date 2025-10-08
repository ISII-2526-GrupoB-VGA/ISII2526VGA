using System;
using System.Collections.Generic;
namespace AppForSEII2526.API.Models;

public class Purchase
{
    [Required]
    public string CustomerUserName { get; set; }
	public string CustomerUserSurname { get; set; }
    [Required]
    public string DeliveryAddress { get; set; }
    public int id { get; set; }
    [Required]
    public PaymentMethodType PaymentMethod { get; set; }
	public DateTime PurchaseDate { get; set; }
    [Required]
    public double TotalPrice { get; set; }
    [Required]
    public int TotalQuantity { get; set; }
    public List<PurchaseItem> PurchaseItems { get; set; } = new List<PurchaseItem>();

    public ApplicationUser ApplicationUser { get; set; }

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
