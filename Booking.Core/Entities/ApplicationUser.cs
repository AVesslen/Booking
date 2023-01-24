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
        // Navigation properties
        public ICollection<ApplicationUserGymClass> AttendingClassess { get; set; } = new List<ApplicationUserGymClass>();
        public ICollection<GymClass> GymClasses { get; set; } = new List<GymClass>();

    }
}
