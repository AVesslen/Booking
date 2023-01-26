using Booking.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Data.Data
{
    public class SeedData
    {

        private static ApplicationDbContext db = default!;
        private static UserManager<ApplicationUser> userManager = default!;
        private static RoleManager<IdentityRole> roleManager = default!;

        public static async Task InitAsync(ApplicationDbContext db, IServiceProvider services, string adminPW)
        {
            if (string.IsNullOrEmpty(adminPW))
                throw new Exception("Can't get password from config");

            if (db is null) throw new NullReferenceException(nameof(ApplicationDbContext));

            if (db.Users.Any()) return;  // Seeda ej om det redan finns data på databasen

            roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            if (roleManager == null)
                throw new NullReferenceException(nameof(RoleManager<IdentityRole>));

            userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

            if (userManager == null)
                throw new NullReferenceException(nameof(UserManager<ApplicationUser>));


            //string adminRole = "Admin";
            //await roleManager.CreateAsync(new IdentityRole(adminRole));


            //var adminUser = new ApplicationUser
            //{
            //    Email = "admin@Gymbokning.se"
            //};

            //var result = await userManager.CreateAsync(adminUser);
            //if (result.Succeeded)
            //{
            //    await userManager.AddPasswordAsync(adminUser, adminPW);
            //    await userManager.AddToRoleAsync(adminUser, adminRole);
            //}

            //await db.AddRangeAsync(adminUser);


            var gymClasses = GenerateGymClasses(10);
            await db.AddRangeAsync(gymClasses);

            await db.SaveChangesAsync();

        }


        private static IEnumerable<GymClass> GenerateGymClasses(int numberOfClasses)
        {
            var gymClasses = new List<GymClass>();
            Random random = new Random();

            string[] names = { "Spinning", "Yoga", "HIT", "Dance", "Running", "Tabata", "Step" };
            string[] descriptions = { "Distance", "Beginners", "Hard", "70+", "Outdoor", "Intervalls", "Advanced" };

            TimeSpan duration = TimeSpan.FromMinutes(50);
            DateTime startTime = DateTime.Now;

            for (int i = 0; i < numberOfClasses; i++)
            {
                int index = random.Next(names.Length);

                GymClass gymClass = new GymClass
                {
                    Name = names[index],
                    StartTime = startTime,
                    Duration = duration,
                    Description= descriptions[index]
                };

                gymClasses.Add(gymClass);
            }
            return gymClasses;
        }
    }
}