using System;

public class ReviewItem
{


    public string Comment { get; set; }

    public int DeviceId { get; set; }
    public Device Device { get; set; }

    public int Id { get; set; }

    [Precision(2, 1)] //4.4, 3.5, 2.0...
    [Range(1, 5)]
    public float Rating { get; set; }

    public int ReviewId { get; set; }
    public Review Review { get; set; }

    public ReviewItem()
    {
    }

    public ReviewItem(string comment, int deviceId, float rating, int reviewId)
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
            && Id == other.Id
            && Rating.Equals(other.Rating)
            && string.Equals(Comment, other.Comment, StringComparison.Ordinal)
            && ReviewId == other.ReviewId;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Comment, DeviceId, Id, Rating, ReviewId);
    }
}
