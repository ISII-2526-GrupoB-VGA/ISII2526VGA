using System;

public class Model
{
	public int Id { get; set; }
	public string NameModel { get; set; }
	
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

	public override bool Equals(object obj)
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
