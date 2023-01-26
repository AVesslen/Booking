using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Core.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName }";        
        
        // Navigation properties
        public ICollection<ApplicationUserGymClass> AttendingClassess { get; set; } = new List<ApplicationUserGymClass>();
        public ICollection<GymClass> GymClasses { get; set; } = new List<GymClass>();
    }
}
