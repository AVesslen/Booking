using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Core.Entities
{
#nullable disable
    public class ApplicationUserGymClass
    {
        // public int Id { get; set; } behövs ej pga kompositnyckel

        // Foreign Keys
        public int GymClassId { get; set; }
        public string ApplicationUserId { get; set; } = string.Empty;


        // Navigation properties

        public GymClass GymClass { get; set; } //= new GymClass();
        public ApplicationUser ApplicationUser { get; set; } //= new ApplicationUser();

    }
}
