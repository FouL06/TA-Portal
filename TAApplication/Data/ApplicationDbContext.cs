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
 *  Database Seed Data
 */

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;
using TAApplication.Controllers;
using TAApplication.Models;
using CsvHelper;
using System.IO;
using System.Globalization;

namespace TAApplication.Data
{
    public class ApplicationDbContext : IdentityDbContext<TAUser>
    {
        public IHttpContextAccessor _httpContextAccessor;
        public readonly IConfiguration _configuration;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor, IConfiguration config)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = config;
        }

        public async Task InitializeUsers(UserManager<TAUser> um, RoleManager<IdentityRole> rm)
        {
            if (rm.Roles.Count() == 3)
                return;

            await rm.CreateAsync(new IdentityRole("Administrator"));
            await rm.CreateAsync(new IdentityRole("Professor"));
            await rm.CreateAsync(new IdentityRole("Applicant"));

            TAUser user1 = new TAUser()
            {
                UserName = "admin@utah.edu",
                Unid = "u1234567",
                Name = "Admin Administrator",
                EmailConfirmed = true
            };

            await um.CreateAsync(user1, "123ABC!@#def");
            await um.AddToRoleAsync(user1, "Administrator");

            TAUser user2 = new TAUser()
            {
                UserName = "professor@utah.edu",
                Unid = "u7654321",
                Name = "St. Germain",
                EmailConfirmed = true
            };

            await um.CreateAsync(user2, "123ABC!@#def");
            await um.AddToRoleAsync(user2, "Professor");

            TAUser user3 = new TAUser()
            {
                UserName = "u0000000@utah.edu",
                Unid = "u0000000",
                Name = "John Cena",
                EmailConfirmed = true
            };

            await um.CreateAsync(user3, "123ABC!@#def");
            await um.AddToRoleAsync(user3, "Applicant");
            this.Availability.Add(new Availability()
            {
                User = user3,
                TimeSlots = "YYYYYYYYYYYYYYYYNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNN" + // Monday
                            "NNNNNNNNNNNNNNNNYYYYYYYYYYYYYYYYYYYYNNNNNNNNNNNN" + // Tuesday
                            "NNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNN" + // Wednesday
                            "NNNNNNNNNNNNNNNNYYYYYYYYYYYYYYYYYYYYNNNNNNNNNNNN" + // Thursday
                            "YYYYYYYYYYYYYYYYNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNN"   // Friday
            });

            TAUser user4 = new TAUser()
            {
                UserName = "u0000001@utah.edu",
                Unid = "u0000001",
                Name = "Ichigo Nakamura",
                EmailConfirmed = true
            };

            await um.CreateAsync(user4, "123ABC!@#def");
            await um.AddToRoleAsync(user4, "Applicant");
            this.Availability.Add(new Availability() { User = user4 });

            TAUser user5 = new TAUser()
            {
                UserName = "u0000002@utah.edu",
                Unid = "u0000002",
                Name = "Zero Two",
                EmailConfirmed = true
            };

            await um.CreateAsync(user5, "123ABC!@#def");
            await um.AddToRoleAsync(user5, "Applicant");
            this.Availability.Add(new Availability() { User = user5 });
        }

        public async Task InitializeApplications(UserManager<TAUser> um)
        {
            this.Database.EnsureCreated();

            // Find User ID's
            TAUser user1 = await um.FindByNameAsync("u0000000@utah.edu");
            TAUser user2 = await um.FindByNameAsync("u0000001@utah.edu");

            if (this.Application.Any())
            {
                return;
            }

            var application = new Application[]
            {
                new Application{ user=user1,
                    currentDegree=currentDegree.BS,
                    department="CS",
                    GPA=3.8,
                    desiredHours=20,
                    available=true,
                    semestersCompleted=2,
                    statement="I think I would be a great TA.",
                    transferSchool="SLCC",
                    linkedInUrl="https://www.linkden.com",
                    resume="resume.pdf",
                    profilePicture="user1.jpg",
                    CreationDate=DateTime.Parse("2022-10-6"),},
                new Application{ user=user2,
                    currentDegree=currentDegree.MS,
                    department="BIO",
                    GPA=3.2,
                    desiredHours=10,
                    available=false,
                    semestersCompleted=3,
                    statement="",
                    transferSchool="",
                    linkedInUrl="",
                    resume="",
                    profilePicture="",
                    CreationDate=DateTime.Parse("2022-10-6")}
            };

            foreach (Application app in application)
            {
                this.Application.Add(app);
            }
            this.SaveChanges();
        }

        public async Task InitializeCourses(UserManager<TAUser> um)
        {
            this.Database.EnsureCreated();

            TAUser professor = await um.FindByNameAsync("professor@utah.edu");

            if (this.Course.Any())
            {
                return;
            }

            var courses = new Course[]
            {
                new Course
                {
                    Semester=Season.Spring,
                    Year=2023,
                    CourseName="Intro to Computer Programming",
                    Department="CS",
                    CourseNumber=1400,
                    Section=001,
                    Description="Introductory course to computer programming in java.",
                    ProfessorUNID=professor.Unid,
                    ProfessorName=professor.Name,
                    TimeOffered="M/W 1:00-2:30",
                    Location="WEB L104",
                    CreditHours=3,
                    NumEnrolled=24,
                    Notes="Current course is only taught in java and is in need of two TA's"
                },
                new Course
                {
                    Semester=Season.Spring,
                    Year=2023,
                    CourseName="Data Structures and Algorithms",
                    Department="CS",
                    CourseNumber=2420,
                    Section=001,
                    Description="Basic introduction to data structures and algorithms using java.",
                    ProfessorUNID=professor.Unid,
                    ProfessorName=professor.Name,
                    TimeOffered="T/TH 1:00-2:30",
                    Location="WEB L105",
                    CreditHours=3,
                    NumEnrolled=81,
                    Notes="Current course is only taught in java and is in need of five TA's for grading and assistance in teaching."
                },
                new Course
                {
                    Semester=Season.Spring,
                    Year=2023,
                    CourseName="Software Practice I",
                    Department="CS",
                    CourseNumber=3500,
                    Section=001,
                    Description="Covers the basics of software development best practices and will involve creating a spreadsheet application.",
                    ProfessorUNID=professor.Unid,
                    ProfessorName=professor.Name,
                    TimeOffered="M/W/F 11:00-12:00",
                    Location="WEB L3201",
                    CreditHours=3,
                    NumEnrolled=144,
                    Notes=""
                },
                new Course
                {
                    Semester=Season.Spring,
                    Year=2023,
                    CourseName="Software Practice II",
                    Department="CS",
                    CourseNumber=3505,
                    Section=001,
                    Description="Continues software development best practices and will be taught in C++.",
                    ProfessorUNID=professor.Unid,
                    ProfessorName=professor.Name,
                    TimeOffered="M/W/F 3:00-3:50",
                    Location="L105",
                    CreditHours=3,
                    NumEnrolled=69,
                    Notes=""
                },
                new Course
                {
                    Semester=Season.Spring,
                    Year=2023,
                    CourseName="Computer Systems",
                    Department="CS",
                    CourseNumber=4400,
                    Section=001,
                    Description="Discuss topics of how computers on a low level and involves bit manipulation and optimization.",
                    ProfessorUNID=professor.Unid,
                    ProfessorName=professor.Name,
                    TimeOffered="F 9:00-12:00",
                    Location="WEB L101",
                    CreditHours=3,
                    NumEnrolled=215,
                    Notes="Extra TA's needed, with large amounts of students needing to take this course."
                }
            };

            foreach (Course course in courses)
            {
                this.Course.Add(course);
            }

            this.SaveChanges();

            InitializeEnrollment(courses);
        }

        public void InitializeEnrollment(Course[] courses)
        {
            this.Database.EnsureCreated();
            if (this.EnrollmentOverTime.Any())
            {
                return;
            }

            List<string> dates = new List<string>() {
                " Nov 1", " Nov 2", " Nov 3", " Nov 4", " Nov 5",
                " Nov 6", " Nov 7", " Nov 8", " Nov 9", " Nov 10",
                " Nov 11", " Nov 12", " Nov 13", " Nov 14", " Nov 15",
                " Nov 16", " Nov 17", " Nov 18", " Nov 19", " Nov 20",
                " Nov 21", " Nov 22", " Nov 23", " Nov 24", " Nov 25",
                " Nov 26", " Nov 27", " Nov 28", " Nov 29" };

            string userDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string fullDir = userDir + @"/source/repos/TAApplication/TAApplication/wwwroot/uploads/temp.csv";
            using (var streamReader = new StreamReader(fullDir))
            using (var csv = new CsvReader(streamReader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();

                while (csv.Read())
                {
                    var courseStrings = csv.GetField<string>("Course").Split(" ")[1];

                    foreach (Course c in courses)
                    {
                        if (c.CourseNumber == int.Parse(courseStrings))
                        {
                            for (int i = 1; i <= 29; i++)
                            {
                                this.EnrollmentOverTime.Add(new EnrollmentOverTime
                                {
                                    Course = c,
                                    Date = new DateTime(2022, 11, i),
                                    NumEnrolled = csv.GetField<int>(dates[i - 1])
                                });
                            }

                            this.SaveChanges();
                        }
                    }
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Application>().ToTable("Application");
        }

        /// <summary>
        /// Every time Save Changes is called, add timestamps
        /// </summary>
        /// <returns></returns>
        public override int SaveChanges()  // JIM: Override save changes to add timestamps
        {
            AddTimestamps();
            return base.SaveChanges();
        }
        /// <summary>
        /// Every time Save Changes (Async) is called, add timestamps
        /// </summary>
        /// <returns></returns>
        public override async Task<int> SaveChangesAsync(System.Threading.CancellationToken cancellationToken = default)
        {
            AddTimestamps();   // JIM: Override save changes async to add timestamps
            return await base.SaveChangesAsync(cancellationToken);
        }
        /// <summary>
        /// JIM: this code adds time/user to DB entry
        /// 
        /// Check the DB tracker to see what has been modified, and add timestamps/names as appropriate.
        /// 
        /// </summary>
        private void AddTimestamps()
        {
            var entities = ChangeTracker.Entries()
                .Where(x => x.Entity is ModificationTracking
                        && (x.State == EntityState.Added || x.State == EntityState.Modified));

            var currentUsername = "";

            if (_httpContextAccessor.HttpContext == null) // happens during startup/initialization code
            {
                currentUsername = "DBSeeder";
            }
            else
            {
                currentUsername = _httpContextAccessor.HttpContext.User.Identity?.Name;
            }

            currentUsername ??= "Sadness"; // JIM: compound assignment magic... test for null, and if so, assign value

            foreach (var entity in entities)
            {
                if (entity.State == EntityState.Added)
                {
                    ((ModificationTracking)entity.Entity).CreationDate = DateTime.UtcNow;
                    ((ModificationTracking)entity.Entity).CreatedBy = currentUsername;
                }
                ((ModificationTracking)entity.Entity).ModificationDate = DateTime.UtcNow;
                ((ModificationTracking)entity.Entity).ModifiedBy = currentUsername;
            }
        }
        /// <summary>
        /// JIM: this code adds time/user to DB entry
        /// 
        /// Check the DB tracker to see what has been modified, and add timestamps/names as appropriate.
        /// 
        /// </summary>
        public DbSet<TAApplication.Models.Application> Application { get; set; }
        /// <summary>
        /// JIM: this code adds time/user to DB entry
        /// 
        /// Check the DB tracker to see what has been modified, and add timestamps/names as appropriate.
        /// 
        /// </summary>
        public DbSet<TAApplication.Models.Course> Course { get; set; }
        /// <summary>
        /// JIM: this code adds time/user to DB entry
        /// 
        /// Check the DB tracker to see what has been modified, and add timestamps/names as appropriate.
        /// 
        /// </summary>
        public DbSet<TAApplication.Models.Availability> Availability { get; set; }

        public DbSet<TAApplication.Models.EnrollmentOverTime> EnrollmentOverTime { get; set; }
    }
}