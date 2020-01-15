using System;
using System.Collections.Generic;
using System.Text;
using Gymboking_V._01.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Gymboking_V._01.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser,IdentityRole,string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        public DbSet<Gymboking_V._01.Models.ApplicationUserGymClass> ApplicationUserGymClass { get; set; }

        public DbSet<Gym> Gym { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ApplicationUserGymClass>()
            .HasKey(t => new { t.ApplicationUserId, t.GymId });

            //Global Filter
            modelBuilder.Entity<Gym>().HasQueryFilter(g => g.StartTime > DateTime.Now);
        }


        
    }
}
