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

namespace Gymboking_V._01.Controllers
{
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
        public async Task<IActionResult> Index()
        {
            return View(await _context.Gym.ToListAsync());
        }
        //-------------------------------------------------------------------------------


        public async Task<IActionResult> BokingToggel(int? id)
        {

            if (id == null) return NotFound();
            
           var ApplicationUser_Id =  _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            var userid = userManager.GetUserId(User);

            var currentgym = await _context.Gym
                .Include(a => a.AttendingMembers)
                .FirstOrDefaultAsync(g =>g.Id==id);


            var attending = currentgym.AttendingMembers.FirstOrDefault(u => u.ApplicationUserId == userid);

            if(attending == null)
            {
                var book = new ApplicationUserGymClass
                {
                    ApplicationUserId = userid,
                    GymId = currentgym.Id
                };
            _context.Add(book);
            await _context.SaveChangesAsync();
            }

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

            var gym = await _context.Gym
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gym == null)
            {
                return NotFound();
            }

            return View(gym);
        }

        // GET: Gyms/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Gyms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
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
