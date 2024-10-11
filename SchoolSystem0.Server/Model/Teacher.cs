using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SchoolSystem0.Server.Models
{
    public class Teacher
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; }

        [Required]
        [Phone]
        [StringLength(15)]
        public string Contact { get; set; }

        [Required]
        [StringLength(500)]
        public string Address { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Salary { get; set; }

        // Changed from ICollection to IList
        [Required]
        public IList<string> Subjects { get; set; } = new List<string>();

        public ICollection<Class> Classes { get; set; } = new List<Class>();

        public ICollection<Student> GetStudentsByClass(int classId)
        {
            var classObj = Classes.FirstOrDefault(c => c.Id == classId);
            return classObj?.Students ?? new List<Student>();
        }
    }

    public class Class
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string ClassName { get; set; }

        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}