using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Gymboking_V._01.Data;
using Gymboking_V._01.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Gymboking_V._01.Controllers
{
    [Authorize]
    public class GymsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> userManager;

        public GymsController(ApplicationDbContext context,UserManager<ApplicationUser> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }

        // GET: Gyms
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            //if(User.Identity.IsAuthenticated )
            //{
            //    return View(await _context.Gym)
            //}

            var model = await _context.Gym
              .Include(g => g.AttendingMembers)
              .ThenInclude(a => a.ApplicationUser)
              .IgnoreQueryFilters()
              .ToListAsync();

            return View(model);
        }
        //-------------------------------------------------------------------------------

            [Authorize(Roles ="Member")]
        public async Task<IActionResult> BookingToogle(int? id)
        {
            if (id == null) return NotFound();

            //Hämta den inloggade användarens id
            // var userId = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            var userId = userManager.GetUserId(User);

            //Hämta aktuellt gympass
            var currentGymClass = await _context.Gym
                .Include(a => a.AttendingMembers)
                .FirstOrDefaultAsync(g => g.Id == id);

            //Är den aktuella inloggade användaren bokad på passet?
            var attending = currentGymClass.AttendingMembers
                .FirstOrDefault(u => u.ApplicationUserId == userId);

            //Om inte, boka användaren på passet
            if (attending == null)
            {
                var book = new ApplicationUserGymClass
                {
                    ApplicationUserId = userId,
                    GymId = currentGymClass.Id
                };

                _context.ApplicationUserGymClass.Add(book);
                _context.SaveChanges();
            }

            //Annars avboka
            else
            {
                _context.ApplicationUserGymClass.Remove(attending);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));

        }



        //--------------------------------------------------------------------------------
        // GET: Gyms/Details/5
        
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gymClass = await _context.Gym
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gymClass == null)
            {
                return NotFound();
            }

            return View(gymClass);
        }



        // GET: Gyms/Create
        [Authorize (Roles="Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Gyms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Name,StartTime,Duration,Description")] Gym gym)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gym);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(gym);
        }

        // GET: Gyms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gym = await _context.Gym.FindAsync(id);
            if (gym == null)
            {
                return NotFound();
            }
            return View(gym);
        }

        // POST: Gyms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,StartTime,Duration,Description")] Gym gym)
        {
            if (id != gym.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gym);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GymExists(gym.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(gym);
        }

        // GET: Gyms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gym = await _context.Gym
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gym == null)
            {
                return NotFound();
            }

            return View(gym);
        }

        // POST: Gyms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gym = await _context.Gym.FindAsync(id);
            _context.Gym.Remove(gym);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GymExists(int id)
        {
            return _context.Gym.Any(e => e.Id == id);
        }

      
    }
}
