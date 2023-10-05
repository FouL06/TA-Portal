/**
 * Author:    Ashton Foulger
 * Partner:   Kyle Charlton
 * Date:      9/28/22
 * Course:    CS 4540, University of Utah, School of Computing
 * Copyright: CS 4540 and Ashton Foulger - This work may not be copied for use in Academic Coursework.
 *
 * I, Ashton Foulger and Kyle Charlton, certify that I wrote this code from scratch and did 
 * not copy it in part or whole from another source.  Any references used 
 * in the completion of the assignment are cited in my README file and in
 * the appropriate method header.
 *
 * File Contents:
 *  Admin code For Roles page
 */

$(document).ready(function () {
    $('#table_id').DataTable();
});

function ChangeRoles(userID, role, checkbox) {
    $.post(
        {
            url: "/Admin/ChangeRoles",
            data: {
                "userID": userID,
                "role": role,
                "addRemove": checkbox.checked ? "Add" : "Remove"
            }
        }).fail(() => {
            checkbox.checked = !checkbox.checked;
        });
}