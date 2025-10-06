using System;

public class ReviewItem
{

    [PrimaryKey(nameof(DeviceId), nameof(ReviewId))]

    public int DeviceId { get; set; }

    public string Comment { get; set; }

    [ForeignKey(nameof(DeviceId))]


    public int ID { get; set; }
    public float Rating { get; set; }  
    [ForeignKey(nameof(ReviewId))]
    public string ReviewId { get; set; }

    public Review Review { get; set; } //Esto añadí sin saber muy bien porque
    public Device Device { get; set; } //Esto tampoco se muy bien pq, pero he copiado lo de PurchaseItem
                                       //Borrar estos comentarios tras corregirlos
    public ReviewItem()
    {
    }

    public ReviewItem(string comment, int deviceId, float rating, string reviewId)
    {
        DeviceId = deviceId;
        Comment = comment;
        Rating = rating;
        ReviewId = reviewId;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(this, obj))
            return true;

        if (obj == null || GetType() != obj.GetType())
            return false;

        ReviewItem other = (ReviewItem)obj;

        return DeviceId == other.DeviceId
            && ID == other.ID
            && Rating.Equals(other.Rating)
            && string.Equals(Comment, other.Comment, StringComparison.Ordinal)
            && string.Equals(ReviewId, other.ReviewId, StringComparison.Ordinal);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Comment, DeviceId, ID, Rating, ReviewId);
    }
}
