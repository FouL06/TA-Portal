/**
 * Author:    Ashton Foulger
 * Partner:   Kyle Charlton
 * Date:      10/3/22
 * Course:    CS 4540, University of Utah, School of Computing
 * Copyright: CS 4540 and Ashton Foulger - This work may not be copied for use in Academic Coursework.
 *
 * I, Ashton Foulger and Kyle Charlton, certify that I wrote this code from scratch and did 
 * not copy it in part or whole from another source.  Any references used 
 * in the completion of the assignment are cited in my README file and in
 * the appropriate method header.
 *
 * File Contents:
 *  OLD Controller Views and Code
 */

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TAApplication.Data;

namespace TAApplication.Controllers
{
    [Authorize]
    public class OLDController : Controller
    {
        private readonly ILogger<OLDController> _logger;
        private readonly UserManager<TAUser> _um;

        public OLDController(ILogger<OLDController> logger, UserManager<TAUser> um)
        {
            _logger = logger;
            _um = um;
        }

        public IActionResult ApplicantList()
        {
            return View();
        }

        public IActionResult ApplicantCreate()
        {
            return View();
        }

        public IActionResult ApplicantDetails()
        {
            if (User.IsInRole("Administrator") || User.IsInRole("Professor") || User.Identity?.Name == "u0000000@utah.edu")
            {
                return View();
            }
            else
            {
                return Redirect("/Identity/Account/AccessDenied");
            }
        }
    }
}
