using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Core.Entities
{
    public class GymClass
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public TimeSpan Duration { get; set; }

        public DateTime EndTime => StartTime + Duration;

        public string Description { get; set; } = string.Empty;

        // Navigation properties
        public ICollection<ApplicationUserGymClass> AttendingMembers { get; set; } = new List<ApplicationUserGymClass>();
        public ICollection<ApplicationUser> ApplicationUsers { get; set; } = new List<ApplicationUser>();

    }
}
