/**
 * Author:    Ashton Foulger
 * Partner:   Kyle Charlton
 * Date:      9/27/22
 * Course:    CS 4540, University of Utah, School of Computing
 * Copyright: CS 4540 and Ashton Foulger - This work may not be copied for use in Academic Coursework.
 *
 * I, Ashton Foulger and Kyle Charlton, certify that I wrote this code from scratch and did 
 * not copy it in part or whole from another source.  Any references used 
 * in the completion of the assignment are cited in my README file and in
 * the appropriate method header.
 *
 * File Contents:
 *  Admin Controller Views and Code
 */

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TAApplication.Data;
using TAApplication.Models;

namespace TAApplication.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AdminController> _logger;
        private readonly UserManager<TAUser> _um;

        public AdminController(ApplicationDbContext context, ILogger<AdminController> logger, UserManager<TAUser> um)
        {
            _context = context;
            _logger = logger;
            _um = um;
        }

        public IActionResult Roles()
        {
            return View();
        }

        public IActionResult EnrollmentTrends()
        {
            var enrollment = _context.EnrollmentOverTime.Include(item => item.Course).ToList();
            HashSet<string> courseSet = new HashSet<string>();
            foreach (var item in enrollment)
            {
                string course = item.Course.Department + " " + item.Course.CourseNumber;
                if (!courseSet.Contains(course))
                    courseSet.Add(course);
            }
            ViewData["Courses"] = courseSet.ToList();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetEnrollmentData(string dept, uint courseNum, string startDate, string endDate)
        {
            DateTime start = DateTime.Parse(startDate);
            DateTime end = DateTime.Parse(endDate);

            var enrollments = _context.EnrollmentOverTime.Include(item => item.Course).Where(item => item.Course.Department == dept && item.Course.CourseNumber == courseNum && item.Date >= start && item.Date <= end);

            var data = new { items = enrollments.Select(item => new
                {
                    course = item.Course.Department + " " + item.Course.CourseNumber,
                    date = item.Date,
                    numEnrolled = item.NumEnrolled
                })
            };

            return Ok(data);
        }   

        [HttpPost]
        public async Task<IActionResult> ChangeRoles(string userID, string role, string addRemove)
        {
            // Check if userID is empty
            if (userID == "")
            {
                return BadRequest("User was not found, due to an empty ID.");
            }

            // Validate user role
            if (!(role == "Administrator" || role == "Professor" || role == "Applicant"))
            {
                return BadRequest("Invalid role specified.");
            }

            // Get current user from ID
            TAUser user = await _um.FindByIdAsync(userID);

            if (addRemove == "Add" && !(await _um.IsInRoleAsync(user, role)))
            {
                var result = await _um.AddToRoleAsync(user, role);

                if (!result.Succeeded)
                {
                    return Problem("Failed to add user to role");
                }
            }
            else if (addRemove == "Remove" && (await _um.IsInRoleAsync(user, role)))
            {
                var result = await _um.RemoveFromRoleAsync(user, role);

                if (!result.Succeeded)
                {
                    return Problem("Failed to remove user from role");
                }
            }
            else
            {
                return BadRequest("Invalid input received.");
            }

            return Ok(new { success = true, message = "Users role has been updated." });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
