using System;
using System.Collections.Generic;
namespace AppForSEII2526.API.Models;

public class Device
{
    [Required]
    public string Brand { get; set; }
	public string Color { get; set; }
	public int Id { get; set; }
	[Required]
	public string Name { get; set; }
    [Range(0.01, 10000.00, ErrorMessage = "El precio debe estar entre 0.01 y 10000.00")]
    [Required]
    public double priceForPurchase { get; set; }
	[Range(0.01, 10000.00, ErrorMessage = "El precio debe estar entre 0.01 y 10000.00")]
	[Required]
    public double priceForRent { get; set; }
	[Required]
	public int quantityForPurchase { get; set; }
	[Required]
    public int quantityForRent { get; set; }
	public List <RentDevice> RentedDevices { get; set; }
	public List <ReviewItem> ReviewItems { get; set; }
	public int Year { get; set; }
	public int ModelId { get; set; }
	public Model Model { get; set; }

	public List<PurchaseItem> PurchaseItems { get; set; } = new List<PurchaseItem>();

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

	public override bool Equals(object? obj)
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
        return HashCode.Combine(
            Id,
            Brand?.ToLowerInvariant(),
            Color?.ToLowerInvariant(),
            Name?.ToLowerInvariant(),
            priceForPurchase,
            priceForRent,
            quantityForPurchase,
            quantityForRent
        );
    }

}
