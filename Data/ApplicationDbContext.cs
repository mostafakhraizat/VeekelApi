using Common.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using VeekelApi.Models;
using VeekelApi.Models.Vehicle;

namespace VeekelApi.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
          : base(options)
        {

        }
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<BrandCompany> BrandCompanies { get; set; }
        public DbSet<MobileSession> MobileSessions { get; set; }
        public DbSet<VehicleListing> VehicleListings { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<ConfigHash> ConfigHashes { get; set; } 
        public DbSet<VehicleImage> VehicleImages { get; set; }

    }
}
 