/**
 * Author:    Ashton Foulger
 * Partner:   Kyle Charlton
 * Date:      10/6/22
 * Course:    CS 4540, University of Utah, School of Computing
 * Copyright: CS 4540 and Ashton Foulger - This work may not be copied for use in Academic Coursework.
 *
 * I, Ashton Foulger and Kyle Charlton, certify that I wrote this code from scratch and did 
 * not copy it in part or whole from another source.  Any references used 
 * in the completion of the assignment are cited in my README file and in
 * the appropriate method header.
 *
 * File Contents:
 *  Applications Controller Views and Code
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TAApplication.Data;
using TAApplication.Models;

namespace TAApplication.Controllers
{
    public class ApplicationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<TAUser> _userManager;
        private readonly IConfiguration _configuration;

        public ApplicationsController(ApplicationDbContext context, UserManager<TAUser> um, IConfiguration config)
        {
            _context = context;
            _userManager = um;
            _configuration = config;
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Index()
        {
            int numApplications = _context.Application.Count();
            double averageGPA = 0;
            double averageHours = 0;

            foreach (var application in _context.Application)
            {
                averageGPA += application.GPA;
                averageHours += application.desiredHours;
            }
            averageGPA /= numApplications;
            averageHours /= numApplications;

            ViewData["numApplications"] = numApplications;
            ViewData["averageGPA"] = averageGPA;
            ViewData["averageHours"] = averageHours;

            return View();
        }

        // GET: Applications
        [Authorize(Roles = "Administrator,Professor")]
        public async Task<IActionResult> List()
        {
            return View(await _context.Application.Include(a => a.user).ToListAsync());
        }

        // GET: Applications/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            
            var application = _context.Application.Include(a => a.user).FirstOrDefault(a => a.ID == id);
            if (application == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (User.IsInRole("Professor") || User.IsInRole("Administrator") || (application != null && application.user == currentUser))
            {
                return View(application);
            }
            else
            {
                return Redirect("/Identity/Account/AccessDenied");
            }
        }

        // GET: Applications/Create
        [Authorize(Roles = "Applicant")]
        public IActionResult Create()
        {
            // Verify if the existing user already has an application
            var currentUser = _userManager.GetUserAsync(User).Result;
            var application = _context.Application.FirstOrDefault(a => a.user == currentUser);
            if (application != null)
            {
                return NotFound();
            }

            return View();
        }

        // POST: Applications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Applicant")]
        public async Task<IActionResult> Create([Bind("ID,currentDegree,department,GPA,desiredHours,available,semestersCompleted,statement,transferSchool,linkedInUrl")] Application application, [FromForm] List<IFormFile> resume, [FromForm] List<IFormFile> profilePicture)
        {
            // Verify if the existing user can create this application
            var currentUser = await _userManager.GetUserAsync(User);
            var existingApplication = _context.Application.FirstOrDefault(a => a.user == currentUser);
            if (existingApplication != null || ApplicationExists(application.ID))
            {
                return NotFound();
            }

            application.user = await _userManager.GetUserAsync(User);

            if (resume != null && resume.Count > 0 && !IsValidFile(resume, "Resume", out string errorMessage))
            {
                ViewData["ResumeError"] = "Error uploading resume: " + errorMessage;
            }
            else if (profilePicture != null && profilePicture.Count > 0 && !IsValidFile(profilePicture, "Photo", out errorMessage))
            {
                ViewData["ProfilePictureError"] = "Error uploading profile picture: " + errorMessage;
            }
            else if (ModelState.IsValid)
            {
                if (resume != null && resume.Count != 0)
                {
                    String filename = SaveFile(resume[0]);
                    if (filename == "")
                    {
                        ViewData["ResumeError"] = "Error uploading resume: File IO Error.";
                    }
                    else
                    {
                        application.resume = filename;
                    }
                }

                if (profilePicture != null && profilePicture.Count != 0)
                {
                    String filename = SaveFile(profilePicture[0]);
                    if (filename == "")
                    {
                        ViewData["ProfilePictureError"] = "Error uploading profile picture: File IO Error.";
                    }
                    else
                    {
                        application.profilePicture = filename;
                    }
                }

                if (!ViewData.ContainsKey("ResumeError") && !ViewData.ContainsKey("ProfilePictureError"))
                {
                    _context.Add(application);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new { id = application.ID });
                }
            }
            return View(application);
        }

        // GET: Applications/Edit/5
        [Authorize(Roles = "Applicant")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            var application = _context.Application.FirstOrDefault(a => a.user == currentUser);
            if (application == null || application.ID != id)
            {
                return NotFound();
            }

            return View(application);
        }

        // POST: Applications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Applicant")]
        public async Task<IActionResult> Edit(int id, [Bind("ID,currentDegree,department,GPA,desiredHours,available,semestersCompleted,statement,transferSchool,linkedInUrl")] Application application, List<IFormFile> resume, List<IFormFile> profilePicture)
        {
            // Initial validation to see if the current user can edit this application
            if (id != application.ID || !ApplicationExists(id))
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            var oldApplication = _context.Application.FirstOrDefault(a => a.user == currentUser);
            if (oldApplication == null || oldApplication.ID != id)
            {
                return NotFound();
            }

            // Carry over some old data
            application.CreationDate = oldApplication.CreationDate;
            application.CreatedBy = oldApplication.CreatedBy;
            application.user = oldApplication.user;

            // Parse files if provided
            if (resume != null && resume.Count > 0 && !IsValidFile(resume, "Resume", out string errorMessage))
            {
                ViewData["ResumeError"] = "Error uploading resume: " + errorMessage;
            }
            else if (profilePicture != null && profilePicture.Count > 0 && !IsValidFile(profilePicture, "Photo", out errorMessage))
            {
                ViewData["ProfilePictureError"] = "Error uploading profile picture: " + errorMessage;
            }
            else if (ModelState.IsValid)
            {
                if (resume != null && resume.Count != 0)
                {
                    String filename = SaveFile(resume[0]);
                    if (filename == "")
                    {
                        ViewData["ResumeError"] = "Error uploading resume: File IO Error.";
                    }
                    else
                    {
                        application.resume = filename;
                    }
                }
                else
                {
                    application.resume = oldApplication.resume;
                }

                if (profilePicture != null && profilePicture.Count != 0)
                {
                    String filename = SaveFile(profilePicture[0]);
                    if (filename == "")
                    {
                        ViewData["ProfilePictureError"] = "Error uploading profile picture: File IO Error.";
                    }
                    else
                    {
                        application.profilePicture = filename;
                    }
                }
                else
                {
                    application.profilePicture = oldApplication.profilePicture;
                }

                if (!ViewData.ContainsKey("ResumeError") && !ViewData.ContainsKey("ProfilePictureError"))
                {

                    
                    _context.Entry(oldApplication).CurrentValues.SetValues(application);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Details), new { id = application.ID });
                }
            }
            return View(application);
        }

        // GET: Applications/Delete/5
        [Authorize(Roles = "Applicant")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            var application = _context.Application.FirstOrDefault(a => a.user == currentUser);
            if (application != null && application.ID == id)
            {
                return View(application);
            }
            else
            {
                return Redirect("/Identity/Account/AccessDenied");
            }
        }

        // POST: Applications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Applicant")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var application = _context.Application.FirstOrDefault(a => a.user == currentUser);
            if (application != null && application.ID == id)
            {
                _context.Application.Remove(application);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(HomeController.Index));
            }
            else
            {
                return Redirect("/Identity/Account/AccessDenied");
            }
        }

        private bool ApplicationExists(int id)
        {
            return _context.Application.Any(e => e.ID == id);
        }

        private bool IsValidFile(List<IFormFile> files, String category, out String message)
        {
            message = "";

            if (files is null || files.Count != 1)
            {
                message = "One file must be uploaded at a time.";
                return false;
            }

            var file = files[0];

            if (!category.Equals("Resume") && !category.Equals("Photo"))
            {
                message = "Invalid file category.";
                return false;
            }

            if (category.Equals("Resume") && !Path.GetExtension(file.FileName).Equals(".pdf"))
            {
                message = "Invalid file. Resume filename must end in '.pdf'.";
                return false;
            }

            if (category.Equals("Photo") && !(Path.GetExtension(file.FileName).Equals(".jpg") || Path.GetExtension(file.FileName).Equals(".png") || Path.GetExtension(file.FileName).Equals(".gif")))
            {
                message = "Invalid file. Profile picture filename must end in '.jpg', '.png', or '.gif'.";
                return false;
            }

            if (file.Length <= 0 || file.Length > 10_000_000)
            {
                message = "Invalid file. File must be less than 10Mb.";
                return false;
            }

            return true;
        }

        private String SaveFile(IFormFile file)
        {
            String path;
            String filename = "";

            try
            {
                String tempname;
                do
                {
                    tempname = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + Path.GetExtension(file.FileName);
                    path = Path.Combine(_configuration["FilePath"], tempname);
                }
                while (System.IO.File.Exists(path));

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                filename = tempname;
            }
            catch
            {
            }

            return filename;
        }
    }
}
