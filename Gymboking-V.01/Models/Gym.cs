using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gymboking_V._01.Models
{
    public class Gym
    {
        public int Id{ get; set; }

        [Required]
        [Display(Name = "Gym Class Name")]
        public string Name{ get; set; }

        [Required]
        [Display(Name = "Start date")]
        public DateTime StartTime { get; set; }

        [Required]
        [Display(Name = "Class Duratione")]
        public TimeSpan Duration { get; set; }
        public DateTime EndTime { get { return StartTime + Duration; } }

        [Display(Name = "Class Description")]
        public string  Description { get; set; }

        public virtual ICollection<ApplicationUserGymClass> AttendingMembers { get; set; }
    }
}
