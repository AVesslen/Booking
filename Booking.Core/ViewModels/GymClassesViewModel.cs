using Booking.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Core.ViewModels
{
    public class GymClassesViewModel
    {
        public int Id { get; set; }

        [StringLength(maximumLength: 40, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public DateTime? StartTime { get; set; }

        [Required]        
        public TimeSpan? Duration { get; set; }

        public bool Attending { get; set; }     

    }
}
