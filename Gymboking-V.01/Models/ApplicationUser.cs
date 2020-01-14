using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gymboking_V._01.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get { return FirstName + "" + LastName; } }
        public DateTime TimeOfRegistration { get; set; }
        public  ICollection<ApplicationUserGymClass> AttendingClasses { get; set; }
    }
}
