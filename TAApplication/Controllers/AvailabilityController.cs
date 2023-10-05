/**
 * Author:    Ashton Foulger
 * Partner:   Kyle Charlton
 * Date:      11/17/22
 * Course:    CS 4540, University of Utah, School of Computing
 * Copyright: CS 4540 and Ashton Foulger - This work may not be copied for use in Academic Coursework.
 *
 * I, Ashton Foulger and Kyle Charlton, certify that I wrote this code from scratch and did 
 * not copy it in part or whole from another source.  Any references used 
 * in the completion of the assignment are cited in my README file and in
 * the appropriate method header.
 *
 * File Contents:
 *  Slots Controller Views and Code
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TAApplication.Data;
using TAApplication.Models;

namespace TAApplication.Controllers
{
    public class AvailabilityController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<TAUser> _userManager;

        public AvailabilityController(ApplicationDbContext context, UserManager<TAUser> um)
        {
            _context = context;
            _userManager = um;
        }

        // GET: Availability
        public async Task<IActionResult> Index(int id)
        {
            return await GetSchedule(id);
        }

        [HttpPost]
        public async Task<IActionResult> SetSchedule([Bind("ID,TimeSlots")] Availability availability)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var oldAvailability = await _context.Availability.FirstOrDefaultAsync(m => m.ID == availability.ID);

            if (currentUser != oldAvailability?.User)
            {
                return BadRequest("This user cannot update the specified availability.");
            }
            else if (ModelState.IsValid)
            {
                _context.Entry(oldAvailability).CurrentValues.SetValues(availability);
                _context.SaveChanges();
                return Ok(new { success = true, message = "User availability has been updated." });
            }

            return Problem("Failed to update user availability."); ;
        }

        public async Task<IActionResult> GetSchedule(int id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var availability = await _context.Availability.FirstOrDefaultAsync(m => m.ID == id);

            if (currentUser == null)
                ViewData["CanEdit"] = false;
            else
                ViewData["CanEdit"] = currentUser == availability?.User;

            return View(availability);
        }
    }
}
