using Microsoft.AspNetCore.Identity;

namespace AppForSEII2526.API.Models;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser {
    [Display(Name = "Name")]
    [Required]
    public string FirstName { get; set; }
    [Display(Name = "Last Name")]
    public string LastName { get; set; }
    [Required]
    public string Address { get; set; }
    [System.ComponentModel.DataAnnotations.EmailAddress]
    [Required]
    public string email { get; set; }

    [System.ComponentModel.DataAnnotations.Phone]
    public int phoneNumber { get; set; }

    public List<Purchase> Purchases { get; set; } = new();
    public List<Review> Reviews{ get; set; } = new();

    public ApplicationUser() {
    }

    public ApplicationUser(string firstName, string lastName, string address, string email, int phoneNumber) {
        FirstName = firstName;
        LastName = lastName;
        Address = address;
        this.email = email;
        this.phoneNumber = phoneNumber;
    }



}