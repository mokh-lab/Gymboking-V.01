using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gymboking_V._01.Models
{
    public class ApplicationUser : IdentityUser
    {

        public virtual ICollection<ApplicationUserGymClass> AttendingClasses { get; set; }
    }
}
