/**
 * Author:    Ashton Foulger
 * Partner:   Kyle Charlton
 * Date:      10/19/22
 * Course:    CS 4540, University of Utah, School of Computing
 * Copyright: CS 4540 and Ashton Foulger - This work may not be copied for use in Academic Coursework.
 *
 * I, Ashton Foulger and Kyle Charlton, certify that I wrote this code from scratch and did 
 * not copy it in part or whole from another source.  Any references used 
 * in the completion of the assignment are cited in my README file and in
 * the appropriate method header.
 *
 * File Contents:
 *  Courses Data Model
 */

using System.ComponentModel.DataAnnotations;

namespace TAApplication.Models
{
    public enum Season
    {
        Spring,
        Summer,
        Fall
    }

    public class Course
    {
        public int ID { get; set; }

        [Display(Name = "Semeseter", Prompt = "Fall", Description = "Select the semester this course is offered.")]
        [Required]
        public Season Semester { get; set; }

        [Display(Name = "Year", Prompt = "2022", Description = "Select the year this course is offered.")]
        [Required]
        [Range(2022, 3000)]
        public int Year { get; set; }

        [Display(Name = "Course Name", Prompt = "Web Software", Description = "Enter the name of the course.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Course Name must not be empty.")]
        public string CourseName { get; set; } = "";

        [Display(Name = "Department", ShortName = "Dept", Prompt = "CS", Description = "Enter the department this course belongs to.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Department must not be empty.")]
        public string Department { get; set; } = "";

        [Display(Name = "Course Number", Prompt = "4540", Description = "Enter the course number.")]
        [Required]
        public uint CourseNumber { get; set; }

        [Display(Name = "Course Section", ShortName = "Section", Prompt = "001", Description = "Enter the course section.")]
        [Required]
        public uint Section { get; set; }

        [Display(Name = "Description", Prompt = "Enter description here...", Description = "Enter the course description.")]
        [StringLength(50000)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; } = "";

        [Display(Name = "Professor UNID", Prompt = "U1234567", Description = "Enter the professor's UNID for this course.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Professor UNID must not be empty.")]
        [RegularExpression("^(U|u)[0-9]{7}$", ErrorMessage = "UNID must be formatted as a capital 'U' followed by 7 digits such as 'U1234567'")]
        public string ProfessorUNID { get; set; } = "";

        [Display(Name = "Professor Name", Description = "Enter the professor's name for this course.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Professor Name must not be empty.")]
        public string ProfessorName { get; set; } = "";

        [Display(Name = "Time Offered", Prompt = "M/W 3:00-5:00", Description = "Enter the times this course is offered.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Time Offered must not be empty.")]
        [DisplayFormat(NullDisplayText = "")]
        public string TimeOffered { get; set; } = "";

        [Display(Name = "Location", Prompt = "WEB L104", Description = "Enter where this course will be located.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Location must not be empty.")]
        [DisplayFormat(NullDisplayText = "")]
        public string Location { get; set; } = "";

        [Display(Name = "Credit Hours", Prompt = "3", Description = "Enter the amount of credit hours this course is worth.")]
        [Required]
        public uint CreditHours { get; set; }

        [Display(Name = "Students Enrolled", Prompt = "69", Description = "Enter the number of students enrolled.")]
        [Required]
        public uint NumEnrolled { get; set; }

        [Display(Name = "Course Notes", ShortName = "Notes", Prompt = "Enter notes here...", Description = "Enter any notes regarding this course.")]
        [StringLength(50000)]
        [DataType(DataType.MultilineText)]
        [DisplayFormat(NullDisplayText = "")]
        public string Notes { get; set; } = "";
    }
}
