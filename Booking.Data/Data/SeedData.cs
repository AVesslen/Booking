using Bogus;
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

        public static async Task InitAsync(ApplicationDbContext context, IServiceProvider services, string adminPW)
        {
            if (context is null) throw new NullReferenceException(nameof(ApplicationDbContext));
            db = context;

            if (string.IsNullOrEmpty(adminPW))
                throw new Exception("Can't get password from config");

            if (db.Users.Any()) return;  // Seeda ej om det redan finns data på databasen

            ArgumentNullException.ThrowIfNull(nameof(services));

            roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            if (roleManager == null) throw new NullReferenceException(nameof(RoleManager<IdentityRole>));

            userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            if (userManager == null) throw new NullReferenceException(nameof(UserManager<ApplicationUser>));

            var gymClasses = GenerateGymClasses(15);
            await db.AddRangeAsync(gymClasses);
            await db.SaveChangesAsync();

            var roleNames = new[] { "Member", "Admin" };
            var adminEmail = "admin@Gymbokning.se";
            var adminFirstName = "Kalle";
            var adminLastName = "Anka";

            await AddRolesAsync(roleNames);

            var admin = await AddAdminAsync(adminEmail, adminFirstName, adminLastName, adminPW);

            await AddToRolesAsync(admin, roleNames);


            //var rr = await roleManager.CreateAsync(new IdentityRole { Name = "Admin" });
            //var r =  await userManager.AddToRoleAsync(m, "Admin");

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
        }

        private static async Task AddToRolesAsync(ApplicationUser admin, string[] roleNames)
        {
            foreach (var roleName in roleNames)
            {
                if (await userManager.IsInRoleAsync(admin, roleName)) continue;
                var result = await userManager.AddToRoleAsync(admin, roleName);
                if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));
            }
        }

        private static async Task<ApplicationUser> AddAdminAsync(string adminEmail, string adminFirstName, string adminLastName, string adminPW)
        {
            var foundUser = await userManager.FindByEmailAsync(adminEmail);

            if (foundUser != null) return null!;

            var admin = new ApplicationUser
            {
                UserName= adminEmail,
                Email = adminEmail,
                FirstName = adminFirstName,
                LastName = adminLastName
            };

            var result = await userManager.CreateAsync(admin, adminPW);
            if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));

            return admin;
        }

        private static async Task AddRolesAsync(string[] roleNames)
        {
            foreach (var roleName in roleNames)
            {
                if (await roleManager.RoleExistsAsync(roleName)) continue;
                var role = new IdentityRole { Name = roleName };
                var result = await roleManager.CreateAsync(role);

                if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));
            }
        }


        private static IEnumerable<GymClass> GenerateGymClasses(int numberOfClasses)
        {
            var faker = new Faker("sv");
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
                    Description = descriptions[index]
                };

                gymClasses.Add(gymClass);
            }
            return gymClasses;
        }
    }
}


