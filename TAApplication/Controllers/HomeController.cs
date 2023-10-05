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
 *  Home Controller Views and Code
 */

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using TAApplication.Data;
using TAApplication.Models;

namespace TAApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<TAUser> _um;
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger, UserManager<TAUser> um)
        {
            _logger = logger;
            _um = um;
            _context = context;
        }

        public IActionResult Index()
        {
            if (User.IsInRole("Applicant"))
            {
                var currentUser = _um.GetUserAsync(User).Result;
                if (currentUser is null)
                    return View();

                var application = _context.Application.FirstOrDefault(a => a.user == currentUser);
                if (application is null)
                    return View();

                ViewData["ApplicationID"] = application.ID;
            }

            return View();
        }

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}