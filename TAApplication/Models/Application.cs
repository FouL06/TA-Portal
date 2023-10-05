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
*  Application Model Code
*/

using System.ComponentModel.DataAnnotations;
using TAApplication.Data;

namespace TAApplication.Models
{
    // Current degree pursued
    public enum currentDegree
    {
        BS,
        MS,
        BSMS,
        PhD
    }

    /// <summary>
    /// Application model contains all data related to TA applications.
    /// Along with descriptions and required tags.
    /// </summary>
    public class Application : ModificationTracking
    {
        public int ID { get; set; }

        public TAUser? user { get; set; }

        [Required]
        [Display(Name = "Current Degree Pursued", ShortName = "Current Degree", Prompt = "BS", Description = "The type of degree you are pursuing. Select from the dropdown.")]
        public currentDegree currentDegree { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Department must not be empty.")]
        [Display(Name = "Department Major", ShortName = "Department", Prompt = "CS", Description = "Your enrolled major's department code.")]
        public string department { get; set; } = "";

        [Required]
        [Display(Name = "University of Utah GPA", ShortName = "GPA", Prompt = "0.0", Description = "Your current GPA at the University of Utah.")]
        [Range(0.0, 4.0)]
        public double GPA { get; set; }

        [Required]
        [Display(Name = "Desired Hours", ShortName = "Desired Hours", Prompt = "5 to 20", Description = "Your desired amount of work hours that you would like to work while being a TA. Must be within 5 to 20 hours.")]
        [Range(5, 20)]
        public int desiredHours { get; set; }

        [Required]
        [Display(Name = "Available", ShortName = "Availability", Description = "Are you available at least a week before the semester starts?")]
        public Boolean available { get; set; }

        [Required]
        [Display(Name = "Semesters Completed", ShortName = "Semesters Completed", Prompt = "0", Description = "How many semesters have you completed at the University of Utah?")]
        [Range(0, double.MaxValue)]
        public int semestersCompleted { get; set; }

        [Display(Name = "Personal Statement", ShortName = "Personal Statement", Prompt = "Tell us why you would be a great TA!", Description = "Personal statement is a chance for you to tell us more about yourself and helps us to understand what would make you a great TA!")]
        [StringLength(50000)]
        [DataType(DataType.MultilineText)]
        public string? statement { get; set; }

        [Display(Name = "Transfer School", ShortName = "Transfer School", Prompt = "SLCC", Description = "What school did you transfer from, if any?")]
        public string? transferSchool { get; set; }

        [Display(Name = "LinkedIn", ShortName = "LinkedIn", Description = "Your LinkedIn profile URL.")]
        [DisplayFormat(NullDisplayText = "")]
        [Url]
        public string? linkedInUrl { get; set; }

        [Display(Name = "Resume", ShortName = "Resume", Description = "Please upload your resume here.")]
        [DisplayFormat(NullDisplayText = "")]
        public string? resume { get; set; }

        [Display(Name = "Photo", ShortName = "Photo", Description = "Please upload a professional photo of yourself.")]
        [DisplayFormat(NullDisplayText = "")]
        public string? profilePicture { get; set; }
    }
}
