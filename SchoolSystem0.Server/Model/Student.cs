using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace SchoolSystem0.Server.Models
{
    public class Student
    {
        [Key] // Primary Key
        public int Id { get; set; }

        [Required]
        [StringLength(100)] // Limit name length to 100 characters
        public string FullName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [StringLength(10)] // Example constraint on gender (can be "Male", "Female", etc.)
        public string Gender { get; set; }

        [Required]
        [StringLength(50)] // Length constraint for Student ID
        public string StudentId { get; set; }  // Unique Student Identifier

        // Complex object for Contact Information (Owned type)
        [Required]
        public ContactInformation ContactInformation { get; set; }

        // Complex object for School Details (Owned type)
        public SchoolDetails SchoolDetails { get; set; }

        // Navigation Properties for Grades and Costs
        public ICollection<Grade> Grades { get; set; } = new List<Grade>();
        public ICollection<Cost> Costs { get; set; } = new List<Cost>();
    }

    // Contact Information class, Owned by Student
    public class ContactInformation
    {
        [Required]
        [EmailAddress]
        [StringLength(255)] // Limit email length
        public string Email { get; set; }

        [Required]
        [Phone] // Use Phone data annotation for validation
        [StringLength(15)] // Phone number length constraint
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(500)] // Address length constraint
        public string Address { get; set; }
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
        // Remove the Student object here, keeping just the StudentId
    }

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
        public int StudentId { get; set; }
    }
}
