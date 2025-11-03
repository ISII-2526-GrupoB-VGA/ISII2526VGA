using System;
namespace AppForSEII2526.API.Models;

public class Model
{
	public int Id { get; set; }
    [Required]
    public string NameModel { get; set; }
	public List<Device> Devices { get; set; } = new List<Device>();

    public Model() { }

	public Model(int id, string NameModel)
	{
		Id = id; 
		NameModel = NameModel;
	}

	public Model(string NameModel)
	{
		NameModel = NameModel;
	}

	public Model(int Id)
	{
		Id = Id;
	}

    public Model(int id, string nameModel, List<Device> devices) : this(id, nameModel)
    {
        Devices = devices;
    }

    public override bool Equals(object? obj)
	{
		if (obj == null || GetType() != obj.GetType())
			return false;

		Model other = (Model)obj;
		return Id == other.Id && NameModel == other.NameModel;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(Id, NameModel);
	}
}
