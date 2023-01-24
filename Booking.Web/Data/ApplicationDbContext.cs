using Booking.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Booking.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser,IdentityRole,string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<GymClass> GymClass { get; set; }
        public DbSet<ApplicationUserGymClass> ApplicationUserGymClasses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Defined composite key for junction entity (kopplingstabell)
            modelBuilder.Entity<ApplicationUserGymClass>()
               .HasKey(a => new
               {
                   a.GymClassId,
                   a.ApplicationUserId
               });


            modelBuilder.Entity<ApplicationUser>()
                .HasMany(a => a.GymClasses)
                .WithMany(g => g.ApplicationUsers)
                .UsingEntity<ApplicationUserGymClass>(
                  ag => ag.HasOne(ag => ag.GymClass).WithMany(g => g.AttendingMembers),
                  ag => ag.HasOne(ag => ag.ApplicationUser).WithMany(g => g.AttendingClassess));

        }
    }
}