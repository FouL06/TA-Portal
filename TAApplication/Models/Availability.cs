/**
* Author:    Ashton Foulger
* Partner:   Kyle Charlton
* Date:      11/22/22
* Course:    CS 4540, University of Utah, School of Computing
* Copyright: CS 4540 and Ashton Foulger - This work may not be copied for use in Academic Coursework.
*
* I, Ashton Foulger and Kyle Charlton, certify that I wrote this code from scratch and did 
* not copy it in part or whole from another source.  Any references used 
* in the completion of the assignment are cited in my README file and in
* the appropriate method header.
*
* File Contents:
*  Availablilty Model Code
*/

using System.ComponentModel.DataAnnotations;
using System.Configuration;
using TAApplication.Data;

namespace TAApplication.Models
{
    public class Availability
    {
        public int ID { get; set; }
        
        public TAUser? User { get; set; }

        [RegularExpression("^(Y|N){240}$")]
        public string TimeSlots { get; set; } = "NNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNN" + //    Monday 8am-8pm
                                                "NNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNN" + //   Tuesday 8am-8pm
                                                "NNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNN" + // Wednesday 8am-8pm
                                                "NNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNN" + //  Thursday 8am-8pm
                                                "NNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNN";  //    Friday 8am-8pm
    }
}
