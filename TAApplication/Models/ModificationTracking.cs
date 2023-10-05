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
 *  Tracking Time updates code.
 */

using System.ComponentModel.DataAnnotations;

namespace TAApplication.Models
{
    public class ModificationTracking
    {

        [ScaffoldColumn(false)]
        public DateTime CreationDate { get; set; }

        [ScaffoldColumn(false)]
        public DateTime ModificationDate { get; set; }

        [ScaffoldColumn(false)]
        public String? CreatedBy { get; set; }

        [ScaffoldColumn(false)]
        public String? ModifiedBy { get; set; }

    }
}
