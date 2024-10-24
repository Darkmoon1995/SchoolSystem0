using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolSystem0.Server.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [StringLength(10)]
        public string Gender { get; set; }

        [Required]
        [StringLength(50)]
        public string StudentId { get; set; }

        // Complex object for Contact Information (Owned type)
        [Required]
        public ContactInformation ContactInformation { get; set; }

        // Complex object for School Details (Owned type)
        public SchoolDetails SchoolDetails { get; set; }

        // Navigation Properties for Grades and Costs
        public ICollection<Grade> Grades { get; set; } = new List<Grade>();
        public ICollection<Cost> Costs { get; set; } = new List<Cost>();

        // List to track student absences
        public List<DateTime> Absences { get; set; } = new List<DateTime>();
    }

    // Contact Information class, Owned by Student
    public class ContactInformation
    {
        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; }

        [Required]
        [Phone]
        [StringLength(15)]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(500)]
        public string Address { get; set; }

        // Latitude and Longitude for location tracking
        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }
    }

    // School Details class, Owned by Student
    public class SchoolDetails
    {
        [Required]
        [StringLength(255)] // School name length constraint
        public string SchoolName { get; set; }

        [StringLength(100)] // Class name length constraint
        public string ClassName { get; set; }

        [StringLength(10)] // Grade length constraint (e.g., "A", "B", "10th")
        public string Grade { get; set; }
    }

    // Grade class to store test results for students
    public class Grade
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string TestName { get; set; }

        [Required]
        [Range(0, 100)]
        public int GradeValue { get; set; }

        // Foreign key for Student
        public int StudentId { get; set; }
    }

    // Cost class to store expenses for students
    public class Cost
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Description { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Amount { get; set; }

        [Required]
        public DateTime DateIncurred { get; set; }

        // Foreign key for Student
        public int StudentId { get; set; }
    }

    // Class to handle Neshan API response for directions
    public class NeshanDirectionResponse
    {
        public Route[] Routes { get; set; }

        public class Route
        {
            public Leg[] Legs { get; set; }
        }

        public class Leg
        {
            public Distance Distance { get; set; }
            public Duration Duration { get; set; }
        }

        public class Distance
        {
            public double Value { get; set; } // Distance in meters
        }

        public class Duration
        {
            public double Value { get; set; } // Duration in seconds
        }
    }
}
