using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.Api.Data
{
    public class HotelListingDbContext : IdentityDbContext<ApplicationUser>
    {
        // can use this or genrate Primary Constructor using code snippet (ctr+. on Constructur name)
        public HotelListingDbContext(DbContextOptions<HotelListingDbContext> options) : base(options)
        {
        }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<ApiKey> ApiKeys { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ApiKey>
                ().HasIndex(k => k.Key).IsUnique();

            base.OnModelCreating(builder);
        }

    }
}
