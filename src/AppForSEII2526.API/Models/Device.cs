using System;

public class Device
{
	public string Brand { get; set; }
	public string Color { get; set; }
	public int Id { get; set; }
	public string Name { get; set; }
	public double priceForPurchase { get; set; }
	public double priceForRent { get; set; }
	public int quantityForPurchase { get; set; }
	public int quantityForRent { get; set; }
	public IList <RentDevice> RentedDevices { get; set; }
	public IList <ReviewItem> ReviewItems { get; set; }
	public int Year { get; set; }

    public Device()
	{
	}

	public Device(int id, string brand, string color, string name, double priceForPurchase, double priceForRent, int quantityForPurchase, int quantityForRent, int year)
	{
		Id = id;
		Brand = brand;
		Color = color;
		Name = name;
		this.priceForPurchase = priceForPurchase;
		this.priceForRent = priceForRent;
		this.quantityForPurchase = quantityForPurchase;
		this.quantityForRent = quantityForRent;
		Year = year;
    }

	public Device(string brand, string color, string name, double priceForPurchase, double priceForRent, int quantityForPurchase, int quantityForRent, int year)
	{
		Brand = brand;
		Color = color;
		Name = name;
		this.priceForPurchase = priceForPurchase;
		this.priceForRent = priceForRent;
		this.quantityForPurchase = quantityForPurchase;
		this.quantityForRent = quantityForRent;
		Year = year;
    }

    public Device(string color, string name, double priceForPurchase, double priceForRent, int quantityForPurchase, int quantityForRent, int year)
	{
		Color = color;
		Name = name;
		this.priceForPurchase = priceForPurchase;
		this.priceForRent = priceForRent;
		this.quantityForPurchase = quantityForPurchase;
		this.quantityForRent = quantityForRent;
		Year = year;
    }

	public override bool Equals(object obj)
	{
		if (ReferenceEquals(this, obj))
			return true;
		if (obj == null || this.GetType() != obj.GetType())
			return false;

		Device other = (Device)obj;
		return Id == other.Id &&
			   string.Equals(Brand, other.Brand, StringComparison.OrdinalIgnoreCase) &&
			   string.Equals(Color, other.Color, StringComparison.OrdinalIgnoreCase) &&
			   string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase) &&
			   priceForPurchase == other.priceForPurchase &&
			   priceForRent == other.priceForRent &&
			   quantityForPurchase == other.quantityForPurchase &&
			   quantityForRent == other.quantityForRent &&
			   Year == other.Year;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(Id, 
								Brand?.ToLowerInvariant(), 
								Color?.ToLowerInvariant(), 
								Name?.ToLowerInvariant(), 
								priceForPurchase, 
								priceForRent, 
								quantityForPurchase, 
								quantityForRent, 
								Year);
	}
}
