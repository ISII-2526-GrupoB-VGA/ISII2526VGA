using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AppForSEII2526.API.Models;

namespace AppForSEII2526.API.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{

    public DbSet<Device> Devices { get; set; }
    public DbSet<Purchase> Purchases { get; set; }
    public DbSet<PurchaseItem> PurchaseItems { get; set; }
    public DbSet<ReviewItem> ReviewItems { get; set; }
    public DbSet<Model> Models { get; set; }
    public DbSet<RentDevice> RentedDevices { get; set; }
    public DbSet<Rental> Rentals { get; set; }
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<Review> Reviews { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);


        builder.Entity<RentDevice>()
            .HasKey(rd => new { rd.DeviceID, rd.RentID });
        builder.Entity<PurchaseItem>()
            .HasKey(pi => new { pi.DeviceId, pi.purchaseId });
       
    }
}

