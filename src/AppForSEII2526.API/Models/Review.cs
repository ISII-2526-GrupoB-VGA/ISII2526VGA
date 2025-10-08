using System;

public class Review
{
    public int CustomerCount { get; set; }
    public int CustomerId { get; set; }
    public DateTime DateOfReview { get; set; }
    public double OverallRating { get; set; }
    [System.ComponentModel.DataAnnotations.Required]
    
    public int ReviewId { get; set; }
    public string ReviewTitle { get; set; }

    public Review()
    {
    }

    public Review(int customerCount, int customerId, DateTime dateOfReview, double overallRating, int reviewId, string reviewTitle)
    {
        CustomerCount = customerCount;
        CustomerId = customerId;
        DateOfReview = dateOfReview;
        OverallRating = overallRating;
        ReviewId = reviewId;
        ReviewTitle = reviewTitle;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(this, obj))
            return true;
        if (obj == null || this.GetType() != obj.GetType())
            return false;

        Review other = (Review)obj;

        return CustomerCount == other.CustomerCount &&
               CustomerId == other.CustomerId &&
               DateOfReview.Equals(other.DateOfReview) &&
               OverallRating.Equals(other.OverallRating) &&
               ReviewId == other.ReviewId &&
               string.Equals(ReviewTitle, other.ReviewTitle, StringComparison.OrdinalIgnoreCase);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(CustomerCount,
                                CustomerId,
                                DateOfReview,
                                OverallRating,
                                ReviewId,
                                ReviewTitle?.ToLowerInvariant());
    }
}
