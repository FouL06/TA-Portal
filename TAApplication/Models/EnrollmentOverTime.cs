/**
* Author:    Ashton Foulger
* Partner:   Kyle Charlton
* Date:      12/1/22
* Course:    CS 4540, University of Utah, School of Computing
* Copyright: CS 4540 and Ashton Foulger - This work may not be copied for use in Academic Coursework.
*
* I, Ashton Foulger and Kyle Charlton, certify that I wrote this code from scratch and did 
* not copy it in part or whole from another source.  Any references used 
* in the completion of the assignment are cited in my README file and in
* the appropriate method header.
*
* File Contents:
*  Enrollment Model Code
*/

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace TAApplication.Models
{
    public class EnrollmentOverTime
    {
        public int ID { get; set; }
        public Course Course { get; set; }
        public DateTime Date { get; set; }
        public int NumEnrolled { get; set; }
    }
}
