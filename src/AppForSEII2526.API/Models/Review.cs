using System;
namespace AppForSEII2526.API.Models;


public class Review
{

    [System.ComponentModel.DataAnnotations.Required]
    public string? CustomerCountry { get; set; } //"?" indica que puede seer nulo  
    [System.ComponentModel.DataAnnotations.Required]
    public int CustomerId { get; set; }
    public DateTime DateOfReview { get; set; }
    public double OverallRating { get; set; }
    [System.ComponentModel.DataAnnotations.Required]

    public int ReviewId { get; set; }
    [System.ComponentModel.DataAnnotations.Required]
    public string ReviewTitle { get; set; }
    public IList<ReviewItem> ReviewItems { get; set; } = new List<ReviewItem>();

    public ApplicationUser ApplicationUser
    {
        get => default;
        set
        {
        }
    }

    public Review()
    {
    }

    public Review(string CustomerCountry, int customerId, DateTime dateOfReview, double overallRating, int reviewId, string reviewTitle)
    {
        CustomerCountry = CustomerCountry;
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

        return CustomerCountry == other.CustomerCountry &&
               CustomerId == other.CustomerId &&
               DateOfReview.Equals(other.DateOfReview) &&
               OverallRating.Equals(other.OverallRating) &&
               ReviewId == other.ReviewId &&
               string.Equals(ReviewTitle, other.ReviewTitle, StringComparison.OrdinalIgnoreCase);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(CustomerCountry,
                                CustomerId,
                                DateOfReview,
                                OverallRating,
                                ReviewId,
                                ReviewTitle?.ToLowerInvariant());
    }
}

