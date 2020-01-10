﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gymboking_V._01.Models
{
    public class Gym
    {
        public int Id{ get; set; }
        public string Name{ get; set; }
        public DateTime StartTime { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime EndTime { get { return StartTime + Duration; } }
        public string  Description { get; set; }

        public virtual ICollection<ApplicationUserGymClass> AttendingMembers { get; set; }
    }
}
