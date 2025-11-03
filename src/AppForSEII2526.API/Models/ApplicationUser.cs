using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppForSEII2526.API.Models
{
    
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "Name")] //¿Quitar?
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Display(Name = "Last Name")] 
        public string? LastName { get; set; } 

        //public string userName { get; set; } = string.Empty;    //Esto lo puse ahora   
        //public string userSurname { get; set; } = string.Empty; //Esto lo puse ahora

        [Required]
        public string Address { get; set; } = string.Empty;

       

        public List<Purchase> Purchases { get; set; } = new();
        public List<Review> Reviews { get; set; } = new();
        public List<Rental> Rentals { get; set; } = new();

        public ApplicationUser() { }

        public ApplicationUser(
            string firstName,
            string? lastName,
            string address,
            string email,
            string? phoneNumber)
        {
            FirstName = firstName;
            LastName = lastName;
            Address = address;

            
            Email = email;
            PhoneNumber = phoneNumber;
        }
    }
}
