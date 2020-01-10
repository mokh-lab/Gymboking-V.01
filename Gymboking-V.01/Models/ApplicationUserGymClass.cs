using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Gymboking_V._01.Models
{
    public class ApplicationUserGymClass
    {
        //  [Key, Column(Order = 0)]
        public int GymId { get; set; }

        // [Key, Column(Order = 1)]
        public string ApplicationUserId { get; set; }


        public Gym Gym { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
