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
 *  Admin code For Enrollments Over Time Code and HighCharts
 */

"use strict";

let postInProgress = false;

function Main() {
    window.addEventListener('DOMContentLoaded', (event) => {
        document.getElementById("start-date").valueAsDate = new Date('11/01/2022');
        document.getElementById("end-date").valueAsDate = new Date();
    });
};

function GetEnrollmentData() {
    if (postInProgress)
        return;
    postInProgress = true;

    let spinner = document.getElementById('spinner');
    let button = document.getElementById('data-button');
    let course = document.getElementById('course-select').value.split(' ');
    let startDate = document.getElementById('start-date').value;
    let endDate = document.getElementById('end-date').value;

    spinner.setAttribute('style', '');
    button.disabled = true;

    $.post(
        {
            url: "/Admin/GetEnrollmentData",
            data: {
                "dept": course[0],
                "courseNum": Number(course[1]),
                "startDate": startDate,
                "endDate": endDate
            }
        }).fail(() => {
            alert("Failed to get enrollment data");
        }).done((res) => {
            let series = {
                name: res.items[0].course,
                data: res.items.map((item) => {
                    let date = new Date(item.date);
                    return [Date.UTC(date.getUTCFullYear(), date.getUTCMonth(), date.getUTCDate()), item.numEnrolled]
                })
            }
            UpdateChart(series);
        }).always(() => {
            postInProgress = false;
            button.disabled = false;
            document.getElementById('spinner').setAttribute('style', 'display: none !important');
        });
}

function UpdateChart(series) {
    let startDate = document.getElementById("start-date").valueAsDate;
    let endDate = document.getElementById("end-date").valueAsDate;
    let min = Date.UTC(startDate.getUTCFullYear(), startDate.getUTCMonth(), startDate.getUTCDate());
    let max = Date.UTC(endDate.getUTCFullYear(), endDate.getUTCMonth(), endDate.getUTCDate());

    Highcharts.chart('chart-container', {
        title: {
            text: 'Enrollments Over Time',
            align: 'center'
        },
        yAxis: {
            title: {
                text: 'Students'
            }
        },
        xAxis: {
            accessibility: {
                rangeDescription: 'Date'
            },
            title: {
                text: 'Date'
            },
            type: 'datetime',
            labels: {
                formatter: function () {
                    return Highcharts.dateFormat('%d %b %Y',
                        this.value);
                }
            },
            min: min,
            max: max
        },
        legend: {
            layout: 'vertical',
            align: 'right',
            verticalAlign: 'middle'
        },
        plotOptions: {
            series: {
                label: {
                    connectorAllowed: false
                }
            }
        },
        series: [
            series
        ],
        responsive: {
            rules: [{
                condition: {
                    maxWidth: 500
                },
                chartOptions: {
                    legend: {
                        layout: 'horizontal',
                        align: 'center',
                        verticalAlign: 'bottom'
                    }
                }
            }]
        }
    });
}

Main();
