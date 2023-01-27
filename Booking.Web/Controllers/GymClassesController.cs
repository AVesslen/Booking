﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Booking.Core.Entities;
using Booking.Web.Data;
using Booking.Data.Data;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Booking.Web.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

namespace Booking.Web.Controllers
{
    [Authorize]
    //[Authorize(Roles = "Admin")]
    public class GymClassesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> userManager;
       // private readonly RoleManager<IdentityRole> roleManager;

        public GymClassesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            this.userManager = userManager;
            //this.roleManager = roleManager;
        }

        // GET: GymClasses
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
              return _context.GymClass != null ? 
                          View(await _context.GymClass.Include(g => g.AttendingMembers)
                                                      .ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.GymClass'  is null.");
        }



        // GET: GymClasses        
        public async Task<IActionResult> BookedClasses()
        {

            var userId = userManager.GetUserId(User);       

            var attending = await _context.GymClass.Include(g => g.AttendingMembers)
                                                   .Where(m => m.AttendingMembers.Any(a => a.ApplicationUserId == userId))
                                                   .ToListAsync();


            return _context.GymClass != null ?
                        View("BookedClasses",attending) : Problem("Entity set 'ApplicationDbContext.GymClass'  is null.");

        }


        // GET: GymClasses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.GymClass == null)
            {
                return NotFound();
            }

            var gymClass = await _context.GymClass.Include(g => g.AttendingMembers).Include(g => g.ApplicationUsers)
                                                  .FirstOrDefaultAsync(m => m.Id == id);
            if (gymClass == null)
            {
                return NotFound();
            }

            return View(gymClass);
        }

        // GET: GymClasses/Create        
        public IActionResult Create()
        {
            return View();
        }

        // POST: GymClasses/Create       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,StartTime,Duration,Description")] GymClass gymClass)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gymClass);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(gymClass);
        }

        // GET: GymClasses/Edit/5       
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.GymClass == null)
            {
                return NotFound();
            }

            var gymClass = await _context.GymClass.FindAsync(id);
            if (gymClass == null)
            {
                return NotFound();
            }
            return View(gymClass);
        }

        // POST: GymClasses/Edit/5        
        [HttpPost]
        [ValidateAntiForgeryToken]       
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,StartTime,Duration,Description")] GymClass gymClass)
        {
            if (id != gymClass.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gymClass);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GymClassExists(gymClass.Id))
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
            return View(gymClass);
        }

        // GET: GymClasses/Delete/5        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.GymClass == null)
            {
                return NotFound();
            }

            var gymClass = await _context.GymClass
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gymClass == null)
            {
                return NotFound();
            }

            return View(gymClass);
        }

        // POST: GymClasses/Delete/5        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.GymClass == null)
            {
                return Problem("Entity set 'ApplicationDbContext.GymClass'  is null.");
            }
            var gymClass = await _context.GymClass.FindAsync(id);
            if (gymClass != null)
            {
                _context.GymClass.Remove(gymClass);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GymClassExists(int id)
        {
          return (_context.GymClass?.Any(e => e.Id == id)).GetValueOrDefault();
        }


       
        public async Task<IActionResult> BookingToggle(int? id) // id:t på gympasset vi vill boka
        {
            if (id == null) return BadRequest();          

            var userId = userManager.GetUserId(User);  // behöver ej vara asynkront

            if (userId == null) return NotFound();

            var currentGymClass = await _context.GymClass.Include(g => g.AttendingMembers)
                                                         .FirstOrDefaultAsync(g => g.Id == id);

            var attending = currentGymClass?.AttendingMembers.FirstOrDefault(a => a.ApplicationUserId == userId);

            ////Omständigt när vi redan har id för class och user.Kan slå på nyckel i stället:

            //var attending = await _context.ApplicationUserGymClasses.FindAsync(id, userId);

            if (attending == null) 
            {
                var booking = new ApplicationUserGymClass
                {
                    ApplicationUserId = userId,
                    GymClassId = (int)id
                };                               

                _context.ApplicationUserGymClasses.Add(booking);
            }
            else 
            {
                _context.ApplicationUserGymClasses.Remove(attending);               
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
