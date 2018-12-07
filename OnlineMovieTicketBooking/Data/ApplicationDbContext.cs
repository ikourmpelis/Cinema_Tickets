using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineMovieTicketBooking.Models;

namespace OnlineMovieTicketBooking.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<BookingTable> BookingTable { get; set; }
        public DbSet<Cart> Cart{ get; set; }
        public DbSet<MovieDetails> MovieDetails { get; set; }
        public DbSet<Hall1> Hall1 { get; set; }
        public DbSet<UserProfile> UserProfile { get; set; }
        public DbSet<Rates> Rates { get; set; }
        public DbSet<Halls> Halls { get; set; }
        public DbSet<MovieML> MovieML { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
